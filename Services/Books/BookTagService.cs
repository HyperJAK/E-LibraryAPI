using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Repositories.Books;

namespace ELib_IDSFintech_Internship.Services.Books
{
    public class BookTagService : IBookTagRepository
    {
        private readonly Data.ELibContext _context;
        private readonly ILogger<BookTagService> _logger;

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "BookTag";



        public BookTagService(Data.ELibContext context, ILogger<BookTagService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<int?> Create(BookTag newObject)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Delete(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BookTag>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BookTag?> GetById(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Update(BookTag modifiedObject)
        {
            throw new NotImplementedException();
        }
    }
}
