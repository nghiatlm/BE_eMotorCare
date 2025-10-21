using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMototCare.BLL.Services.EVCheckServices;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/evchecks")]
    [ApiController]
    public class EVCheckController : ControllerBase
    {
        private readonly IEVCheckService _evCheckService;

        public EVCheckController(IEVCheckService evCheckService)
        {
            _evCheckService = evCheckService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EVCheckRequest request)
        {
            var id = await _evCheckService.CreateAsync(request);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo EVCheck thành công")
            );
        }
    }
}
