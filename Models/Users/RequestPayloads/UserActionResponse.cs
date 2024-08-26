using ELib_IDSFintech_Internship.Repositories.Books.Authors;
using ELib_IDSFintech_Internship.Repositories.Users;
using ELib_IDSFintech_Internship.Services.Enums;

namespace ELib_IDSFintech_Internship.Models.Users.RequestPayloads
{
    public class UserActionResponse : IUserActionResponseRepository
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public string? SessionID { get; set; }
        public User? User { get; set; }
    }
}
