using ELib_IDSFintech_Internship.Models.Books;

namespace ELib_IDSFintech_Internship.Repositories.Books
{
    public interface IBookTagRepository : IDefaultRepository<BookTag>
    {
        //This clears the cached data in memory
        public Task<bool?> ClearCache();
    }
}
