using ELib_IDSFintech_Internship.Models.Tools;
using ELib_IDSFintech_Internship.Repositories.Users.RequestPayloads;

namespace ELib_IDSFintech_Internship.Models.Users.RequestPayloads
{
    [ValidateOne("EntityObject", "Id", "SessionID")]
    public class UserActionRequest : IUserActionRequest
    {

        private string? _sessionID;
        private User? _entityObject;
        private int? _id;

        public User? EntityObject
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
