using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Repositories.Books;

namespace ELib_IDSFintech_Internship.Services.Books
{
    public class BookLocationService : IBookLocationRepository
    {

        private readonly Data.ELibContext _context;
        private readonly ILogger<BookLocationService> _logger;

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "BookLocation";



        public BookLocationService(Data.ELibContext context, ILogger<BookLocationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<int?> Create(BookLocation newObject)
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

        public Task<int?> Update(BookLocation modifiedObject)
        {
            throw new NotImplementedException();
        }
    }
}
