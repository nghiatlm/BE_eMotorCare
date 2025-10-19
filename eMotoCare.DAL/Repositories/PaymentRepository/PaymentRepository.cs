using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.PaymentRepository
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext context)
            : base(context) { }

        public Task<Payment?> GetByTransactionCodeAsync(string transactionCode) =>
            _context
                .Payments.AsNoTracking()
                .FirstOrDefaultAsync(x => x.TransactionCode == transactionCode);

        public async Task<IReadOnlyList<Payment>> GetByAppointmentIdAsync(Guid appointmentId) =>
            await _context
                .Payments.AsNoTracking()
                .Where(x => x.AppointmentId == appointmentId)
                .ToListAsync();
    }
}
