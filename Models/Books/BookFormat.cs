﻿using System.ComponentModel.DataAnnotations;

namespace ELib_IDSFintech_Internship.Models.Books
{
    public class BookFormat
    {
        [Key]
        public int Id { get; set; }

        public required string Type { get; set; }
    }
}