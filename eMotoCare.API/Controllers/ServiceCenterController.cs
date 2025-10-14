using eMotoCare.BLL.Services.ServiceCenterServices;
using eMotoCare.Common.Exceptions;
using eMotoCare.Common.Models.ApiResponse;
using eMotoCare.Common.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/service-centers")]
    [Authorize(Roles = "ROLE_ADMIN")]
    public class ServiceCenterController : ControllerBase
    {
        private readonly IServiceCenterService _service;

        public ServiceCenterController(IServiceCenterService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _service.GetPagedAsync(search, page, pageSize);

            if (
                data == null
                || data.RowDatas == null
                || data.RowDatas.Count == 0
                || data.Total == 0
            )
                throw new AppException(ErrorCode.LIST_EMPTY);

            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Get service centers successfully",
                    Data = data,
                }
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var sc = await _service.GetByIdAsync(id);
            if (sc is null)
                throw new AppException(ErrorCode.NOT_FOUND);

            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Get service center successfully",
                    Data = sc,
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceCenterRequest req)
        {
            var id = await _service.CreateAsync(req);
            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Service center created",
                    Data = new { id },
                }
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ServiceCenterRequest req)
        {
            await _service.UpdateAsync(id, req);
            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Service center updated",
                }
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Service center deleted",
                }
            );
        }
    }
}
