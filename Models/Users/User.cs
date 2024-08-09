using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;

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

        public CreditCard? CreditCard { get; set; }

        public Subscription? Subscription { get; set; }

        public DateOnly? SubscriptionStartDate { get; set; }

        public DateOnly? SubscriptionEndDate { get; set; }

        public required DateTime TimeStamp { get; set; }
    }
}
