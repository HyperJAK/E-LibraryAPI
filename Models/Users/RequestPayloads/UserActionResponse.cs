using ELib_IDSFintech_Internship.Repositories.Users.RequestPayloads;


namespace ELib_IDSFintech_Internship.Models.Users.RequestPayloads
{
    public class UserActionResponse : IUserActionResponse
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public string? SessionID { get; set; }
        public User? User { get; set; }
    }
}
