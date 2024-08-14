using System.ComponentModel.DataAnnotations;

namespace ELib_IDSFintech_Internship.Models.Users
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        public string Type { get; set; }

        public double Price { get; set; }

        public DateOnly DurationInDays { get; set; }
    }
}
