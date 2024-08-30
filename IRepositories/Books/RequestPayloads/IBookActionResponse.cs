using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Users;

namespace ELib_IDSFintech_Internship.Repositories.Books.RequestPayloads
{
    public interface IBookActionResponse : IDefaultResponsePayload
    {
        public Book? Book { get; set; }
        public User? User { get; set; }
    }
}
