namespace ELib_IDSFintech_Internship.Models.Users.Sessions
{
    public class SessionActionRequest
    {
        public required int UserId { get; set; }
        public required string SessionID { get; set; }
    }
}
