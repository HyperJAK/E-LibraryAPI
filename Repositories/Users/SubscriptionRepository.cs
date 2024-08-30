using ELib_IDSFintech_Internship.Models.Users;
using ELib_IDSFintech_Internship.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace ELib_IDSFintech_Internship.Services.Users
{
    public class SubscriptionRepository : ISubscriptionRepository
    {

        private readonly Data.ELibContext _context;
        private readonly ILogger<SubscriptionRepository> _logger;

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "Subscription";



        public SubscriptionRepository(Data.ELibContext context, ILogger<SubscriptionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Subscription>?> GetAll()
        {
            _logger.LogInformation($"Getting all {_logName}s information, Service Layer");
            try
            {
                return await _context.Subscriptions.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get all {_logName}s in Services Layer");
                throw ex;
            }
        }

        public Task<Subscription?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Update(Subscription modifiedObject)
        {
            throw new NotImplementedException();
        }
        public Task<Subscription?> Create(Subscription newObject)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
