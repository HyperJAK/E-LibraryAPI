using ELib_IDSFintech_Internship.Models.Users;
using ELib_IDSFintech_Internship.Repositories.Users;

namespace ELib_IDSFintech_Internship.Services.Users
{
    public class SubscriptionService : ISubscriptionRepository
    {

        private readonly Data.ELibContext _context;
        private readonly ILogger<SubscriptionService> _logger;

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "Subscription";



        public SubscriptionService(Data.ELibContext context, ILogger<SubscriptionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<int?> Create(Subscription entity)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Delete(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Subscription>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Subscription?> GetById(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Update(Subscription newEntity)
        {
            throw new NotImplementedException();
        }
    }
}
