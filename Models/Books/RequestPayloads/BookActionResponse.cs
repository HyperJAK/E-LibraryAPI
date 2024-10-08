﻿using ELib_IDSFintech_Internship.Models.Users;
using ELib_IDSFintech_Internship.Repositories.Books.RequestPayloads;

namespace ELib_IDSFintech_Internship.Models.Books.RequestPayloads
{
    public class BookActionResponse : IBookActionResponse
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public Book? Book { get; set; }
        public User? User { get; set; }
    }
}
