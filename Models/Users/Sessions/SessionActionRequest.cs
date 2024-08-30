namespace ELib_IDSFintech_Internship.Models.Users.Sessions
{
    public class SessionActionRequest
    {
        private int _userId;
        private string _sessionId;

        public SessionActionRequest(int userId, string sessionid) {
            UserId = userId;
            SessionID = sessionid;
        }
        public int UserId { get => _userId; set => _userId = value; }
        public string SessionID { get => _sessionId; set => _sessionId = value; }
    }
}
