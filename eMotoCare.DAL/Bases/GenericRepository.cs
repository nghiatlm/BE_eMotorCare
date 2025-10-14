using eMotoCare.DAL.Context;
using Microsoft.EntityFrameworkCore;


namespace eMotoCare.DAL.Bases
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DBContextMotoCare _context;

        // Constructor yêu cầu DbContext phải được inject
        public GenericRepository(DBContextMotoCare context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Lấy tất cả bản ghi
        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        // Lấy tất cả bản ghi (Asynchronous)
        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        // Thêm một bản ghi
        public void Create(T entity)
        {
            _context.Add(entity);  // Không gọi SaveChanges ở đây nữa
        }

        // Thêm một bản ghi (Asynchronous)
        public async Task<int> CreateAsync(T entity)
        {
            await _context.AddAsync(entity);  // Không gọi SaveChanges ở đây nữa
            return 0; // Không trả về kết quả ở đây, chỉ để thực hiện thêm entity
        }

        // Cập nhật bản ghi
        public void Update(T entity)
        {
            _context.Update(entity); // Không gọi SaveChanges ở đây nữa
        }

        // Cập nhật bản ghi (Asynchronous)
        public async Task<int> UpdateAsync(T entity)
        {
            _context.Update(entity); // Không gọi SaveChanges ở đây nữa
            return 0;
        }

        // Xóa bản ghi
        public void Delete(T entity)
        {
            _context.Remove(entity); // Không gọi SaveChanges ở đây nữa
        }

        // Xóa bản ghi (Asynchronous)
        public async Task<bool> DeleteAsync(T entity)
        {
            _context.Remove(entity); // Không gọi SaveChanges ở đây nữa
            return true;
        }

        // Lấy bản ghi theo ID
        public T GetById(int id)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;  // Ngắt kết nối với DbContext
            }
            return entity;
        }

        // Lấy bản ghi theo ID (Asynchronous)
        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;  // Ngắt kết nối với DbContext
            }
            return entity;
        }

        // Lấy bản ghi theo mã (String ID)
        public T GetById(string code)
        {
            var entity = _context.Set<T>().Find(code);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;  // Ngắt kết nối với DbContext
            }
            return entity;
        }

        // Lấy bản ghi theo mã (Asynchronous)
        public async Task<T> GetByIdAsync(string code)
        {
            var entity = await _context.Set<T>().FindAsync(code);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;  // Ngắt kết nối với DbContext
            }
            return entity;
        }

        public T GetById(Guid code)
        {
            var entity = _context.Set<T>().Find(code);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;  // Ngắt kết nối với DbContext
            }
            return entity;
        }

        // Lấy bản ghi theo mã (Asynchronous)
        public async Task<T> GetByIdAsync(Guid code)
        {
            var entity = await _context.Set<T>().FindAsync(code);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;  // Ngắt kết nối với DbContext
            }
            return entity;
        }
    }
}
