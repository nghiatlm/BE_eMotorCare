using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.PaymentRepository
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<Payment?> GetByTransactionCodeAsync(string transactionCode);
        Task<IReadOnlyList<Payment>> GetByAppointmentIdAsync(Guid appointmentId);
    }
}
