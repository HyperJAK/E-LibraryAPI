using ELib_IDSFintech_Internship.Models.Tools;
using ELib_IDSFintech_Internship.Repositories.Books.Authors;

namespace ELib_IDSFintech_Internship.Models.Books.Authors.RequestPayloads
{
    [ValidateOne("SessionID", "Id", "EntityObject")]
    public class AuthorActionRequest : IAuthorActionRequestRepository
    {

        private string _sessionID;
        private BookAuthor? _entityObject;
        private int? _id;

        public BookAuthor? EntityObject
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
