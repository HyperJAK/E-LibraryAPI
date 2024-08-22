using ELib_IDSFintech_Internship.Models.Users;
using ELib_IDSFintech_Internship.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace ELib_IDSFintech_Internship.Services.Users
{
    public class UserService : IUserRepository
    {

        private readonly Data.ELibContext _context;
        private readonly ILogger<UserService> _logger;

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "User";



        public UserService(Data.ELibContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
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
            try
            {
                return await _context.Users.ToListAsync();
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
            try
            {
                return await _context.Users.Where(l => l.Id == id).FirstOrDefaultAsync();
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
