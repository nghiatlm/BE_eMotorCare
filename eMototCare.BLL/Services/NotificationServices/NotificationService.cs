

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Exceptions;
using eMotoCare.DAL;
using Microsoft.Extensions.Logging;
using System.Net;

namespace eMototCare.BLL.Services.NotificationServices
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<NotificationService> _logger;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, ILogger<NotificationService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(NotificationRequest req)
        {

            try
            {
                var account = await _unitOfWork.Accounts.GetByIdAsync(req.ReceiverId);
                if (account == null) throw new AppException("Account không tồn tại");
                var entity = new Notification
                {
                    Id = Guid.NewGuid(),
                    ReceiverId = req.ReceiverId,
                    Title = req.Title,
                    Message = req.Message,
                    Type = req.Type,
                    SentAt = req.SentAt,
                };
                entity.IsRead = false;
                

                await _unitOfWork.Notifications.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created Notification");
                return entity.Id;

            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Notification failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<NotificationResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.Notifications.GetByIdAsync(id);
                if (entity is null)
                    throw new AppException("Không tìm thấy Notification", HttpStatusCode.NotFound);

                return _mapper.Map<NotificationResponse>(entity);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById Notification failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
