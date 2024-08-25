using System.Security.Cryptography;

namespace ELib_IDSFintech_Internship.Services.Enums
{
    public class SessionManagement
    {
        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        public string GenerateSessionId()
        {
            byte[] buffer = new byte[16];
            _rng.GetBytes(buffer);
            return BitConverter.ToString(buffer).Replace("-", string.Empty);
        }

        public bool EqualSessionIds(string sessionId1, string sessionId2)
        {
            return string.Equals(sessionId1, sessionId2, StringComparison.OrdinalIgnoreCase);
        }


    }
}
