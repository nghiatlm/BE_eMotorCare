namespace eMotoCare.Application.Interfaces.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        List<T> GetAll();
        Task<List<T>> GetAllAsync();

        void Create(T entity);
        Task<int> CreateAsync(T entity);

        void Update(T entity);
        Task<int> UpdateAsync(T entity);

        void Delete(T entity);
        Task<bool> DeleteAsync(T entity);

        T GetById(int id);
        Task<T> GetByIdAsync(int id);

        T GetById(string code);
        Task<T> GetByIdAsync(string code);

        T GetById(Guid code);
        Task<T> GetByIdAsync(Guid code);

    }
}
