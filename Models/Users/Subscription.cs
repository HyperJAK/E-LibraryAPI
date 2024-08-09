using System.ComponentModel.DataAnnotations;
using ELib_IDSFintech_Internship.Models.Users.Enums;

namespace ELib_IDSFintech_Internship.Models.Users
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        public SubscriptionType Type { get; set; }

        public double Price { get; set; }

        public DateOnly DurationInDays { get; set; }
    }
}
