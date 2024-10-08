﻿using ELib_IDSFintech_Internship.Models.Books.Authors;
using ELib_IDSFintech_Internship.Models.Books.Formats;
using ELib_IDSFintech_Internship.Models.Common;
using ELib_IDSFintech_Internship.Models.Users.RequestPayloads;
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

        //Location: Shelf B2 row 2 col 4 for example
        public int? LocationId { get; set; }
        [ForeignKey("LocationId")]
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

        public ICollection<UserHasBooks> UserBooks { get; set; } = new List<UserHasBooks>();

        //Author
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public required BookAuthor Author { get; set; }
        


        public required DateTime TimeStamp { get; set; }
    }
}
