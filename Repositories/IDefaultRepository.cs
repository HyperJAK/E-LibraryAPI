using ELib_IDSFintech_Internship.Models.Books;

namespace ELib_IDSFintech_Internship.Repositories
{
    public interface IDefaultRepository<T>
    {
        public Task<IEnumerable<T>?> GetAll();

        public Task<T?> GetById(int ID);

        public Task<int?> Create(T entity);

        public Task<int?> Update(T newEntity);

        public Task<int?> Delete(int ID);
    }
}
