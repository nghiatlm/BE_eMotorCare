using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.CampaignServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/campaigns")]
    [ApiController]
    public class CampaignsController : ControllerBase
    {
        private readonly ICampaignService _service;

        public CampaignsController(ICampaignService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_CUSTOMER,ROLE_ADMIN,ROLE_TECHNICIAN")]
        public async Task<IActionResult> GetByParams(
            [FromQuery] string? code,
            [FromQuery] string? name,
            [FromQuery] CampaignType? campaignType,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] CampaignStatus? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _service.GetPagedAsync(code, name, campaignType, fromDate, toDate, status, page, pageSize);
            return Ok(
                ApiResponse<PageResult<CampaignResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách Campaign thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_CUSTOMER,ROLE_ADMIN,ROLE_TECHNICIAN")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            return Ok(
                ApiResponse<CampaignResponse>.SuccessResponse(
                    item,
                    "Lấy Campaign thành công"
                )
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_ADMIN")]
        public async Task<IActionResult> Create([FromBody] CampaignRequest request)
        {
            var id = await _service.CreateAsync(request);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo Campaign thành công")
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_ADMIN")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Cancel Campaign thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_ADMIN")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CampaignUpdateRequest request)
        {
            await _service.UpdateAsync(id, request);
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Cập nhật Campaign thành công")
            );
        }
    }
}
