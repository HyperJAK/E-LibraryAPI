using ELib_IDSFintech_Internship.Models.Books;

namespace ELib_IDSFintech_Internship.Models.Users
{
    public class UserHasBooks
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public DateTime BorrowedDate { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
