using BE_eMotoCare.API.Realtime.Services;
using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.NotificationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/notifications")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;
        private readonly INotifierService _notifier;

        public NotificationsController(INotificationService service, INotifierService notifier)
        {
            _service = service;
            _notifier = notifier;
        }

        [HttpGet]
        public async Task<IActionResult> GetByParams(
            [FromQuery] Guid? receiverId,
            [FromQuery] NotificationEnum? notificationType,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _service.GetPagedAsync(receiverId, notificationType, page, pageSize);
            return Ok(
                ApiResponse<PageResult<NotificationResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách Notification thành công"
                )
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NotificationRequest request)
        {
            var id = await _service.CreateAsync(request);
            await _notifier.NotifyCreateAsync("Notifier created", new {request.Title});
            return Ok(ApiResponse<object>.SuccessResponse(new { id }, "Tạo Notification thành công"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            return Ok(ApiResponse<NotificationResponse>.SuccessResponse(item, "Lấy Notification thành công"));
        }
    }
}
