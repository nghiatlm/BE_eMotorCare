
using System.Net;
using eMotoCare.BO.DTO.Dashboard;
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

        public async Task<Overview?> GetOverviewAsync(Guid? serviceCenterId)
        {
            try
            {
                // var campaigns = await _unitOfWork.Campaigns.FindAllAsync();
                // var TotalCampaign = campaigns?.Count ?? 0;
                var totalEvCheck = await _unitOfWork.EVChecks.CountEVChecksInProgressAsync();
                var appointments = await _unitOfWork.Appointments.TotalAppoinmentAndRevenue(serviceCenterId);
                var rmas = await _unitOfWork.RMAs.TotalRMA(serviceCenterId);

                var overview = new Overview
                {
                    TotalCampaign = 0,
                    TotalRecall = 0,
                    totalEVCheckInProgress = totalEvCheck,
                    totalAppointment = appointments.totalAppointment,
                    TotalRevenue = appointments.totalRevenue,
                    TotalRMA = rmas ?? 0
                };

                return overview;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Account failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}