

using eMotoCare.DAL.context;

namespace eMotoCare.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        => _context.Dispose();

        public async Task<int> SaveAsync()
        => await _context.SaveChangesAsync();
    }
}