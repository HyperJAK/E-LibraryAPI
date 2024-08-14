using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Repositories.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ELib_IDSFintech_Internship.Services.Books
{
    public class BookFormatService : IBookFormatRepository
    {

        private readonly Data.ELibContext _context;
        private readonly ILogger<BookFormatService> _logger;
        private readonly IMemoryCache _memoryCache;

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "BookFormat";

        private readonly string cacheKey = "bookFormatsCaching";
        private IEnumerable<BookFormat>? cachedBookFormats;


        public BookFormatService(Data.ELibContext context, ILogger<BookFormatService> logger, IMemoryCache memoryCache)
        {
            _context = context;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<int?> Create(BookFormat newObject)
        {
            _logger.LogInformation($"Creating a {_logName}, Service Layer");
            try
            {
                _context.BookFormats.Add(newObject);

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
                var entity = await _context.BookFormats.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (entity == null)
                {
                    _logger.LogInformation($"No {_logName} found");
                    return null;
                }
                _context.BookFormats.Remove(entity);

                //returns how many entries were deleted (should be 1 if it found the location that needs deleting)
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete the {_logName}, in Service Layer");
                throw ex;
            }
        }

        public async Task<IEnumerable<BookFormat>?> GetAll()
        {
            _logger.LogInformation($"Getting all {_logName}s information, Service Layer");
            try
            {

                if (_memoryCache.TryGetValue(cacheKey, out cachedBookFormats))
                {
                    _logger.LogInformation($"{_logName}s retrieved from cache");
                }
                else
                {
                    _logger.LogInformation($"{_logName}s not found in cache");

                    cachedBookFormats = await _context.BookFormats.ToListAsync();

                    //Setting behavior of the cached items after a certain passed time
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);

                    _memoryCache.Set(cacheKey, cachedBookFormats, cacheEntryOptions);
                }

                return cachedBookFormats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get all {_logName}s in Services Layer");
                throw ex;
            }
        }

        public async Task<BookFormat?> GetById(int id)
        {
            _logger.LogInformation($"Getting a single {_logName} using his ID: {id}, Service Layer");
            try
            {
                //if all books are cached we enter
                if (_memoryCache.TryGetValue(cacheKey, out cachedBookFormats))
                {
                    //we try to get the specific book from the cache
                    _logger.LogInformation($"{_logName}s retrieved from cache");
                    return cachedBookFormats?.Where(l => l.Id == id).FirstOrDefault();
                }
                else
                {
                    //if there is no cache then we call database
                    _logger.LogInformation($"{_logName}s not found in cache");

                    cachedBookFormats = await _context.BookFormats.ToListAsync();
                    return await _context.BookFormats.Where(l => l.Id == id).FirstOrDefaultAsync();

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get the {_logName} with supposed ID: {id}, in Services Layer");
                throw ex;
            }
        }

        public async Task<int?> Update(BookFormat modifiedObject)
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

        public Task<bool?> ClearCache()
        {
            _logger.LogInformation($"Clearing all cached {_logName}s, Service Layer");
            try
            {
                _memoryCache.Remove(cacheKey);

                _logger.LogInformation($"Cleared all cached {_logName}s");

                return Task.FromResult<bool?>(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to clear the cached {_logName}s, in Service Layer");
                throw ex;
            }
        }
    }
}
