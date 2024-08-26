using ELib_IDSFintech_Internship.Models.Tools;
using ELib_IDSFintech_Internship.Models.Users;
using ELib_IDSFintech_Internship.Repositories.Tools;
using ELib_IDSFintech_Internship.Services.Books;
using ELib_IDSFintech_Internship.Services.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;

namespace ELib_IDSFintech_Internship.Services.Tools
{
    public class SessionManagementService : ISessionManagementRepository
    {
        private readonly Data.ELibContext _context;
        private readonly ILogger<SessionManagementService> _logger;
        private readonly IMemoryCache _memoryCache;

        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "Session";



        public SessionManagementService(Data.ELibContext context, ILogger<SessionManagementService> logger, IMemoryCache memoryCache)
        {
            _context = context;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        //add DB stuff to store value in DB
        public async Task<string?> GenerateSessionId(int userId)
        {
            byte[] buffer = new byte[16];
            _rng.GetBytes(buffer);
            return BitConverter.ToString(buffer).Replace("-", string.Empty);
        }

        public async Task<bool?> EqualSessionIds(SessionActionRequest request)
        {
            //here we need to get session ID from db based on given session id and user iD and compare

            return string.Equals(request.SessionID,"Here from database", StringComparison.OrdinalIgnoreCase);
        }

        public Task<IEnumerable<Session>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Session?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Session?> Create(Session newObject)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Update(Session modifiedObject)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
