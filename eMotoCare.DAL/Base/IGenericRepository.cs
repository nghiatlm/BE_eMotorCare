
namespace eMotoCare.DAL.Base
{
    public interface IGenericRepository<T> where T : class
    {
        List<T> FindAll();
        Task<List<T>> FindAllAsync();

        void Create(T entity);
        Task<int> CreateAsync(T entity);

        void Update(T entity);
        Task<int> UpdateAsync(T entity);

        void Delete(T entity);
        Task<bool> DeleteAsync(T entity);

        T FindById(Guid code);
        Task<T> FindByIdAsync(Guid code);
    }
}