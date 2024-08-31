using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Books.RequestPayloads;
using ELib_IDSFintech_Internship.Models.Sessions;
using ELib_IDSFintech_Internship.Models.Users;
using ELib_IDSFintech_Internship.Models.Users.RequestPayloads;
using ELib_IDSFintech_Internship.Repositories;
using ELib_IDSFintech_Internship.Repositories.Users;
using ELib_IDSFintech_Internship.Services.Books;
using ELib_IDSFintech_Internship.Services.Enums;
using ELib_IDSFintech_Internship.Services.Sessions;
using ELib_IDSFintech_Internship.Services.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;

namespace ELib_IDSFintech_Internship.Services.Users
{
    public class UserRepository : IUserRepository
    {

        private readonly Data.ELibContext _context;
        private readonly ILogger<UserRepository> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly AES256Encryption _securityAES;
        private readonly BookRepository _bookService;
        private readonly SessionManagementRepository _sessionManager;

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "User";



        public UserRepository(Data.ELibContext context, ILogger<UserRepository> logger, IMemoryCache memoryCache, AES256Encryption securityAES, BookRepository bookService, SessionManagementRepository sessionManager)
        {
            _context = context;
            _logger = logger;
            _memoryCache = memoryCache;
            _securityAES = securityAES;
            _bookService = bookService;
            _sessionManager = sessionManager;
        }
        

        //need to add more checks later for user and session ID
        public async Task<BookActionResponse?> BorrowBook(BorrowBookRequest request)
        {
            _logger.LogInformation($"Borrowing a {_logName}, Service Layer");
            var response = new BookActionResponse();
            try
            {
                var user = await _context.Users.Where(u => u.Id == request.UserId).Include(l => l.Subscription).Include(b => b.UserBooks).ThenInclude(b => b.Book).FirstOrDefaultAsync();
                var latestBook = await _context.Books.Where(u => u.Id == request.BookId).FirstOrDefaultAsync();

                if (user == null)
                {
                    _logger.LogInformation($"No {_logName} found");
                    response.Status = (int)ResponseType.NoObjectFound;
                    response.Message = $"No {_logName} found";

                    return response;
                }
                if (latestBook == null)
                {
                    _logger.LogInformation($"No Book found");
                    response.Status = (int)ResponseType.NoObjectFound;
                    response.Message = $"No Book found";

                    return response;
                }
                else
                {
                    _logger.LogInformation($"Testing to see if we have enough books");
                    if(latestBook.PhysicalBookAvailability == false && latestBook.Type == "Physical")
                    {
                        _logger.LogInformation($"Not enough books");
                        response.Status = (int)ResponseType.OutOfBook;
                        response.Message = $"Not enough books";

                        return response;
                    }
                    else
                    {
                        _logger.LogInformation($"We have enough of this book");
                    }
                }
                if (user.UserBooks.Any(ub => ub.Book.Id == latestBook.Id)) 
                {
                    _logger.LogInformation($"User is already borrowing this Book");
                    response.Status = (int)ResponseType.UserAlreadyBorrow;
                    response.Message = $"User is already borrowing this book";

                    return response;
                }
                if (user.Subscription == null || user.Subscription.Type == "None")
                {
                    _logger.LogInformation($"No {_logName} subscription found");
                    response.Status = (int)ResponseType.SubscriptionNeeded;
                    response.Message = $"No {_logName} subscription found";

                    return response;
                }

                //Here we compare session IDs
                var session = new SessionActionRequest((int)request.UserId, request.SessionID);
                var sessionId = await _sessionManager.EqualSessionIds(session);

                //we prepare response based on the result
                if ((bool)sessionId && sessionId != null)
                {
                    //preparing return date
                    var returnDate = DateTime.Now;
                    switch (user.Subscription.Type)
                    {
                        case "None":
                            returnDate = DateTime.Now;
                            break;

                        case "Basic":
                            returnDate = DateTime.Now.AddDays(14);
                            break;

                        case "Advanced":
                            returnDate = DateTime.Now.AddDays(60);
                            break;

                        case "Premium":
                            returnDate = DateTime.MaxValue;
                            break;

                    }

                    if (latestBook.Type == "Physical")
                    {
                        var newBorrow = new UserHasBooks
                        {
                            UserId = user.Id,
                            BookId = latestBook.Id,
                            BorrowedDate = DateTime.Now,
                            DueDate = returnDate

                        };

                        user.UserBooks.Add(newBorrow);

                        latestBook.PhysicalBookCount--;

                        if (latestBook.PhysicalBookCount == 0)
                        {
                            latestBook.PhysicalBookAvailability = false;
                        }

                        _context.Entry(latestBook).State = EntityState.Modified;

                    }
                    else
                    {
                        var newBorrow = new UserHasBooks
                        {
                            UserId = user.Id,
                            BookId = latestBook.Id,
                            BorrowedDate = DateTime.Now,
                            DueDate = returnDate

                        };

                        user.UserBooks.Add(newBorrow);
                    }

                    //returns how many entries were Created (should be 1)
                    var count = await _context.SaveChangesAsync();

                    //clearing cache
                    await ClearCache($"User_{user.Id}");
                    //we also need to clear the cache of the book because we updated its availability / quantity
                    await _bookService.ClearCache($"Book_{request.BookId}");

                    var user2 = await _context.Users.Where(u => u.Id == request.UserId).Include(l => l.Subscription).Include(b => b.UserBooks).ThenInclude(b => b.Book).FirstOrDefaultAsync();
                    var book2 = await _context.Books.Where(u => u.Id == request.BookId).FirstOrDefaultAsync();


                    if (count > 0)
                    {
                        response.Status = (int)ResponseType.ResponseSuccess;
                        response.Message = $"Successfully borrowed book";
                        response.User = user2;

                        return response;
                    }
                    else
                    {
                        response.Status = (int)ResponseType.FailedToCreate;
                        response.Message = $"Failed to unborrow the book from the {_logName}";

                        return response;
                    }
                }
                else
                {
                    response.Status = (int)ResponseType.FailedToCreate;
                    response.Message = $"Failed to unborrow the book from the {_logName}";

                    return response;
                }

                    

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to borrow the {_logName}, in Service Layer");
                throw ex;
            }
        }

        //need to add more checks later for user and session ID
        public async Task<BookActionResponse?> UnborrowBook(BorrowBookRequest request)
        {
            _logger.LogInformation($"Unborrowing a {_logName}, Service Layer");
            var response = new BookActionResponse();
            try
            {
                var user = await _context.Users.Where(u => u.Id == request.UserId).Include(l => l.Subscription).Include(b => b.UserBooks).ThenInclude(b => b.Book).FirstOrDefaultAsync();
                var book = await _context.Books.Where(u => u.Id == request.BookId).FirstOrDefaultAsync();
                var latestUserBook = user.UserBooks.Where(user => user.UserId == request.UserId && user.BookId == request.BookId).FirstOrDefault();

                var count = 0;


                if (user == null)
                {
                    _logger.LogInformation($"No {_logName} found");
                    response.Status = (int)ResponseType.NoObjectFound;
                    response.Message = $"No {_logName} found";

                    return response;
                }
                if(book == null)
                {
                    _logger.LogInformation($"No Book found");
                    response.Status = (int)ResponseType.NoObjectFound;
                    response.Message = $"No book found";

                    return response;
                }
                if (latestUserBook == null)
                {
                    _logger.LogInformation($"No UserBook found");
                    response.Status = (int)ResponseType.NoObjectFound;
                    response.Message = $"No userbook found";

                    return response;
                }

                //Here we compare session IDs
                var session = new SessionActionRequest((int)request.UserId, request.SessionID);
                var sessionId = await _sessionManager.EqualSessionIds(session);

                //we prepare response based on the result
                if (sessionId.GetValueOrDefault() && sessionId != null)
                {
                    if (book.Type == "Physical")
                    {

                        book.PhysicalBookCount++;

                        if (book.PhysicalBookCount >= 1)
                        {
                            book.PhysicalBookAvailability = true;
                        }

                        _context.Entry(book).State = EntityState.Modified;

                        count = await _context.SaveChangesAsync();

                        user.UserBooks.Remove(latestUserBook);
                    }
                    else
                    {
                        user.UserBooks.Remove(latestUserBook);
                    }

                    //returns how many entries were Created (should be 1)
                    count = await _context.SaveChangesAsync();


                    //clearing cache
                    await ClearCache($"User_{user.Id}");
                    //we also need to clear the cache of the book because we updated its availability / quantity
                    await _bookService.ClearCache($"Book_{request.BookId}");

                    var user2 = await _context.Users.Where(u => u.Id == request.UserId).Include(l => l.Subscription).Include(b => b.UserBooks).ThenInclude(b => b.Book).FirstOrDefaultAsync();
                    var book2 = await _context.Books.Where(u => u.Id == request.BookId).FirstOrDefaultAsync();


                    

                    if (count > 0)
                    {
                        response.Status = (int)ResponseType.ResponseSuccess;
                        response.Message = $"Successfully unborrowed the book from the {_logName}";
                        response.User = user2;
                        return response;

                    }
                    else
                    {
                        response.Status = (int)ResponseType.FailedToCreate;
                        response.Message = $"Failed to unborrow the book from the {_logName}";
                        return response;
                    }
                }
                else
                {
                    response.Status = (int)ResponseType.FailedToCreate;
                    response.Message = $"Failed to unborrow the book from the {_logName}";
                    return response;
                }



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to unborrow the {_logName}, in Service Layer");
                throw ex;
            }
        }

        public async Task<UserActionResponse?> Create(User newObject)
        {
            _logger.LogInformation($"Creating a {_logName}, Service Layer");
            var response = new UserActionResponse();
            try
            {
                //first we encrypt the hash password
                newObject.Password = _securityAES.Encrypt(newObject.Password);

                _context.Users.Add(newObject);

                //returns how many entries were Created (should be 1)
                await _context.SaveChangesAsync();

                var getUpdated = await _context.Users.Where(x => x.Id == newObject.Id).FirstOrDefaultAsync();

                //Here we generate session ID
                var sessionId = await _sessionManager.GenerateSessionId(getUpdated.Id);

                //we prepare response based on the result
                if (sessionId != null)
                {
                    response.Status = (int)ResponseType.ResponseSuccess;
                    response.Message = $"Successfully created the {_logName}";
                    response.User = getUpdated;
                    response.SessionID = sessionId;
                }
                else
                {
                    response.Status = (int)ResponseType.FailedToCreate;
                    response.Message = $"Failed to create the {_logName}";
                    response.User = getUpdated;
                    response.SessionID = sessionId;
                }

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to create the {_logName}, in Service Layer");
                throw ex;
            }
        }

        public async Task<UserActionResponse?> Delete(int id)
        {

            _logger.LogInformation($"Deleting a {_logName}, Service Layer");
            var response = new UserActionResponse();
            try
            {
                var entity = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (entity == null)
                {
                    _logger.LogInformation($"No {_logName} found");
                    response.Status = (int)ResponseType.NoObjectFound;
                    response.Message = $"Couldn't find a {_logName} with ID: {id}";
                    return response;
                }
                _context.Users.Remove(entity);


                //returns how many entries were updated (should be 1 if it found the location that needs updating)
                var result = await _context.SaveChangesAsync();


                //neccessairy to clear the cache after a delete
                await ClearCache($"User_{id}");

                //we prepare response based on the result
                if (result > 0)
                {
                    response.Status = (int)ResponseType.ResponseSuccess;
                    response.Message = $"Successfully deleted the {_logName}";
                }
                else
                {
                    response.Status = (int)ResponseType.FailedToDelete;
                    response.Message = $"Failed to delete the {_logName}";
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete the {_logName}, in Service Layer");
                throw ex;
            }

        }

        public async Task<IEnumerable<User>?> GetAll()
        {
            _logger.LogInformation($"Getting all {_logName}s information, Service Layer");

            var cacheKey = $"User_all";

            try
            {
                if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<User>? cachedUsers))
                {
                    _logger.LogInformation($"{_logName}s retrieved from cache");
                    return cachedUsers;
                }
                else
                {
                    _logger.LogInformation($"{_logName}s not found in cache");

                    cachedUsers = await _context.Users
                        .Include(u => u.Subscription)
                        .Include(u => u.UserBooks)
                        .ThenInclude(b => b.Book)
                        .ThenInclude(b => b.Author)
                        .Include(u => u.UserBooks)
                        .ThenInclude(b => b.Book)
                        .ThenInclude(b => b.PhysicalBookLocation)
                        .Include(u => u.UserBooks)
                        .ThenInclude(b => b.Book)
                        .ThenInclude(b => b.Formats)
                        .Include(u => u.UserBooks)
                        .ThenInclude(b => b.Book)
                        .ThenInclude(b => b.Languages)
                        .Include(u => u.UserBooks)
                        .ThenInclude(b => b.Book)
                        .ThenInclude(b => b.Genres)
                        .Include(u => u.UserBooks)
                        .ThenInclude(b => b.Book)
                        .ThenInclude(b => b.Tags).ToListAsync();


                    //Setting behavior of the cached items after a certain passed time
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetPriority(CacheItemPriority.Normal);

                    _memoryCache.Set(cacheKey, cachedUsers, cacheEntryOptions);
                }

                return cachedUsers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get all {_logName}s in Services Layer");
                throw ex;
            }
        }

        public async Task<User?> GetById(int id)
        {
            _logger.LogInformation($"Getting a single {_logName} using his ID: {id}, Service Layer");


            var cacheKey = $"User_${id}";

            try
            {
                /*if (_memoryCache.TryGetValue(cacheKey, out User? cachedUser))
                {
                    _logger.LogInformation($"{_logName} retrieved from cache");
                    return cachedUser;
                }
                else
                {*/
                    _logger.LogInformation($"{_logName}s not found in cache");

                     var cachedUser = await _context.Users.Where(l => l.Id == id)
                        .Include(u => u.Subscription)
                        .Include(u => u.CreditCard)
                        .Include(u => u.UserBooks)
                        .ThenInclude(b => b.Book)
                        .ThenInclude(b => b.Author)
                        .Include(u => u.UserBooks)
                        .ThenInclude(b => b.Book)
                        .ThenInclude(b => b.PhysicalBookLocation)
                        .Include(u => u.UserBooks)
                        .ThenInclude(b => b.Book)
                        .ThenInclude(b => b.Formats)
                        .Include(u => u.UserBooks)
                        .ThenInclude(b => b.Book)
                        .ThenInclude(b => b.Languages)
                        .Include(u => u.UserBooks)
                        .ThenInclude(b => b.Book)
                        .ThenInclude(b => b.Genres)
                        .Include(u => u.UserBooks)
                        .ThenInclude(b => b.Book)
                        .ThenInclude(b => b.Tags).FirstOrDefaultAsync();


                    /*//Setting behavior of the cached items after a certain passed time
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetPriority(CacheItemPriority.Low);

                    _memoryCache.Set(cacheKey, cachedUser, cacheEntryOptions);*/

                    return cachedUser;
                /*}*/
 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get the {_logName} with supposed ID: {id}, in Services Layer");
                throw ex;
            }
        }

        public async Task<UserActionResponse?> Update(User modifiedObject)
        {
            _logger.LogInformation($"Updating a {_logName}, Service Layer");
            var response = new UserActionResponse();
            try
            {
                //clearing cache
                await ClearCache($"User_{modifiedObject.Id}");

                //first we encrypt password
                modifiedObject.Password = _securityAES.Encrypt(modifiedObject.Password);

                _context.Entry(modifiedObject).State = EntityState.Modified;

                var result = await _context.SaveChangesAsync();

                //we creadit a user credit card if not created or update one
                if(modifiedObject.CreditCard != null)
                {
                    if (modifiedObject.CreditCard?.Id == null || modifiedObject.CreditCard.Id == 0)
                    {
                        _logger.LogInformation($"Creating a new credit card for user, Service Layer");
                        _context.CreditCards.Add(modifiedObject.CreditCard);
                        await _context.SaveChangesAsync();

                        var user = await _context.Users.Where(l => l.Id == modifiedObject.Id).FirstOrDefaultAsync();
                        var newlyCreatedCreditCard = await _context.CreditCards.Where(l => l.Id == modifiedObject.CreditCard.Id).FirstOrDefaultAsync();

                        user.CreditCard = newlyCreatedCreditCard;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _logger.LogInformation($"Updating existing user credit card, Service Layer");
                        _context.Entry(modifiedObject.CreditCard).State = EntityState.Modified;
                        result += await _context.SaveChangesAsync();
                    }
                }
                

                //we prepare response based on the result
                if (result > 0)
                {
                    response.Status = (int)ResponseType.ResponseSuccess;
                    response.Message = $"Successfully updated the {_logName}";
                }
                else
                {
                    response.Status = (int)ResponseType.FailedToUpdate;
                    response.Message = $"Failed to update the {_logName}";
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update the {_logName}, in Service Layer");
                throw;
            }
        }


        public async Task<UserActionResponse?> VerifyUser(VerificationRequest verificationObject)
        {
            _logger.LogInformation($"verifying a {_logName}, Service Layer");
            var response = new UserActionResponse();
            try
            {
                var user = await _context.Users.Where(l => l.Email == verificationObject.Email).FirstOrDefaultAsync();

                if (user == null)
                {
                    return null;
                }
                
                if(_securityAES.Decrypt(user.Password) == verificationObject.Password)
                {
                    //Here we generate session ID
                    var sessionId = await _sessionManager.GenerateSessionId(user.Id);

                    //we prepare response based on the result
                    if (sessionId != null)
                    {
                        response.Status = (int)ResponseType.ResponseSuccess;
                        response.Message = $"Successfully SignedIn the {_logName}";
                        response.User = user;
                        response.SessionID = sessionId;
                    }
                    else
                    {
                        response.Status = (int)ResponseType.FailedToSignIn;
                        response.Message = $"Failed to SignIn to {_logName}";
                        response.User = user;
                        response.SessionID = sessionId;
                    }

                    return response;
                }
                else
                {
                    response.Status = (int)ResponseType.FailedToSignIn;
                    response.Message = $"{_logName} password does not match";
                    response.User = user;

                    return response;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to verify the {_logName}, in Service Layer");
                throw ex;
            }
        }

        public Task<bool?> ClearCache(string key)
        {
            _logger.LogInformation($"Clearing all cached {_logName}s, Service Layer");

            try
            {
                _memoryCache.Remove(key);

                _logger.LogInformation($"Cleared all cached {_logName}s");

                var resp = Task.FromResult<bool?>(true);

                return resp;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to clear the cached {_logName}s, in Service Layer");
                throw ex;
            }
        }

        public async Task<ResponseType?> AddSubscription(SubscriptionActionRequest request)
        {
            _logger.LogInformation($"Assigning a subscription for a {_logName}, Service Layer");
            try
            {
                var user = await _context.Users.Where(u => u.Id == request.UserId).Include(l => l.Subscription).FirstOrDefaultAsync();
                var latestSubscription = await _context.Subscriptions.Where(u => u.Id == request.SubscriptionId).FirstOrDefaultAsync();

                if (user == null)
                {
                    _logger.LogInformation($"No {_logName} found");
                    return ResponseType.UserNotLoggedIn;
                }
                if (latestSubscription == null)
                {
                    _logger.LogInformation($"No Subscription found");
                    return ResponseType.NoObjectFound;
                }
                if (user.Subscription?.Id == latestSubscription.Id)
                {
                    _logger.LogInformation($"User is already borrowing this {_logName}");
                    return ResponseType.UserAlreadySubscribed;
                }

                user.Subscription = latestSubscription;

                //returns how many entries were Created (should be 1)
                var count = await _context.SaveChangesAsync();

                //clearing cache
                await ClearCache($"User_{request.UserId}");

                if (count > 0)
                {
                    return ResponseType.ResponseSuccess;
                }

                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to subscribe the {_logName} to the subscription with id: {request.SubscriptionId}, in Service Layer");
                throw ex;
            }
        }

        Task<User?> IDefaultRepository<User>.Create(User newObject)
        {
            throw new NotImplementedException();
        }

        Task<int?> IDefaultRepository<User>.Update(User modifiedObject)
        {
            throw new NotImplementedException();
        }

        Task<int?> IDefaultRepository<User>.Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserActionResponse?> LogOut(UserActionRequest request)
        {
            _logger.LogInformation($"Logging a {_logName} out, Service Layer");
            var response = new UserActionResponse();
            try
            {
                var entity = await _context.Sessions.Where(x => x.UserId == request.Id && x.SessionId == request.SessionID).FirstOrDefaultAsync();

                if (entity == null)
                {
                    _logger.LogInformation($"No Session found");
                    response.Status = (int)ResponseType.NoObjectFound;
                    response.Message = $"Couldn't find a Session";
                    return response;
                }
                entity.Valid = false;


                //returns how many entries were updated (should be 1 if it found the location that needs updating)
                var result = await _context.SaveChangesAsync();


                //neccessairy to clear the cache after a delete
                await ClearCache($"User_{request.Id}");

                //we prepare response based on the result
                if (result > 0)
                {
                    response.Status = (int)ResponseType.ResponseSuccess;
                    response.Message = $"Successfully logged out the {_logName}";
                }
                else
                {
                    response.Status = (int)ResponseType.FailedToLogOut;
                    response.Message = $"Failed to logout the {_logName}";
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete the {_logName}, in Service Layer");
                throw ex;
            }
        }
    }
}
