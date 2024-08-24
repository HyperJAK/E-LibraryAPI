using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Users;
using ELib_IDSFintech_Internship.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ELib_IDSFintech_Internship.Services.Users
{
    public class UserService : IUserRepository
    {

        private readonly Data.ELibContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly IMemoryCache _memoryCache;

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "User";



        public UserService(Data.ELibContext context, ILogger<UserService> logger, IMemoryCache memoryCache)
        {
            _context = context;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        //need to add more checks later for user and session ID
        public async Task<int?> BorrowBook(BorrowBookRequest request)
        {
            _logger.LogInformation($"Borrowing a {_logName}, Service Layer");
            try
            {
                var user = await _context.Users.Where(u => u.Id == request.UserId).Include(l => l.Subscription).FirstOrDefaultAsync();
                var latestBook = await _context.Books.Where(u => u.Id == request.BookId).FirstOrDefaultAsync();

                if (user == null)
                {
                    _logger.LogInformation($"No {_logName} found");
                    return -1;
                }
                if (latestBook == null)
                {
                    _logger.LogInformation($"No Book found");
                    return -2;
                }
                if (user.Subscription == null)
                {
                    _logger.LogInformation($"No {_logName} subscription found");
                    return -3;
                }

                if(latestBook.Type == "Physical")
                {
                    if (latestBook.PhysicalBookAvailability == true)
                    {
                        user.Books.Add(latestBook);

                        latestBook.PhysicalBookCount--;

                        if(latestBook.PhysicalBookCount == 0)
                        {
                            latestBook.PhysicalBookAvailability = false;
                        }

                        _context.Entry(latestBook).State = EntityState.Modified;

                    }
                }
                else
                {
                    user.Books.Add(latestBook);
                }

               

                //returns how many entries were Created (should be 1)
                return await _context.SaveChangesAsync();





            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to borrow the {_logName}, in Service Layer");
                throw ex;
            }
        }

        public async Task<int?> Create(User newObject)
        {
            _logger.LogInformation($"Creating a {_logName}, Service Layer");
            try
            {
                _context.Users.Add(newObject);

                //returns how many entries were Created (should be 1)
                return await _context.SaveChangesAsync();
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
                        .Include(u => u.Books)
                        .ThenInclude(b => b.Author)
                        .Include(u => u.Books)
                        .ThenInclude(b => b.PhysicalBookLocation)
                        .Include(u => u.Books)
                        .ThenInclude(b => b.Formats)
                        .Include(u => u.Books)
                        .ThenInclude(b => b.Languages)
                        .Include(u => u.Books)
                        .ThenInclude(b => b.Genres)
                        .Include(u => u.Books)
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
                        .Include(u => u.Books)
                        .ThenInclude(b => b.Author)
                        .Include(u => u.Books)
                        .ThenInclude(b => b.PhysicalBookLocation)
                        .Include(u => u.Books)
                        .ThenInclude(b => b.Formats)
                        .Include(u => u.Books)
                        .ThenInclude(b => b.Languages)
                        .Include(u => u.Books)
                        .ThenInclude(b => b.Genres)
                        .Include(u => u.Books)
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
                _context.Entry(modifiedObject).State = EntityState.Modified;

                //returns how many entries were updated (should be 1 if it found the location that needs updating)
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update the {_logName}, in Service Layer");
                throw ex;
            }
        }

        public async Task<User?> VerifyUser(VerificationRequest verificationObject)
        {
            _logger.LogInformation($"verifying a {_logName}, Service Layer");
            try
            {
                var user = await _context.Users.Where(l => (l.Email == verificationObject.Email && l.Password == verificationObject.Password)).FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to verify the {_logName}, in Service Layer");
                throw ex;
            }
        }
    }
}
