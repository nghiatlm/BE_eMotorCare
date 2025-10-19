using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.EVCheckRepository
{
    public interface IEVCheckRepository : IGenericRepository<EVCheck>
    {
        Task<EVCheck?> GetByAppointmentIdAsync(Guid appointmentId);
        Task<EVCheck?> GetByIdIncludeDetailsAsync(Guid evCheckId);
    }
}
