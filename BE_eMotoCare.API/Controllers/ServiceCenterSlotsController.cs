using System.Net;
using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.DAL;
using eMototCare.BLL.Services.ServiceCenterSlotServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/admin/service-centerslots")]
    // [Authorize(Roles = "ROLE_ADMIN")]
    public class ServiceCenterSlotsController : ControllerBase
    {
        private readonly IServiceCenterSlotService _serviceCenterSlotService;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceCenterSlotsController(
            IServiceCenterSlotService serviceCenterSlotService,
            IUnitOfWork unitOfWork
        )
        {
            _serviceCenterSlotService = serviceCenterSlotService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Guid serviceCenterId)
        {
            var items = await _serviceCenterSlotService.GetAllAsync(serviceCenterId);
            var center = await _unitOfWork.ServiceCenters.GetByIdAsync(serviceCenterId);
            if (center is null)
            {
                return NotFound(ApiResponse<object>.NotFound("Không tìm thấy ServiceCenter"));
            }
            var payload = new
            {
                servicecenter = new
                {
                    serviceCenterId = center.Id,
                    serviceCente = new
                    {
                        name = center.Name,
                        address = center.Address,
                        phone = center.Phone,
                        email = center.Email,
                        Slot = items.Select(s => new
                        {
                            id = s.Id,
                            dayOfWeek = s.DayOfWeek.ToString(),
                            startTime = s.StartTime.ToString(@"hh\:mm"),
                            endTime = s.EndTime.ToString(@"hh\:mm"),
                            capacity = s.Capacity,
                            isActive = s.IsActive,
                        }),
                    },
                },
            };

            return Ok(
                ApiResponse<object>.SuccessResponse(
                    payload,
                    "Lấy danh sách slot theo trung tâm thành công"
                )
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            Guid serviceCenterId,
            [FromBody] ServiceCenterSlotRequest req
        )
        {
            var id = await _serviceCenterSlotService.CreateAsync(serviceCenterId, req);
            return CreatedAtAction(
                nameof(GetAll),
                new { serviceCenterId },
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo slot thành công")
            );
        }

        [HttpPut("{slotId:guid}")]
        public async Task<IActionResult> Update(
            Guid serviceCenterId,
            Guid slotId,
            [FromBody] ServiceCenterSlotRequest req
        )
        {
            await _serviceCenterSlotService.UpdateAsync(serviceCenterId, slotId, req);
            return NoContent();
        }

        [HttpDelete("{slotId:guid}")]
        public async Task<IActionResult> Delete(Guid serviceCenterId, Guid slotId)
        {
            await _serviceCenterSlotService.DeleteAsync(serviceCenterId, slotId);
            return NoContent();
        }
    }
}
