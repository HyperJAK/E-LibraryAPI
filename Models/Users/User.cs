using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Users.CreditCards;
using ELib_IDSFintech_Internship.Models.Users.Sessions;
using ELib_IDSFintech_Internship.Models.Users.Subscriptions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ELib_IDSFintech_Internship.Models.Users
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public required string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [MaxLength(30)]
        public string? PhoneNumber { get; set; }

        [MaxLength(100)]
        public string? AboutMe { get; set; } = "";

        public int? CreditCardId { get; set; }

        [ForeignKey("CreditCardId")]
        public CreditCard? CreditCard { get; set; }

        //subscription
        public int? SubscriptionId { get; set; }

        [ForeignKey("SubscriptionId")]
        public Subscription? Subscription { get; set; }

        public ICollection<UserHasBooks> UserBooks { get; set; } = new List<UserHasBooks>();

        public DateOnly? SubscriptionStartDate { get; set; }

        public DateOnly? SubscriptionEndDate { get; set; }

        [JsonIgnore]
        public ICollection<Session>? Sessions { get; set; }

        [Required]
        public required DateTime TimeStamp { get; set; }

    }
}
