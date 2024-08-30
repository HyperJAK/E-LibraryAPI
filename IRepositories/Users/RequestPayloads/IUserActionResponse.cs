using ELib_IDSFintech_Internship.Models.Users;

namespace ELib_IDSFintech_Internship.Repositories.Users.RequestPayloads
{
    public interface IUserActionResponse : IDefaultResponsePayload
    {
        public string? SessionID { get; set; }
        public User? User { get; set; }
    }
}
