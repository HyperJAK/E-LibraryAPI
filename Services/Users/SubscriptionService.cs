using ELib_IDSFintech_Internship.Models.Users;
using ELib_IDSFintech_Internship.Repositories.Users;

namespace ELib_IDSFintech_Internship.Services.Users
{
    public class SubscriptionService : ISubscriptionRepository
    {
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
