using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Users;

namespace ELib_IDSFintech_Internship.Repositories.Books
{
    public interface IBookActionResponseRepository : IDefaultResponsePayloadRepository
    {
        public Book? Book { get; set; }
        public User? User { get; set; }
    }
}
