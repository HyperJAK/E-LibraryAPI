using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Users;
using ELib_IDSFintech_Internship.Repositories;
using ELib_IDSFintech_Internship.Repositories.Users;
using ELib_IDSFintech_Internship.Services.Books;
using ELib_IDSFintech_Internship.Services.Enums;
using ELib_IDSFintech_Internship.Services.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;

namespace ELib_IDSFintech_Internship.Services.Users
{
    public class UserService : IUserRepository
    {

        private readonly Data.ELibContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly AES256Encryption _securityAES;
        private readonly BookService _bookService;
        private readonly SessionManagementService _sessionManager;

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "User";



        public UserService(Data.ELibContext context, ILogger<UserService> logger, IMemoryCache memoryCache, AES256Encryption securityAES, BookService bookService, SessionManagementService sessionManager)
        {
            _context = context;
            _logger = logger;
            _memoryCache = memoryCache;
            _securityAES = securityAES;
            _bookService = bookService;
            _sessionManager = sessionManager;
        }

        //need to add more checks later for user and session ID
        public async Task<ResponseType?> BorrowBook(BorrowBookRequest request)
        {
            _logger.LogInformation($"Borrowing a {_logName}, Service Layer");
            try
            {
                var user = await _context.Users.Where(u => u.Id == request.UserId).Include(l => l.Subscription).Include(b => b.UserBooks).ThenInclude(b => b.Book).FirstOrDefaultAsync();
                var latestBook = await _context.Books.Where(u => u.Id == request.BookId).FirstOrDefaultAsync();

                if (user == null)
                {
                    _logger.LogInformation($"No {_logName} found");
                    return ResponseType.UserNotLoggedIn;
                }
                if (latestBook == null)
                {
                    _logger.LogInformation($"No Book found");
                    return ResponseType.NoObjectFound;
                }
                else
                {
                    _logger.LogInformation($"Testing to see if we have enough books");
                    if(latestBook.PhysicalBookAvailability == false && latestBook.Type == "Physical")
                    {
                        _logger.LogInformation($"Not enough books");
                        return ResponseType.OutOfBook;
                    }
                    else
                    {
                        _logger.LogInformation($"We have enough of this book");
                    }
                }
                if (user.UserBooks.Any(ub => ub.Book.Id == latestBook.Id)) 
                {
                    _logger.LogInformation($"User is already borrowing this {_logName}");
                    return ResponseType.UserAlreadyBorrow;
                }
                if (user.Subscription == null)
                {
                    _logger.LogInformation($"No {_logName} subscription found");
                    return ResponseType.SubscriptionNeeded;
                }

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

                    if(latestBook.PhysicalBookCount == 0)
                    {
                        latestBook.PhysicalBookAvailability = false;
                    }

                    _context.Entry(latestBook).State = EntityState.Modified;

                    //we also need to clear the cache of the book because we updated its availability / quantity
                    await _bookService.ClearCache($"Book_{request.BookId}");

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
                //clearing cache
                await ClearCache($"User_{user.Id}");


                //returns how many entries were Created (should be 1)
                var count = await _context.SaveChangesAsync();

                if(count > 0)
                {
                    return ResponseType.ResponseSuccess;
                }

                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to borrow the {_logName}, in Service Layer");
                throw ex;
            }
        }

        public async Task<(User?, string)> Create(User newObject)
        {
            _logger.LogInformation($"Creating a {_logName}, Service Layer");
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

                return (getUpdated, sessionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to create the {_logName}, in Service Layer");
                throw ex;
            }
        }

        public async Task<int?> Delete(int id)
        {
            _logger.LogInformation($"Deleting a {_logName}, Service Layer");
            try
            {
                var entity = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (entity == null)
                {
                    _logger.LogInformation($"No {_logName} found");
                    return null;
                }
                _context.Users.Remove(entity);

                await ClearCache($"User_{entity.Id}");

                //returns how many entries were deleted (should be 1 if it found the location that needs deleting)
                return await _context.SaveChangesAsync();
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
                if (_memoryCache.TryGetValue(cacheKey, out User? cachedUser))
                {
                    _logger.LogInformation($"{_logName} retrieved from cache");
                    return cachedUser;
                }
                else
                {
                    _logger.LogInformation($"{_logName}s not found in cache");

                     cachedUser = await _context.Users.Where(l => l.Id == id)
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


                    //Setting behavior of the cached items after a certain passed time
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetPriority(CacheItemPriority.Normal);

                    _memoryCache.Set(cacheKey, cachedUser, cacheEntryOptions);

                    return cachedUser;
                }
 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get the {_logName} with supposed ID: {id}, in Services Layer");
                throw ex;
            }
        }

        public async Task<int?> Update(User modifiedObject)
        {
            _logger.LogInformation($"Updating a {_logName}, Service Layer");
            try
            {
                //clearing cache
                await ClearCache($"User_{modifiedObject.Id}");

                //first we encrypt the hash password
                modifiedObject.Password = _securityAES.Encrypt(modifiedObject.Password);

                _context.Entry(modifiedObject).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                //we initiale / create a new database credit card and add it to this user
                if (modifiedObject.CreditCard.Id == null || modifiedObject.CreditCard.Id == 0)
                {
                    _logger.LogInformation($"Creating a new creditCard in table and linking it to current user, Service Layer");
                    _context.CreditCards.Add(modifiedObject.CreditCard);
                    await _context.SaveChangesAsync();

                    var user = await _context.Users.Where(l => l.Id == modifiedObject.Id).FirstOrDefaultAsync();
                    var newlyCreatedCreditCard = await _context.CreditCards.Where(l => l.Id == modifiedObject.CreditCard.Id).FirstOrDefaultAsync();

                    user.CreditCard = newlyCreatedCreditCard;
                    await _context.SaveChangesAsync();
                    return (int)ResponseType.ResponseSuccess;

                }
                else
                {
                    _logger.LogInformation($"Updating already existing user creditCard, Service Layer");
                    _context.Entry(modifiedObject.CreditCard).State = EntityState.Modified;
                    return await _context.SaveChangesAsync();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update the {_logName}, in Service Layer");
                throw ex;
            }
        }

        public async Task<(User?, string)> VerifyUser(VerificationRequest verificationObject)
        {
            _logger.LogInformation($"verifying a {_logName}, Service Layer");
            try
            {
                verificationObject.Password = _securityAES.Encrypt(verificationObject.Password);
                var user = await _context.Users.Where(l => (l.Email == verificationObject.Email && l.Password == verificationObject.Password)).FirstOrDefaultAsync();

                if (user == null) {
                    return (null, null);
                }
                //Here we generate session ID
                var sessionId = await _sessionManager.GenerateSessionId(user.Id);

                return (user, sessionId);
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

                return Task.FromResult<bool?>(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to clear the cached {_logName}s, in Service Layer");
                throw ex;
            }
        }

        public async Task<ResponseType?> AddSubscription(AddSubscriptionRequest request)
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
                await ClearCache($"User_{user.Id}");

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
    }
}
