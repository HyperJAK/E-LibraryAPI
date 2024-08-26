using System.ComponentModel.DataAnnotations;

namespace ELib_IDSFintech_Internship.Models.Books.Authors
{
    public class BookAuthor
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(45)]
        public required string FirstName { get; set; }

        [MaxLength(60)]
        public required string LastName { get; set; }

        public DateOnly? BirthDate { get; set; }

        public required DateTime TimeStamp { get; set; }

        //link to books
        public ICollection<Book> Books { get; set; }
    }
}
