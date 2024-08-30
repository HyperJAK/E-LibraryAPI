using ELib_IDSFintech_Internship.Models.Books;

namespace ELib_IDSFintech_Internship.Repositories.Books
{
    public interface IBookGenreRepository : IDefaultRepository<BookGenre>
    {
        //This clears the cached data in memory
        public Task<bool?> ClearCache(string key);
    }
}
