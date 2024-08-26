using ELib_IDSFintech_Internship.Models.Books.Authors;
using ELib_IDSFintech_Internship.Models.Books.Authors.RequestPayloads;
using ELib_IDSFintech_Internship.Models.Users.RequestPayloads;
using ELib_IDSFintech_Internship.Repositories;
using ELib_IDSFintech_Internship.Repositories.Books.Authors;
using ELib_IDSFintech_Internship.Services.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ELib_IDSFintech_Internship.Services.Books.Authors
{
    public class BookAuthorService : IBookAuthorRepository
    {

        private readonly Data.ELibContext _context;
        private readonly ILogger<BookAuthorService> _logger;
        private readonly IMemoryCache _memoryCache;

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "BookAuthor";



        public BookAuthorService(Data.ELibContext context, ILogger<BookAuthorService> logger, IMemoryCache memoryCache)
        {
            _context = context;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<BookAuthor?> Create(BookAuthor newObject)
        {
            _logger.LogInformation($"Creating a {_logName}, Service Layer");
            try
            {
                _context.Authors.Add(newObject);

                //returns how many entries were Created (should be 1)
                await _context.SaveChangesAsync();

                var getUpdated = await _context.Authors.Where(x => x.Id == newObject.Id).FirstOrDefaultAsync();

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
                var entity = await _context.Authors.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (entity == null)
                {
                    _logger.LogInformation($"No {_logName} found");
                    return null;
                }
                _context.Authors.Remove(entity);

                //returns how many entries were deleted (should be 1 if it found the location that needs deleting)
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete the {_logName}, in Service Layer");
                throw ex;
            }
        }

        public async Task<IEnumerable<BookAuthor>?> GetAll()
        {
            _logger.LogInformation($"Getting all {_logName}s information, Service Layer");
            try
            {
                return await _context.Authors.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get all {_logName}s in Services Layer");
                throw ex;
            }
        }

        public async Task<BookAuthor?> GetById(int id)
        {
            _logger.LogInformation($"Getting a single {_logName} using his ID: {id}, Service Layer");
            try
            {
                return await _context.Authors.Where(l => l.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get the {_logName} with supposed ID: {id}, in Services Layer");
                throw ex;
            }
        }

        public async Task<AuthorActionResponse?> Update(BookAuthor modifiedObject)
        {
            _logger.LogInformation($"Updating a {_logName}, Service Layer");
            var response = new AuthorActionResponse();
            try
            {
                //clearing cache
                await ClearCache($"Author_{modifiedObject.Id}");

                _context.Entry(modifiedObject).State = EntityState.Modified;

                //returns how many entries were updated (should be 1 if it found the location that needs updating)
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    response.Status = (int)ResponseType.ResponseSuccess;
                    response.Message = $"Successfully updated the {_logName}";

                    return response;
                }
                else
                {
                    response.Status = (int)ResponseType.FailedToUpdate;
                    response.Message = $"Failed to update the {_logName}";

                    return response;
                }

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

        Task<int?> IDefaultRepository<BookAuthor>.Update(BookAuthor modifiedObject)
        {
            throw new NotImplementedException();
        }
    }
}
