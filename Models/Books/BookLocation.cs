using System.ComponentModel.DataAnnotations;

namespace ELib_IDSFintech_Internship.Models.Books
{
    public class BookLocation
    {
        [Key]
        public int Id { get; set; }

        public int Floor { get; set; }
        public int Shelf { get; set; }
        public char Section { get; set; }

        //link to books
        public ICollection<Book> Books { get; set; }
    }
}
