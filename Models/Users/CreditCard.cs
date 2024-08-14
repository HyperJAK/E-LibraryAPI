using ELib_IDSFintech_Internship.Models.Books;
using System.ComponentModel.DataAnnotations;

namespace ELib_IDSFintech_Internship.Models.Users
{
    public class CreditCard
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public required string FullName { get; set; }

        [MaxLength(300)]
        public required string CardNumber { get; set; }

        [MaxLength(300)]
        public required string BillingAddress { get; set; }

        public required DateOnly ExpirationDate { get; set; }

        public required DateTime TimeStamp { get; set; }

        // Navigation property to Book
        public User User { get; set; }
    }
}
