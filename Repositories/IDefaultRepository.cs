namespace ELib_IDSFintech_Internship.Repositories
{
    public interface IDefaultRepository<T>
    {
        public Task<IEnumerable<T>?> GetAll();

        public Task<T?> GetById(int id);

        public Task<int?> Create(T newObject);

        public Task<int?> Update(T modifiedObject);

        public Task<int?> Delete(int id);
    }
}
