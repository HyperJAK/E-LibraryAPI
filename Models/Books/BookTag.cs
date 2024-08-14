using ELib_IDSFintech_Internship.Models.Books.Enums;
using System.ComponentModel.DataAnnotations;

namespace ELib_IDSFintech_Internship.Models.Books
{
    public class BookTag
    {
        [Key]
        public int Id { get; set; }

        public required string Type { get; set; }
    }
}
