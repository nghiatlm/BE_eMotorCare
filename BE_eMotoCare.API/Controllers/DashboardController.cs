

using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Dashboard;
using eMotoCare.BO.DTO.Responses;
using eMototCare.BLL.Services.DashboardServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/dashboards")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }

        [HttpGet("overview")]
        [Authorize(Roles = "ROLE_ADMIN, ROLE_MANAGER")]
        public async Task<IActionResult> GetAppointmentDashboard([FromQuery] Guid? serviceCenterId, [FromQuery] int? year)
        {
            var data = await _dashboardService.GetAppointmentDashboardAsync(serviceCenterId, year);
            return data != null? Ok(ApiResponse<AppointmentDashboardResponse>.SuccessResponse(data,"Lấy data thành công")): NotFound(ApiResponse<AppointmentDashboardResponse>.BadRequest("Appointment dashboard not found")
        );
        }
    }
}