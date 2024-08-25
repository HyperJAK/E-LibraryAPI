namespace ELib_IDSFintech_Internship.Models.Users
{
    public class LogOutRequest
    {
        public required int UserId { get; set; }
        public required string SessionID { get; set; }
    }
}
