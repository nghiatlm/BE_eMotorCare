using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.ServiceCenterServices;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/admin/service-centers")]
    //[Authorize(Roles = "ROLE_ADMIN")]
    public class ServiceCentersController : ControllerBase
    {
        private readonly IServiceCenterService _serviceCenterService;

        public ServiceCentersController(IServiceCenterService serviceCenterService)
        {
            _serviceCenterService = serviceCenterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] string? search,
            [FromQuery] StatusEnum? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _serviceCenterService.GetPagedAsync(search, status, page, pageSize);
            return Ok(
                ApiResponse<PageResult<ServiceCenterResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách ServiceCenter thành công"
                )
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _serviceCenterService.GetByIdAsync(id);
            return Ok(
                ApiResponse<ServiceCenterResponse>.SuccessResponse(
                    item,
                    "Lấy ServiceCenter thành công"
                )
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceCenterRequest request)
        {
            var id = await _serviceCenterService.CreateAsync(request);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo ServiceCenter thành công")
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ServiceCenterRequest request)
        {
            await _serviceCenterService.UpdateAsync(id, request);
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Cập nhật ServiceCenter thành công")
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _serviceCenterService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá ServiceCenter thành công"));
        }
    }
}
