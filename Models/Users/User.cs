using ELib_IDSFintech_Internship.Models.Books;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELib_IDSFintech_Internship.Models.Users
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        public required string Username { get; set; }

        [MaxLength(100)]
        public required string Email { get; set; }

        [MaxLength(300)]
        public required string Password { get; set; }

        [MaxLength(30)]
        public string? PhoneNumber { get; set; }

        public int? CreditCardId { get; set; }

        [ForeignKey("CreditCardId")]
        public CreditCard? CreditCard { get; set; }

        //subscription
        public int? SubscriptionId { get; set; }

        [ForeignKey("SubscriptionId")]
        public Subscription? Subscription { get; set; }

        public ICollection<Book>? Books { get; set; } = new List<Book>();

        public DateOnly? SubscriptionStartDate { get; set; }

        public DateOnly? SubscriptionEndDate { get; set; }

        public required DateTime TimeStamp { get; set; }
    }
}
