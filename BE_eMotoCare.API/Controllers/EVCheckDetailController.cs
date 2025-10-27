using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.EVCheckDetailServices;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/ev_check_details")]
    [ApiController]
    public class EVCheckDetailController : ControllerBase
    {
        private readonly IEVCheckDetailService _eVCheckDetailService;

        public EVCheckDetailController(IEVCheckDetailService eVCheckDetailService)
        {
            _eVCheckDetailService = eVCheckDetailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetByParams(
            [FromQuery] Guid? maintenanceStageDetailId,
            [FromQuery] Guid? campaignDetailId,
            [FromQuery] Guid? partItemId,
            [FromQuery] Guid? eVCheckId,
            [FromQuery] Guid? replacePartId,
            [FromQuery] string? result,
            [FromQuery] Remedies? remedies,
            [FromQuery] string? unit,
            [FromQuery] decimal? quantity,
            [FromQuery] decimal? pricePart,
            [FromQuery] decimal? priceService,
            [FromQuery] decimal? totalAmount,
            [FromQuery] EVCheckDetailStatus? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _eVCheckDetailService.GetPagedAsync(maintenanceStageDetailId, 
                                                                campaignDetailId, 
                                                                partItemId, 
                                                                eVCheckId, 
                                                                replacePartId, 
                                                                result, 
                                                                remedies, 
                                                                unit, 
                                                                quantity, 
                                                                pricePart, 
                                                                priceService,
                                                                totalAmount, 
                                                                status, 
                                                                page, 
                                                                pageSize);
            return Ok(
                ApiResponse<PageResult<EVCheckDetailResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách EVCheckDetail thành công"
                )
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _eVCheckDetailService.GetByIdAsync(id);
            return Ok(
                ApiResponse<EVCheckDetailResponse>.SuccessResponse(
                    item,
                    "Lấy Part EVCheckDetail thành công"
                )
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EVCheckDetailRequest request)
        {
            var id = await _eVCheckDetailService.CreateAsync(request);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo EVCheckDetail thành công")
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _eVCheckDetailService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá thành công"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] EVCheckDetailRequest request)
        {
            await _eVCheckDetailService.UpdateAsync(id, request);
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Cập nhật thành công")
            );
        }
    }
}
