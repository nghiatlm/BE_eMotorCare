using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.PartTypeServices;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/part-types")]
    [ApiController]
    public class PartTypeController : ControllerBase
    {
        private readonly IPartTypeService _partTypeService;
        public PartTypeController(IPartTypeService partTypeService)
        {
            _partTypeService = partTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetByParams(
            [FromQuery] string? name,
            [FromQuery] string? description,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _partTypeService.GetPagedAsync(name, description, page, pageSize);
            return Ok(
                ApiResponse<PageResult<PartTypeResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách Part Type thành công"
                )
            );
        }
    }
}
