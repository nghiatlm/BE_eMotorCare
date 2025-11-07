using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.ServiceCenterRepository
{
    public class ServiceCenterRepository
        : GenericRepository<ServiceCenter>,
            IServiceCenterRepository
    {
        public ServiceCenterRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<ServiceCenter?> GetByIdAsync(Guid id)
        {
            return await _context
                .ServiceCenters.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<(IReadOnlyList<ServiceCenter> Items, long Total)> GetPagedAsync(
            string? search,
            StatusEnum? status,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.ServiceCenters.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                q = q.Where(x =>
                    x.Code.Contains(search)
                    || x.Name.Contains(search)
                    || (x.Email != null && x.Email.Contains(search))
                    || (x.Phone != null && x.Phone.Contains(search))
                    || (x.Address != null && x.Address.Contains(search))
                );
            }

            if (status.HasValue)
                q = q.Where(x => x.Status == status.Value);

            var total = await q.LongCountAsync();

            var items = await q.OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<bool> ExistsCodeAsync(string code) =>
            _context.ServiceCenters.AnyAsync(x => x.Code == code);

        public Task<bool> ExistsEmailAsync(string email) =>
            _context.ServiceCenters.AnyAsync(x => x.Email == email);

        public Task<bool> ExistsPhoneAsync(string phone) =>
            _context.ServiceCenters.AnyAsync(x => x.Phone == phone);

        public Task<ServiceCenter?> GetByCodeAsync(string code) =>
            _context.ServiceCenters.AsNoTracking().FirstOrDefaultAsync(x => x.Code == code);

        public Task<bool> ExistsAsync(Guid id) =>
            _context.ServiceCenters.AsNoTracking().AnyAsync(x => x.Id == id);

        public async Task<ServiceCenterResponse?> GetDtoByIdAsync(Guid id)
        {
            return await _context
                .ServiceCenters.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new ServiceCenterResponse
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Description = x.Description,
                    Email = x.Email,
                    Phone = x.Phone,
                    Address = x.Address,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Status = x.Status,

                    ServiceCenterSlots = _context
                        .ServiceCenterSlots.AsNoTracking()
                        .Where(s => s.ServiceCenterId == x.Id)
                        .OrderBy(s => s.Date)
                        .Select(s => new ServiceCenterSlotResponse
                        {
                            Id = s.Id,
                            ServiceCenterId = s.ServiceCenterId,
                            Date = s.Date,
                            DayOfWeek = s.DayOfWeek,
                            SlotTime = s.SlotTime,
                            Capacity = s.Capacity,
                            IsActive = s.IsActive,
                            Note = s.Note,
                        })
                        .ToList(),
                })
                .FirstOrDefaultAsync();
        }

        public async Task<(
            IReadOnlyList<ServiceCenterResponse> Items,
            long Total
        )> GetDtoPagedAsync(string? search, StatusEnum? status, int page, int pageSize)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.ServiceCenters.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                q = q.Where(x =>
                    x.Code.Contains(search)
                    || x.Name.Contains(search)
                    || (x.Email != null && x.Email.Contains(search))
                    || (x.Phone != null && x.Phone.Contains(search))
                    || (x.Address != null && x.Address.Contains(search))
                );
            }

            if (status.HasValue)
                q = q.Where(x => x.Status == status.Value);

            var total = await q.LongCountAsync();

            var items = await q.OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ServiceCenterResponse
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Description = x.Description,
                    Email = x.Email,
                    Phone = x.Phone,
                    Address = x.Address,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Status = x.Status,

                    ServiceCenterSlots = _context
                        .ServiceCenterSlots.AsNoTracking()
                        .Where(s => s.ServiceCenterId == x.Id)
                        .OrderBy(s => s.Date)
                        .Select(s => new ServiceCenterSlotResponse
                        {
                            Id = s.Id,
                            ServiceCenterId = s.ServiceCenterId,
                            Date = s.Date,
                            DayOfWeek = s.DayOfWeek,
                            SlotTime = s.SlotTime,
                            Capacity = s.Capacity,
                            IsActive = s.IsActive,
                            Note = s.Note,
                        })
                        .ToList(),
                })
                .ToListAsync();

            return (items, total);
        }
    }
}
