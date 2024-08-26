using ELib_IDSFintech_Internship.Models.Common;
using ELib_IDSFintech_Internship.Models.Users.Sessions;
using ELib_IDSFintech_Internship.Services.Enums;

namespace ELib_IDSFintech_Internship.Repositories.Tools
{
    public interface ISessionManagementRepository : IDefaultRepository<Session>
    {
        public Task<string?> GenerateSessionId(int userId);

        public Task<bool?> EqualSessionIds(SessionActionRequest request);

        public Task<Session?> GetById(Session newObject);

        public Task<int?> Delete(Session modifiedObject);
    }
}
