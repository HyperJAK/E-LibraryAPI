using ELib_IDSFintech_Internship.Models.Books;
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
        private readonly AES256Encryption _securityAES;

        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "Session";



        public SessionManagementService(Data.ELibContext context, ILogger<SessionManagementService> logger, IMemoryCache memoryCache, AES256Encryption securityAES)
        {
            _context = context;
            _logger = logger;
            _memoryCache = memoryCache;
            _securityAES = securityAES;
        }

        //add DB stuff to store value in DB
        public async Task<string?> GenerateSessionId(int userId)
        {
            byte[] buffer = new byte[16];
            _rng.GetBytes(buffer);

            var sessionId = BitConverter.ToString(buffer).Replace("-", string.Empty);
            var userObject = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();


            var encryptedSessionId = _securityAES.Encrypt(sessionId);

            var session = new Session()
            {
                User = userObject,
                SessionId = encryptedSessionId,
                Valid = true,
                TimeStamp = DateTime.UtcNow
            };

            _context.Sessions.Add(session);

            await _context.SaveChangesAsync();

            return encryptedSessionId;
        }

        public async Task<bool?> EqualSessionIds(SessionActionRequest request)
        {
            //here we need to get session ID from db based on given session id and user ID and compare and decrypt both first ofc

            var sessions = await _context.Sessions
                .Where(s => s.UserId == request.UserId && s.Valid == true)
                .ToListAsync();

            var decryptedRequestSessionId = _securityAES.Decrypt(request.SessionID);

            foreach (var session in sessions)
            {
                var decryptedSessionId = _securityAES.Decrypt(session.SessionId);
                if (string.Equals(decryptedRequestSessionId, decryptedSessionId, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            //no match found (error to make)
            return false;

        }

        public async Task<IEnumerable<Session>?> GetAll()
        {
            _logger.LogInformation($"Getting all {_logName}s information, Service Layer");

            try
            {
                _logger.LogInformation($"{_logName}s not found in cache");

                var sessions = await _context.Sessions.ToListAsync();


                return sessions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get all {_logName}s in Services Layer");
                throw ex;
            }
        }

        public async Task<Session?> GetById(Session newObject)
        {
            _logger.LogInformation($"Getting a single {_logName} with userId: {newObject.UserId} and sessionId: {newObject.SessionId}, Service Layer");
            try
            {
                return await _context.Sessions.Where(l => l.UserId == newObject.UserId && l.SessionId == newObject.SessionId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get the {_logName} with userId: {newObject.UserId} and sessionId: {newObject.SessionId}, in Services Layer");
                throw ex;
            }
        }

        public async Task<Session?> Create(Session newObject)
        {
            _logger.LogInformation($"Creating a {_logName}, Service Layer");
            try
            {
                //first we encrypt the hash password
                newObject.SessionId = _securityAES.Encrypt(newObject.SessionId);

                _context.Sessions.Add(newObject);

                //returns how many entries were Created (should be 1)
                await _context.SaveChangesAsync();

                var getUpdated = await _context.Sessions.Where(x => x.UserId == newObject.UserId && x.SessionId == newObject.SessionId).FirstOrDefaultAsync();

                return getUpdated;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to create the {_logName}, in Service Layer");
                throw ex;
            }
        }

        public async Task<int?> Update(Session modifiedObject)
        {
            _logger.LogInformation($"Updating a {_logName}, Service Layer");

            try
            {
                _context.Entry(modifiedObject).State = EntityState.Modified;

                //returns how many entries were updated (should be 1 if it found the location that needs updating)
                var affectedItems = await _context.SaveChangesAsync();


                return affectedItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update the {_logName}, in Service Layer");
                throw ex;
            }
        }

        public async Task<int?> Delete(Session modifiedObject)
        {
            _logger.LogInformation($"Deleting a {_logName}, Service Layer");

            try
            {
                //Deactivating and previous user sessions
                var userActiveSessionToReplace = await _context.Sessions.Where(u => u.UserId == modifiedObject.UserId && u.SessionId == modifiedObject.SessionId && u.Valid == true).FirstOrDefaultAsync();

                if (userActiveSessionToReplace == null)
                {
                    _logger.LogInformation($"No {_logName} found");
                    return null;
                }

                userActiveSessionToReplace.Valid = false;

                //returns how many entries were deleted (should be 1 if it found the location that needs deleting)
                var affectedItems = await _context.SaveChangesAsync();

                return affectedItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete the {_logName}, in Service Layer");
                throw ex;
            }
        }

        //Not used
        public Task<Session?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
