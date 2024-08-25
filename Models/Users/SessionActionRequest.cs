namespace ELib_IDSFintech_Internship.Models.Users
{
    public class SessionActionRequest
    {
        public required int UserId { get; set; }
        public required string SessionID { get; set; }
    }
}
