using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Books.RequestPayloads;

namespace ELib_IDSFintech_Internship.Repositories.Books
{
    public interface IBookRepository : IDefaultRepository<Book>
    {
        //This clears the cached data in memory
        public Task<bool?> ClearCache(string key);

        //This retrieves a specific amount of suggested books based on entered name
        public Task<IEnumerable<Book>?> GetSuggestionsByName(string name);

        //This retrieves books based on the given name
        public Task<IEnumerable<Book>?> GetSearchResultsByName(string name);

        //This retrieves books based on the given genre id
        public Task<IEnumerable<Book>?> GetBooksByGenre(int id);

        //This retrieves books that were borrowed by a specific user
        public Task<IEnumerable<Book>?> GetBorrowedBooks(int userId);

        public new Task<BookActionResponse?> Create(Book newObject);

        public new Task<BookActionResponse?> Delete(int id);



    }
}
