using ELib_IDSFintech_Internship.Models.Users;
using ELib_IDSFintech_Internship.Repositories.Users;

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
