using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.CampaignDetailServices;
using eMototCare.BLL.Services.CampaignServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/campaign-details")]
    [ApiController]
    public class CampaignDetailsController : ControllerBase
    {
        private readonly ICampaignDetailService _service;

        public CampaignDetailsController(ICampaignDetailService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_CUSTOMER,ROLE_ADMIN,ROLE_TECHNICIAN")]
        public async Task<IActionResult> GetByParams(
            [FromQuery] Guid? campaignId,
            [FromQuery] Guid? partId,
            [FromQuery] CampaignActionType? actionType,
            [FromQuery] bool? isMandatory,
            [FromQuery] int? estimatedTime,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _service.GetPagedAsync(campaignId, partId, actionType, isMandatory, estimatedTime, page, pageSize);
            return Ok(
                ApiResponse<PageResult<CampaignDetailResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách Campaign Detail thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_CUSTOMER,ROLE_ADMIN,ROLE_TECHNICIAN")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            return Ok(
                ApiResponse<CampaignDetailResponse>.SuccessResponse(
                    item,
                    "Lấy Campaign Detail thành công"
                )
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_ADMIN")]
        public async Task<IActionResult> Create([FromBody] CampaignDetailRequest request)
        {
            var id = await _service.CreateAsync(request);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo Campaign Detail thành công")
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_ADMIN")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá Campaign Detail thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_ADMIN")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CampaignDetailUpdateRequest request)
        {
            await _service.UpdateAsync(id, request);
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Cập nhật Campaign Detail thành công")
            );
        }
    }
}
