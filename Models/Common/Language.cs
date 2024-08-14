using ELib_IDSFintech_Internship.Models.Books;
using System.ComponentModel.DataAnnotations;

namespace ELib_IDSFintech_Internship.Models.Common
{
    public class Language
    {
        [Key]
        public int Id { get; set; }

        public required string Type { get; set; }

        //link to books
        public ICollection<Book>? Books { get; set; } = new List<Book>();
    }
}
