
using System.Net;
using eMotoCare.BO.DTO.Dashboard;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Exceptions;
using eMotoCare.DAL;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.DashboardServices
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(IUnitOfWork unitOfWork, ILogger<DashboardService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<AppointmentDashboardResponse> GetAppointmentDashboardAsync(
        Guid? serviceCenterId,
         int? year)
        {
            try
            {
                var y = year ?? DateTime.UtcNow.AddHours(7).Year;

                var items = await _unitOfWork.Appointments
                    .GetAppointmentDashboardByMonthAsync(serviceCenterId, y);

                return new AppointmentDashboardResponse
                {
                    Year = y,
                    Data = items
                };
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAppointmentDashboardAsync failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}