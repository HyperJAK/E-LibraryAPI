using ELib_IDSFintech_Internship.Models.Users;
using ELib_IDSFintech_Internship.Repositories.Books;

namespace ELib_IDSFintech_Internship.Models.Books.RequestPayloads
{
    public class BookActionResponse : IBookActionResponseRepository
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public Book? Book { get; set; }
    }
}
