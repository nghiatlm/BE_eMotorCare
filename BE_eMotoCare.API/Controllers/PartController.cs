﻿using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.PartServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTime.Services;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/parts")]
    [ApiController]
    public class PartController : ControllerBase
    {
        private readonly IPartService _partService;
        private readonly INotifierService _notifier;

        public PartController(IPartService partService, INotifierService notifier)
        {
            _partService = partService;
            _notifier = notifier;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> GetByParams(
            [FromQuery] Guid? partTypeId,
            [FromQuery] string? code,
            [FromQuery] string? name,
            [FromQuery] Status? status,
            [FromQuery] int? quantity,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _partService.GetPagedAsync(partTypeId, code, name, status, quantity, page, pageSize);
            return Ok(
                ApiResponse<PageResult<PartResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách Part thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _partService.GetByIdAsync(id);
            return Ok(
                ApiResponse<PartResponse>.SuccessResponse(
                    item,
                    "Lấy Part thành công"
                )
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Create([FromBody] PartRequest request)
        {
            var id = await _partService.CreateAsync(request);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo Part thành công")
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _partService.DeleteAsync(id);
            await _notifier.NotifyDeleteAsync("Part", new { Id = id });
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá Part thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PartUpdateRequest request)
        {
            await _partService.UpdateAsync(id, request);
            await _notifier.NotifyUpdateAsync("Part", new { Id = id });
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Cập nhật Part thành công")
            );
        }
    }
}
