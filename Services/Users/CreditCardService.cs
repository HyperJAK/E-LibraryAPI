using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Users;
using ELib_IDSFintech_Internship.Repositories.Users;

namespace ELib_IDSFintech_Internship.Services.Users
{
    public class CreditCardService : ICreditCardRepository
    {
        public Task<int?> Create(CreditCard entity)
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

        public Task<int?> Update(CreditCard newEntity)
        {
            throw new NotImplementedException();
        }
    }
}
