using ELib_IDSFintech_Internship.Models.Books.Formats;

namespace ELib_IDSFintech_Internship.Repositories.Books.Formats
{
    public interface IBookFormatRepository : IDefaultRepository<BookFormat>
    {
        //This clears the cached data in memory
        public Task<bool?> ClearCache(string key);
    }
}
