using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Repositories.Books;

namespace ELib_IDSFintech_Internship.Services.Books
{
    public class BookService : IBookRepository
    {
        public Task<int?> Create(Book entity)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Delete(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Book>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Book?> GetById(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Update(Book newEntity)
        {
            throw new NotImplementedException();
        }
    }
}
