using ELib_IDSFintech_Internship.Models.Users;
using ELib_IDSFintech_Internship.Repositories.Users;

namespace ELib_IDSFintech_Internship.Services.Users
{
    public class UserService : IUserRepository
    {
        public Task<int?> Create(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Delete(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetById(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Update(User newEntity)
        {
            throw new NotImplementedException();
        }
    }
}
