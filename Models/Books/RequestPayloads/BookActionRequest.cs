﻿using ELib_IDSFintech_Internship.Models.Books.Authors;
using ELib_IDSFintech_Internship.Models.Tools;
using ELib_IDSFintech_Internship.Repositories;
using ELib_IDSFintech_Internship.Repositories.Books.RequestPayloads;

namespace ELib_IDSFintech_Internship.Models.Books.RequestPayloads
{
    [ValidateOne("EntityObject", "Id", "SessionID")]
    public class BookActionRequest : IBookActionRequest
    {
        public int? UserId { get; set; }
        public int? BookId { get; set; }

        private string _sessionID;
        private Book? _entityObject;
        private int? _id;

        public Book? EntityObject
        {
            get => _entityObject;
            set => _entityObject = value;
        }

        public int? Id
        {
            get => _id;
            set => _id = value;
        }

        public string? SessionID
        {
            get => _sessionID;
            set => _sessionID = value;
        }
    }
}
