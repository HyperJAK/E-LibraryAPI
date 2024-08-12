using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Repositories.Books;

namespace ELib_IDSFintech_Internship.Services.Books
{
    public class BookService : IBookRepository
    {

        private readonly Data.ELibContext _context;
        private readonly ILogger<BookService> _logger;

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "Book";



        public BookService(Data.ELibContext context, ILogger<BookService> logger)
        {
            _context = context;
            _logger = logger;
        }

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
