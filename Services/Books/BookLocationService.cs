using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Repositories.Books;

namespace ELib_IDSFintech_Internship.Services.Books
{
    public class BookLocationService : IBookLocationRepository
    {
        public Task<int?> Create(BookLocation entity)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Delete(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BookLocation>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BookLocation?> GetById(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Update(BookLocation newEntity)
        {
            throw new NotImplementedException();
        }
    }
}
