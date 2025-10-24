using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
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

        public ServiceCenterSlotsController(IServiceCenterSlotService serviceCenterSlotService)
        {
            _serviceCenterSlotService = serviceCenterSlotService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(Guid serviceCenterId)
        {
            var items = await _serviceCenterSlotService.GetAllAsync(serviceCenterId);
            return Ok(
                ApiResponse<List<ServiceCenterSlotResponse>>.SuccessResponse(
                    items,
                    "Lấy danh sách slot thành công"
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
