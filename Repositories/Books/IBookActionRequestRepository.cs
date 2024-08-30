using ELib_IDSFintech_Internship.Models.Books;

namespace ELib_IDSFintech_Internship.Repositories.Books
{
    public interface IBookActionRequestRepository : IDefaultRequestPayloadRepository<Book>
    {
        public int? UserId { get; set; }
        public int? BookId { get; set; }
    }
}
