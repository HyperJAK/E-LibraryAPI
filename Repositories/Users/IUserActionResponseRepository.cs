using ELib_IDSFintech_Internship.Models.Users;

namespace ELib_IDSFintech_Internship.Repositories.Users
{
    public interface IUserActionResponseRepository : IDefaultResponsePayloadRepository
    {
        public string? SessionID { get; set; }
        public User? User { get; set; }
    }
}
