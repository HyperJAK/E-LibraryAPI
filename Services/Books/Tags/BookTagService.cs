using ELib_IDSFintech_Internship.Models.Books.Tags;
using ELib_IDSFintech_Internship.Repositories.Books.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ELib_IDSFintech_Internship.Services.Books.Tags
{
    public class BookTagService : IBookTagRepository
    {
        private readonly Data.ELibContext _context;
        private readonly ILogger<BookTagService> _logger;
        private readonly IMemoryCache _memoryCache;

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "BookTag";


        public BookTagService(Data.ELibContext context, ILogger<BookTagService> logger, IMemoryCache memoryCache)
        {
            _context = context;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<BookTag?> Create(BookTag newObject)
        {
            _logger.LogInformation($"Creating a {_logName}, Service Layer");
            try
            {
                _context.Tags.Add(newObject);

                //returns how many entries were Created (should be 1)
                await _context.SaveChangesAsync();

                var getUpdated = await _context.Tags.Where(x => x.Id == newObject.Id).FirstOrDefaultAsync();

                return getUpdated;
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

            var cacheKey = $"Tag{id}";

            try
            {
                var entity = await _context.Tags.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (entity == null)
                {
                    _logger.LogInformation($"No {_logName} found");
                    return null;
                }
                _context.Tags.Remove(entity);

                //returns how many entries were deleted (should be 1 if it found the location that needs deleting)
                var affectedItems = await _context.SaveChangesAsync();

                //neccessairy to clear the cache after a delete
                await ClearCache(cacheKey);

                return affectedItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete the {_logName}, in Service Layer");
                throw ex;
            }
        }

        public async Task<IEnumerable<BookTag>?> GetAll()
        {
            _logger.LogInformation($"Getting all {_logName}s information, Service Layer");

            var cacheKey = "tagsCaching";

            try
            {

                if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<BookTag>? cachedTags))
                {
                    _logger.LogInformation($"{_logName}s retrieved from cache");
                    return cachedTags;
                }
                else
                {
                    _logger.LogInformation($"{_logName}s not found in cache");

                    cachedTags = await _context.Tags.ToListAsync();

                    //Setting behavior of the cached items after a certain passed time
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetPriority(CacheItemPriority.Normal);

                    _memoryCache.Set(cacheKey, cachedTags, cacheEntryOptions);

                    return cachedTags;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get all {_logName}s in Services Layer");
                throw ex;
            }
        }

        public async Task<BookTag?> GetById(int id)
        {
            _logger.LogInformation($"Getting a single {_logName} using his ID: {id}, Service Layer");

            var cacheKey = $"Tag{id}";

            try
            {
                //if all books are cached we enter
                if (_memoryCache.TryGetValue(cacheKey, out BookTag? cachedTag))
                {
                    //we try to get the specific book from the cache
                    _logger.LogInformation($"{_logName}s retrieved from cache");
                    return cachedTag;
                }
                else
                {
                    //if there is no cache then we call database
                    _logger.LogInformation($"{_logName}s not found in cache");
                    var result = await _context.Tags.Where(l => l.Id == id).FirstOrDefaultAsync();

                    //Setting behavior of the cached items after a certain passed time
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetPriority(CacheItemPriority.Normal);

                    _memoryCache.Set(cacheKey, result, cacheEntryOptions);

                    return result;

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get the {_logName} with supposed ID: {id}, in Services Layer");
                throw ex;
            }
        }

        public async Task<int?> Update(BookTag modifiedObject)
        {
            _logger.LogInformation($"Updating a {_logName}, Service Layer");

            var cacheKey = $"Tag{modifiedObject.Id}";

            try
            {
                _context.Entry(modifiedObject).State = EntityState.Modified;

                //returns how many entries were updated (should be 1 if it found the location that needs updating)
                var affectedItems = await _context.SaveChangesAsync();

                //neccessairy to clear the cache after an update
                await ClearCache(cacheKey);

                return affectedItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update the {_logName}, in Service Layer");
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
    }
}
