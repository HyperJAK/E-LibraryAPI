using ELib_IDSFintech_Internship.Models.Books;
using System.ComponentModel.DataAnnotations;

namespace ELib_IDSFintech_Internship.Models.Users.Subscriptions
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        public string Type { get; set; }

        public double Price { get; set; }

        public DateOnly DurationInDays { get; set; }

        //link to users
        public ICollection<User> Users { get; set; }
    }
}
