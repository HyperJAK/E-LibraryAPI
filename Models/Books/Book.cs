using ELib_IDSFintech_Internship.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELib_IDSFintech_Internship.Models.Books
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        public required string Title { get; set; }

        public required DateOnly PublishingDate { get; set; }

        [MaxLength(15)]
        public required string ISBN { get; set; }

        [MaxLength(500)]
        public required string Description { get; set; }

        public required string Type { get; set; }

        public required bool PhysicalBookAvailability { get; set; } = true;

        public int PhysicalBookCount { get; set; } = 1;

        [MaxLength(500)]
        public string? DigitalBookURL { get; set; }

        //Shelf B2 row 2 col 4 for example
        public BookLocation? PhysicalBookLocation { get; set; }

        [MaxLength(100)]
        public required string Publisher { get; set; }

        [MaxLength(200)]
        public string? CoverImageURL { get; set; }

        public required int PageCount { get; set; }

        public int? FileSizeInMB { get; set; }

        //For digital books only
        public ICollection<BookFormat>? Formats { get; set; } = new List<BookFormat>();

        public required ICollection<Language> Languages { get; set; } = new List<Language>();

        public ICollection<BookGenre>? Genres { get; set; } = new List<BookGenre>();

        public ICollection<BookTag>? Tags { get; set; } = new List<BookTag>();

        //Author
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public required BookAuthor Author { get; set; }
        


        public required DateTime TimeStamp { get; set; }
    }
}
