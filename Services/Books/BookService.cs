using ELib_IDSFintech_Internship.Models.Books;
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


        public BookService(Data.ELibContext context, ILogger<BookService> logger, IMemoryCache memoryCache)
        {
            _context = context;
            _logger = logger;
            _memoryCache = memoryCache;
        }


        public async Task<Book?> Create(Book newObject)
        {
            _logger.LogInformation($"Creating a {_logName}, Service Layer");
            try
            {
                _context.Books.Add(newObject);

                //returns how many entries were Created (should be 1)
                await _context.SaveChangesAsync();

                var getUpdated = await _context.Books.Where(x => x.Id == newObject.Id).FirstOrDefaultAsync();

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
                await ClearCache($"Book_{id}");

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

            var cacheKey = $"Book_all";

            try
            {

                if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<Book>? cachedBooks))
                {
                    _logger.LogInformation($"{_logName}s retrieved from cache");
                }
                else
                {
                    _logger.LogInformation($"{_logName}s not found in cache");

                    cachedBooks = await _context.Books.ToListAsync();

                    //Setting behavior of the cached items after a certain passed time
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
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
            _logger.LogInformation($"Getting a single {_logName} using its ID: {id}, Service Layer");

            var cacheKey = $"Book_{id}";

            try
            {
                if (_memoryCache.TryGetValue(cacheKey, out Book? cachedBook))
                {
                    _logger.LogInformation($"{_logName} retrieved from cache");
                    return cachedBook;
                }

                _logger.LogInformation($"{_logName} not found in cache");

                var book = await _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.PhysicalBookLocation)
                    .Include(b => b.Languages)
                    .Include(b => b.Genres)
                    .Include(b => b.Tags)
                    .Include(b => b.Formats)
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (book != null)
                {
                    //Setting behavior of the cached items after a certain passed time
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetPriority(CacheItemPriority.Normal);

                    _memoryCache.Set(cacheKey, book, cacheEntryOptions);
                }

                return book;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get the {_logName} with ID: {id}, in Services Layer");
                throw;
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
                await ClearCache($"Book_{modifiedObject.Id}");
                
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

                _logger.LogInformation($"Cleared all cached {_logName}s, specifically: {key}");

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
                var result = await _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.Genres)
                    .Where(l =>
                    l.Title.ToLower().StartsWith(name.ToLower()) ||
                    l.Genres.Any(g => g.Type.ToLower().StartsWith(name.ToLower())) ||
                    l.Author.FirstName.ToLower().StartsWith(name.ToLower())).Take(10).ToListAsync();

                return result;

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

            var cacheKeyNew = $"BooksSearch_{name}";

            try
            {
                //if all books are cached we enter
                if (_memoryCache.TryGetValue(cacheKeyNew, out IEnumerable<Book>? cachedBooks))
                {
                    _logger.LogInformation($"{_logName} retrieved from cache");
                    //if cache retrieved then we return it directly
                    return cachedBooks;
                }
                else
                {
                    //if there is no cache then we call database
                    _logger.LogInformation($"{_logName}s not found in cache");

                    //we search for books then add them to cache then send them to frontend
                    var result = await _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.Genres)
                    .Where(l =>
                    l.Title.ToLower().StartsWith(name.ToLower()) ||
                    l.Genres.Any(g => g.Type.ToLower().StartsWith(name.ToLower())) ||
                    l.Author.FirstName.ToLower().StartsWith(name.ToLower())).ToListAsync();

                    //Setting behavior of the cached items after a certain passed time
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetPriority(CacheItemPriority.Normal);

                    _memoryCache.Set(cacheKeyNew, result, cacheEntryOptions);

                    return await Task.FromResult(result);

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

            var cacheKeyNew = $"BooksGenre_{id}";

            try
            {
                //if all books are cached we enter
                if (_memoryCache.TryGetValue(cacheKeyNew, out IEnumerable<Book>? cachedBooksWithGenre))
                {
                    //we try to get the specific book from the cache
                    _logger.LogInformation($"{_logName}s retrieved from cache");
                    return cachedBooksWithGenre;
                }
                else
                {
                    //if there is no cache then we call database
                    _logger.LogInformation($"{_logName}s not found in cache");

                    var search = await _context.Books.Include(book => book.Genres).Where(book => book.Genres.Any(genre => genre.Id == id)).ToListAsync();

                    //Setting behavior of the cached items after a certain passed time
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetPriority(CacheItemPriority.Normal);

                    _memoryCache.Set(cacheKeyNew, search, cacheEntryOptions);

                    return await Task.FromResult(search);

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting {_logName}s by Genre: {id}, in Services Layer");
                throw ex;
            }
        }

        public async Task<IEnumerable<Book>?> GetBorrowedBooks(int userId)
        {
            _logger.LogInformation($"Getting {_logName}s borrowed by user with ID: {userId}, Service Layer");

            var cacheKeyNew = $"BorrowedBy_{userId}";

            try
            {
                //if all books are cached we enter
                if (_memoryCache.TryGetValue(cacheKeyNew, out IEnumerable<Book>? cachedBorrowedBooks))
                {
                    //we try to get the specific book from the cache
                    _logger.LogInformation($"{_logName}s retrieved from cache");
                    return cachedBorrowedBooks;
                }
                else
                {
                    //if there is no cache then we call database
                    _logger.LogInformation($"{_logName}s not found in cache");

                    var search = await _context.Books.Where(book => book.UserBooks.Any(uBook => uBook.UserId == userId)).ToListAsync();

                    //Setting behavior of the cached items after a certain passed time
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetPriority(CacheItemPriority.Normal);

                    _memoryCache.Set(cacheKeyNew, search, cacheEntryOptions);

                    return search;

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting {_logName}s borrowed by user with ID: {userId}, in Services Layer");
                throw ex;
            }
        }
    }
}
