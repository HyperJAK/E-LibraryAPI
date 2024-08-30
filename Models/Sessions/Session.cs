using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Users;

namespace ELib_IDSFintech_Internship.Models.Sessions
{
    public class Session
    {
        public int? UserId { get; set; }
        public User? User { get; set; }

        public string? SessionId { get; set; }

        public bool? Valid { get; set; }

        public DateTime? TimeStamp { get; set; }
    }
}
