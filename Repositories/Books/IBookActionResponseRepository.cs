using ELib_IDSFintech_Internship.Models.Books;

namespace ELib_IDSFintech_Internship.Repositories.Books
{
    public interface IBookActionResponseRepository : IDefaultResponsePayloadRepository
    {
        public Book? Book { get; set; }
    }
}
