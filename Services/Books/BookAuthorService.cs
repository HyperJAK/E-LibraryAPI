using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Repositories.Books;

namespace ELib_IDSFintech_Internship.Services.Books
{
    public class BookAuthorService : IBookAuthorRepository
    {

        private readonly Data.ELibContext _context;
        private readonly ILogger<BookAuthorService> _logger;

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "BookAuthor";



        public BookAuthorService(Data.ELibContext context, ILogger<BookAuthorService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<int?> Create(BookAuthor entity)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Delete(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BookAuthor>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BookAuthor?> GetById(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Update(BookAuthor newEntity)
        {
            throw new NotImplementedException();
        }
    }
}
