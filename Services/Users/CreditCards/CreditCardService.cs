using ELib_IDSFintech_Internship.Models.Users.CreditCards;
using ELib_IDSFintech_Internship.Repositories.Users.CreditCards;
using Microsoft.EntityFrameworkCore;

namespace ELib_IDSFintech_Internship.Services.Users.CreditCards
{
    public class CreditCardService : ICreditCardRepository
    {
        private readonly Data.ELibContext _context;
        private readonly ILogger<CreditCardService> _logger;

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "CreditCard";



        public CreditCardService(Data.ELibContext context, ILogger<CreditCardService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CreditCard?> Create(CreditCard newObject)
        {
            _logger.LogInformation($"Creating a {_logName}, Service Layer");
            try
            {
                _context.CreditCards.Add(newObject);

                //returns how many entries were Created (should be 1)
                await _context.SaveChangesAsync();

                var getUpdated = await _context.CreditCards.Where(x => x.Id == newObject.Id).FirstOrDefaultAsync();

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
                var entity = await _context.CreditCards.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (entity == null)
                {
                    _logger.LogInformation($"No {_logName} found");
                    return null;
                }
                _context.CreditCards.Remove(entity);

                //returns how many entries were deleted (should be 1 if it found the location that needs deleting)
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete the {_logName}, in Service Layer");
                throw ex;
            }
        }

        public async Task<IEnumerable<CreditCard>?> GetAll()
        {
            _logger.LogInformation($"Getting all {_logName}s information, Service Layer");
            try
            {
                return await _context.CreditCards.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get all {_logName}s in Services Layer");
                throw ex;
            }
        }

        public async Task<CreditCard?> GetById(int id)
        {
            _logger.LogInformation($"Getting a single {_logName} using his ID: {id}, Service Layer");
            try
            {
                return await _context.CreditCards.Where(l => l.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get the {_logName} with supposed ID: {id}, in Services Layer");
                throw ex;
            }
        }

        public async Task<int?> Update(CreditCard modifiedObject)
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
    }
}
