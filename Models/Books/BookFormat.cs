using System.ComponentModel.DataAnnotations;

namespace ELib_IDSFintech_Internship.Models.Books.Formats
{
    public class BookFormat
    {
        [Key]
        public int Id { get; set; }

        public required string Type { get; set; }

        //link to books
        public ICollection<Book>? Books { get; set; } = new List<Book>();
    }
}
