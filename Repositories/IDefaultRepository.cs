using ELib_IDSFintech_Internship.Models.Books;

namespace ELib_IDSFintech_Internship.Repositories
{
    public interface IDefaultRepository
    {
        public Task<IEnumerable<BookAuthor>?> GetAll();

        public Task<BookAuthor?> GetById(int ID);

        public Task<int?> Create(BookAuthor entity);

        public Task<int?> Update(BookAuthor newEntity);

        public Task<int?> Delete(int ID);
    }
}
