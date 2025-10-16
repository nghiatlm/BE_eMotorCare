

using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Base
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Create(T entity)
        {
            _context.Add(entity);
        }

        public async Task<int> CreateAsync(T entity)
        {
            await _context.AddAsync(entity);
            return 1;
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _context.Remove(entity);
            return true;
        }

        public List<T> FindAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<List<T>> FindAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public T FindById(Guid code)
        {
            var entity = _context.Set<T>().Find(code);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public async Task<T> FindByIdAsync(Guid code)
        {
            var entity = await _context.Set<T>().FindAsync(code);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }

        public async Task<int> UpdateAsync(T entity)
        {
            _context.Update(entity);
            return 1;
        }
    }
}