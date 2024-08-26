using ELib_IDSFintech_Internship.Models.Books.Tags;

namespace ELib_IDSFintech_Internship.Repositories.Books.Tags
{
    public interface IBookTagRepository : IDefaultRepository<BookTag>
    {
        //This clears the cached data in memory
        public Task<bool?> ClearCache(string key);
    }
}
