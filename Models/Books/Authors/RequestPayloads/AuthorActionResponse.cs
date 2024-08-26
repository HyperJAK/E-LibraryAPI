using ELib_IDSFintech_Internship.Repositories.Books.Authors;
using ELib_IDSFintech_Internship.Services.Enums;

namespace ELib_IDSFintech_Internship.Models.Books.Authors.RequestPayloads
{
    public class AuthorActionResponse : IAuthorActionResponseRepository
    {
        public int Status { get; set; }
        public string? Message { get; set; }
    }
}
