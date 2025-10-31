using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.PriceServiceServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/price-services")]
    [Authorize(Roles = "ROLE_MANAGER")]
    public class PriceServicesController : ControllerBase
    {
        private readonly IPriceServiceService _priceServiceService;

        public PriceServicesController(IPriceServiceService priceServiceService)
        {
            _priceServiceService = priceServiceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] string? search,
            [FromQuery] Guid? partTypeId,
            [FromQuery] Remedies? remedies,
            [FromQuery] DateTime? fromEffectiveDate,
            [FromQuery] DateTime? toEffectiveDate,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _priceServiceService.GetPagedAsync(
                search,
                partTypeId,
                remedies,
                fromEffectiveDate,
                toEffectiveDate,
                minPrice,
                maxPrice,
                page,
                pageSize
            );
            return Ok(
                ApiResponse<PageResult<PriceServiceResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách bảng giá dịch vụ thành công"
                )
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _priceServiceService.GetByIdAsync(id);
            return Ok(
                ApiResponse<PriceServiceResponse>.SuccessResponse(item, "Lấy chi tiết thành công")
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PriceServiceRequest request)
        {
            var id = await _priceServiceService.CreateAsync(request);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo bảng giá dịch vụ thành công")
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PriceServiceRequest request)
        {
            await _priceServiceService.UpdateAsync(id, request);
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Cập nhật bảng giá dịch vụ thành công")
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _priceServiceService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá bảng giá dịch vụ thành công"));
        }
    }
}
