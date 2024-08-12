using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Repositories.Books;

namespace ELib_IDSFintech_Internship.Services.Books
{
    public class BookGenreService : IBookGenreRepository
    {

        private readonly Data.ELibContext _context;
        private readonly ILogger<BookGenreService> _logger;

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "BookGenre";



        public BookGenreService(Data.ELibContext context, ILogger<BookGenreService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<int?> Create(BookGenre entity)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Delete(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BookGenre>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BookGenre?> GetById(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Update(BookGenre newEntity)
        {
            throw new NotImplementedException();
        }
    }
}
