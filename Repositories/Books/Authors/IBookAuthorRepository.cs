using ELib_IDSFintech_Internship.Models.Books.Authors;
using ELib_IDSFintech_Internship.Models.Books.Authors.RequestPayloads;
using ELib_IDSFintech_Internship.Models.Users.RequestPayloads;
using ELib_IDSFintech_Internship.Services.Enums;

namespace ELib_IDSFintech_Internship.Repositories.Books.Authors
{
    public interface IBookAuthorRepository : IDefaultRepository<BookAuthor>
    {
        //This clears the cached data in memory
        public Task<bool?> ClearCache(string key);

        public new Task<AuthorActionResponse?> Update(BookAuthor modifiedObject);

    }
}
