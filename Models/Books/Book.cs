﻿using System.ComponentModel.DataAnnotations;
using ELib_IDSFintech_Internship.Models.Books.Enums;

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

        public required BookType Type { get; set; }

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
        public BookFormatType? Format { get; set; }

        public required Languages Language { get; set; }

        public ICollection<BookGenre>? Genres { get; set; }

        public required BookAuthor Author { get; set; }

        public required DateTime TimeStamp { get; set; }
    }
}
