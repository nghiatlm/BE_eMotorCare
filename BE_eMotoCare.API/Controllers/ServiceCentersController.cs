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
        private readonly IServiceCenterService _svc;

        public ServiceCentersController(IServiceCenterService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PageResult<ServiceCenterResponse>>>> GetPaged(
            [FromQuery] string? search,
            [FromQuery] StatusEnum? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _svc.GetPagedAsync(search, status, page, pageSize);

            if (
                data is null
                || data.RowDatas is null
                || data.RowDatas.Count == 0
                || data.Total == 0
            )
            {
                var respNotFound = ApiResponse<PageResult<ServiceCenterResponse>>.NotFound(
                    "không có dữ liệu"
                );
                return StatusCode((int)respNotFound.StatusCode, respNotFound);
            }

            var resp = ApiResponse<PageResult<ServiceCenterResponse>>.SuccessResponse(
                data,
                "lấy danh sách thành công"
            );
            return StatusCode((int)resp.StatusCode, resp);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<ServiceCenterResponse>>> GetById(Guid id)
        {
            var item = await _svc.GetByIdAsync(id);
            if (item is null)
            {
                var notFound = ApiResponse<ServiceCenterResponse>.NotFound(
                    "không tìm thấy ServiceCenter"
                );
                return StatusCode((int)notFound.StatusCode, notFound);
            }

            var resp = ApiResponse<ServiceCenterResponse>.SuccessResponse(
                item,
                "lấy chi tiết thành công"
            );
            return StatusCode((int)resp.StatusCode, resp);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> Create(
            [FromBody] ServiceCenterRequest req
        )
        {
            var id = await _svc.CreateAsync(req);

            var resp = ApiResponse<object>.CreatedSuccess(new { id }, "tạo thành công");
            return StatusCode((int)resp.StatusCode, resp);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(
            Guid id,
            [FromBody] ServiceCenterRequest req
        )
        {
            await _svc.UpdateAsync(id, req);

            var resp = ApiResponse<object>.SuccessResponse(null, "cập nhật thành công");
            return StatusCode((int)resp.StatusCode, resp);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(Guid id)
        {
            await _svc.DeleteAsync(id);
            var resp = ApiResponse<object>.DeleteSuccess();
            return StatusCode((int)resp.StatusCode, resp);
        }
    }
}
