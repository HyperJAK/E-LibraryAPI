using ELib_IDSFintech_Internship.Models.Books.Genres;

namespace ELib_IDSFintech_Internship.Repositories.Books.Genres
{
    public interface IBookGenreRepository : IDefaultRepository<BookGenre>
    {
        //This clears the cached data in memory
        public Task<bool?> ClearCache(string key);
    }
}
