using ELib_IDSFintech_Internship.Models.Common;
using ELib_IDSFintech_Internship.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ELib_IDSFintech_Internship.Services.Common
{
    public class LanguageService : ILanguageRepository
    {
        private readonly Data.ELibContext _context;
        private readonly ILogger<LanguageService> _logger;
        private readonly IMemoryCache _memoryCache;

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "Language";

        private readonly string cacheKey = "languagesCaching";
        private IEnumerable<Language>? cachedLanguages;


        public LanguageService(Data.ELibContext context, ILogger<LanguageService> logger, IMemoryCache memoryCache)
        {
            _context = context;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<int?> Create(Language newObject)
        {
            _logger.LogInformation($"Creating a {_logName}, Service Layer");
            try
            {
                _context.Languages.Add(newObject);

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
                var entity = await _context.Languages.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (entity == null)
                {
                    _logger.LogInformation($"No {_logName} found");
                    return null;
                }
                _context.Languages.Remove(entity);

                //returns how many entries were deleted (should be 1 if it found the location that needs deleting)
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete the {_logName}, in Service Layer");
                throw ex;
            }
        }

        public async Task<IEnumerable<Language>?> GetAll()
        {
            _logger.LogInformation($"Getting all {_logName}s information, Service Layer");
            try
            {

                if (_memoryCache.TryGetValue(cacheKey, out cachedLanguages))
                {
                    _logger.LogInformation($"{_logName}s retrieved from cache");
                }
                else
                {
                    _logger.LogInformation($"{_logName}s not found in cache");

                    cachedLanguages = await _context.Languages.ToListAsync();

                    //Setting behavior of the cached items after a certain passed time
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);

                    _memoryCache.Set(cacheKey, cachedLanguages, cacheEntryOptions);
                }

                return cachedLanguages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get all {_logName}s in Services Layer");
                throw ex;
            }
        }

        public async Task<Language?> GetById(int id)
        {
            _logger.LogInformation($"Getting a single {_logName} using his ID: {id}, Service Layer");
            try
            {
                //if all books are cached we enter
                if (_memoryCache.TryGetValue(cacheKey, out cachedLanguages))
                {
                    //we try to get the specific book from the cache
                    _logger.LogInformation($"{_logName}s retrieved from cache");
                    return cachedLanguages?.Where(l => l.Id == id).FirstOrDefault();
                }
                else
                {
                    //if there is no cache then we call database
                    _logger.LogInformation($"{_logName}s not found in cache");

                    cachedLanguages = await _context.Languages.ToListAsync();
                    return await _context.Languages.Where(l => l.Id == id).FirstOrDefaultAsync();

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get the {_logName} with supposed ID: {id}, in Services Layer");
                throw ex;
            }
        }

        public async Task<int?> Update(Language modifiedObject)
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
