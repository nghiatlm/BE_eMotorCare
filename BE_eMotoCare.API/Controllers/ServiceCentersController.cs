using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.ServiceCenterServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/admin/service-centers")]
    //[Authorize(Roles = "ROLE_ADMIN")]
    public class ServiceCentersController : ControllerBase
    {
        private readonly IServiceCenterService _service;

        public ServiceCentersController(IServiceCenterService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] string? search,
            [FromQuery] StatusEnum? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _service.GetPagedAsync(search, status, page, pageSize);
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
            var item = await _service.GetByIdAsync(id);
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
            var id = await _service.CreateAsync(request);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo ServiceCenter thành công")
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ServiceCenterRequest request)
        {
            await _service.UpdateAsync(id, request);
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Cập nhật ServiceCenter thành công")
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá ServiceCenter thành công"));
        }
    }
}
