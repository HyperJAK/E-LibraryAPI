using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Users;
using ELib_IDSFintech_Internship.Repositories.Users;
using ELib_IDSFintech_Internship.Services.Books;

namespace ELib_IDSFintech_Internship.Services.Users
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

        public Task<int?> Create(CreditCard newObject)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Delete(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CreditCard>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<CreditCard?> GetById(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Update(CreditCard modifiedObject)
        {
            throw new NotImplementedException();
        }
    }
}
