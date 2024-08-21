﻿using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Repositories.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ELib_IDSFintech_Internship.Services.Books
{
    public class BookService : IBookRepository
    {

        private readonly Data.ELibContext _context;
        private readonly ILogger<BookService> _logger;
        private readonly IMemoryCache _memoryCache;

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "Book";

        private readonly string cacheKey = "booksCaching";
        private readonly string cacheKeyWithGenres = "booksWithGenresCaching";
        private IEnumerable<Book>? cachedBooks;
        private IEnumerable<Book>? cachedBooksWithGenre;



        public BookService(Data.ELibContext context, ILogger<BookService> logger, IMemoryCache memoryCache)
        {
            _context = context;
            _logger = logger;
            _memoryCache = memoryCache;
        }


        public async Task<int?> Create(Book newObject)
        {
            _logger.LogInformation($"Creating a {_logName}, Service Layer");
            try
            {
                _context.Books.Add(newObject);

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
                var entity = await _context.Books.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (entity == null)
                {
                    _logger.LogInformation($"No {_logName} found");
                    return null;
                }
                _context.Books.Remove(entity);

                //returns how many entries were deleted (should be 1 if it found the location that needs deleting)
                var affectedItems = await _context.SaveChangesAsync();

                //neccessairy to clear the cache after a delete
                await ClearCache();

                return affectedItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete the {_logName}, in Service Layer");
                throw ex;
            }
        }

        public async Task<IEnumerable<Book>?> GetAll()
        {
            _logger.LogInformation($"Getting all {_logName}s information, Service Layer");
            try
            {

                if (_memoryCache.TryGetValue(cacheKey, out cachedBooks))
                {
                    _logger.LogInformation($"{_logName}s retrieved from cache");
                }
                else
                {
                    _logger.LogInformation($"{_logName}s not found in cache");

                    cachedBooks = await _context.Books.ToListAsync();

                    //Setting behavior of the cached items after a certain passed time
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);

                    _memoryCache.Set(cacheKey, cachedBooks, cacheEntryOptions);
                }

                return cachedBooks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get all {_logName}s in Services Layer");
                throw ex;
            }
        }

        public async Task<Book?> GetById(int id)
        {
            _logger.LogInformation($"Getting a single {_logName} using his ID: {id}, Service Layer");
            try
            {
                //if all books are cached we enter
                if (_memoryCache.TryGetValue(cacheKey, out cachedBooks))
                {
                    //we try to get the specific book from the cache
                    _logger.LogInformation($"{_logName}s retrieved from cache");
                    return cachedBooks?.Where(l => l.Id == id).FirstOrDefault();
                }
                else
                {
                    //if there is no cache then we call database
                    _logger.LogInformation($"{_logName}s not found in cache");

                    cachedBooks = await _context.Books.ToListAsync();

                    //Setting behavior of the cached items after a certain passed time
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);

                    _memoryCache.Set(cacheKey, cachedBooks, cacheEntryOptions);

                    return await _context.Books.Where(l => l.Id == id).FirstOrDefaultAsync();

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get the {_logName} with supposed ID: {id}, in Services Layer");
                throw ex;
            }
        }

        public async Task<int?> Update(Book modifiedObject)
        {
            _logger.LogInformation($"Updating a {_logName}, Service Layer");
            try
            {
                _context.Entry(modifiedObject).State = EntityState.Modified;

                //returns how many entries were updated (should be 1 if it found the location that needs updating)
                var affectedItems = await _context.SaveChangesAsync();

                //neccessairy to clear the cache after an update
                await ClearCache();

                return affectedItems;
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

        public async Task<IEnumerable<Book>?> GetSuggestionsByName(string name)
        {
            _logger.LogInformation($"Getting {_logName} suggestions with Name: {name}, Service Layer");
            try
            {
                //if all books are cached we enter
                if (_memoryCache.TryGetValue(cacheKey, out cachedBooks))
                {
                    //we try to get the specific book from the cache
                    _logger.LogInformation($"{_logName}s retrieved from cache");
                    var suggestions = cachedBooks?.Where(l => l.Title.StartsWith(name, StringComparison.OrdinalIgnoreCase)).Take(10).ToList();
                    return await Task.FromResult(suggestions);
                }
                else
                {
                    //if there is no cache then we call database
                    _logger.LogInformation($"{_logName}s not found in cache");

                    cachedBooks = await _context.Books.ToListAsync();

                    //Setting behavior of the cached items after a certain passed time
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);

                    _memoryCache.Set(cacheKey, cachedBooks, cacheEntryOptions);

                    var suggestions = cachedBooks?.Where(l => l.Title.StartsWith(name, StringComparison.OrdinalIgnoreCase)).Take(10).ToList();
                    return await Task.FromResult(suggestions);

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting {_logName} suggestions with Name: {name}, in Services Layer");
                throw ex;
            }
        }

        public async Task<IEnumerable<Book>?> GetSearchResultsByName(string name)
        {
            _logger.LogInformation($"Getting {_logName} search results with Name: {name}, Service Layer");
            try
            {
                //if all books are cached we enter
                if (_memoryCache.TryGetValue(cacheKey, out cachedBooks))
                {
                    //we try to get the specific book from the cache
                    _logger.LogInformation($"{_logName}s retrieved from cache");
                    var search = cachedBooks?.Where(l => l.Title.StartsWith(name, StringComparison.OrdinalIgnoreCase)).ToList();
                    return await Task.FromResult(search);
                }
                else
                {
                    //if there is no cache then we call database
                    _logger.LogInformation($"{_logName}s not found in cache");

                    cachedBooks = await _context.Books.ToListAsync();

                    //Setting behavior of the cached items after a certain passed time
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);

                    _memoryCache.Set(cacheKey, cachedBooks, cacheEntryOptions);

                    var search = cachedBooks?.Where(l => l.Title.StartsWith(name, StringComparison.OrdinalIgnoreCase)).ToList();
                    return await Task.FromResult(search);

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting {_logName} search results with Name: {name}, in Services Layer");
                throw ex;
            }
        }

        public async Task<IEnumerable<Book>?> GetBooksByGenre(int id)
        {
            _logger.LogInformation($"Getting {_logName}s by Genre: {id}, Service Layer");
            try
            {
                //if all books are cached we enter
                if (_memoryCache.TryGetValue(cacheKeyWithGenres, out cachedBooksWithGenre))
                {
                    //we try to get the specific book from the cache
                    _logger.LogInformation($"{_logName}s retrieved from cache");
                    var search = cachedBooksWithGenre?.Where(book => book.Genres.Any(genre => genre.Id == id)).ToList();
                    return await Task.FromResult(search);
                }
                else
                {
                    //if there is no cache then we call database
                    _logger.LogInformation($"{_logName}s not found in cache");

                    cachedBooksWithGenre = await _context.Books.Include(book => book.Genres).ToListAsync();

                    //Setting behavior of the cached items after a certain passed time
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);

                    _memoryCache.Set(cacheKeyWithGenres, cachedBooksWithGenre, cacheEntryOptions);

                    var search = cachedBooksWithGenre?.Where(book => book.Genres.Any(genre => genre.Id == id)).ToList();
                    return await Task.FromResult(search);

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting {_logName}s by Genre: {id}, in Services Layer");
                throw ex;
            }
        }
    }
}
