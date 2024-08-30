using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Books.Authors;
using ELib_IDSFintech_Internship.Models.Tools;
using ELib_IDSFintech_Internship.Repositories.Books.RequestPayloads;
using ELib_IDSFintech_Internship.Repositories.Users;

namespace ELib_IDSFintech_Internship.Models.Users.RequestPayloads
{
    [ValidateOne("SessionID", "EntityObject", "Id")]
    public class BorrowBookRequest : IBookActionRequest
    {
        public int? UserId { get; set; }
        public int? BookId { get; set; }

        private string? _sessionID;
        private Book? _entityObject;
        private int? _id;

        public Book? EntityObject
        {
            get => _entityObject;
            set => _entityObject = value;
        }

        public int? Id
        {
            get => _id;
            set => _id = value;
        }

        public string? SessionID
        {
            get => _sessionID;
            set => _sessionID = value;
        }
    }
}
