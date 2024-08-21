﻿using ELib_IDSFintech_Internship.Models.Books;

namespace ELib_IDSFintech_Internship.Repositories.Books
{
    public interface IBookRepository : IDefaultRepository<Book>
    {
        //This clears the cached data in memory
        public Task<bool?> ClearCache();

        //This retrieves a specific amount of suggested books based on entered name
        public Task<IEnumerable<Book>?> GetSuggestionsByName(string name);

        //This retrieves books based on the given name
        public Task<IEnumerable<Book>?> GetSearchResultsByName(string name);

        //This retrieves books based on the given genre id
        public Task<IEnumerable<Book>?> GetBooksByGenre(int id);
    }
}
