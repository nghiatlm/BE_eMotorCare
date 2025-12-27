

using AutoMapper;
using eMotoCare.BO.Common.src;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.RegularExpressions;

namespace eMototCare.BLL.Services.RMAServices
{
    public class RMAService : IRMAService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<RMAService> _logger;

        public RMAService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<RMAService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<RMAResponse>> GetPagedAsync(
             string? code,
             DateTime? fromDate,
             DateTime? toDate,
             string? returnAddress,
             RMAStatus? status,
             Guid? createdById,
             Guid? serviceCenterId,
             int page,
             int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.RMAs.GetPagedAsync(
                    code,
                    fromDate,
                    toDate,
                    returnAddress,
                    status,
                    createdById,
                    serviceCenterId,
                    page,
                    pageSize
                );
                var rows = _mapper.Map<List<RMAResponse>>(items);
                return new PageResult<RMAResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (AutoMapperMappingException ex)    
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged RMA failed: {Message}", ex.InnerException.Message);
                //throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
                throw new AppException(ex.Message);
            }
        }

        public async Task<Guid> CreateAsync(RMARequest req)
        {

            try
            {

                var entity = _mapper.Map<RMA>(req);
                entity.Id = Guid.NewGuid();
                entity.RMADate = DateTime.Now;
                entity.Code = $"RMA-{entity.RMADate:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
                entity.Status = RMAStatus.PENDING;
                var match = Regex.Match(entity.Note, @"APPT-\d{8}-\d+");
                var code = match.Success ? match.Value : null;
                if (code != null)
                {
                    var appointment = await _unitOfWork.Appointments.GetByCodeAsync(code)
                    ?? throw new AppException("Không tìm thấy cuộc hẹn", HttpStatusCode.NotFound);
                    var notification = new eMotoCare.BO.Entities.Notification
                    {
                        Id = Guid.NewGuid(),
                        Title = "Yêu cầu RMA đã được tạo",
                        Message = "Yêu cầu bảo hành hãng của bạn đã được tạo và đang chờ xét duyệt. Chúng tôi sẽ cập nhật cho bạn sớm nhất có thể khi có kết quả được gửi về từ hãng.",
                        ReceiverId = appointment.Customer.AccountId.Value,
                        Type = NotificationEnum.WARRANTY_STATUS,
                        IsRead = false,
                        SentAt = DateTime.Now,
                        ReferenceId = entity.Id
                    };
                    await _unitOfWork.Notifications.CreateAsync(notification);
                }

                await _unitOfWork.RMAs.CreateAsync(entity);
                await _unitOfWork.SaveAsync();
                
                _logger.LogInformation("Created RMA");
                return entity.Id;

            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create RMA failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.RMAs.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy RMA",
                        HttpStatusCode.NotFound
                    );

                entity.Status = RMAStatus.CANCELED;
                await _unitOfWork.RMAs.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Canceled RMA {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cancle RMA failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, RMAUpdateRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.RMAs.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy RMA",
                        HttpStatusCode.NotFound
                    );

                if (req.Code != null)
                {
                    var code = req.Code.Trim();
                    if (
                        !string.Equals(entity.Code, code, StringComparison.OrdinalIgnoreCase)
                        && await _unitOfWork.RMAs.ExistsCodeAsync(code)
                    )
                        throw new AppException("Code đã tồn tại", HttpStatusCode.Conflict);
                    entity.Code = code;
                }
                    

                if (req.RMADate != null)
                    entity.RMADate = req.RMADate.Value;

                if (req.ReturnAddress != null)
                    entity.ReturnAddress = req.ReturnAddress;

                if (req.Note != null)
                    entity.Note = req.Note;

                if (req.Status != null)
                {
                    if (req.Status == RMAStatus.APPROVED)
                    {
                       var rmaDetails = await _unitOfWork.RMADetails.GetByRmaId(entity.Id);
                       foreach (var detail in rmaDetails)
                        {
                            detail.Status = RMADetailStatus.APPROVED;
                            await _unitOfWork.RMADetails.UpdateAsync(detail);
                        }
                    }
                    var match = Regex.Match(entity.Note, @"APPT-\d{8}-\d+");
                    var code = match.Success ? match.Value : null;
                    if (code != null)
                    {
                        var appointment = await _unitOfWork.Appointments.GetByCodeAsync(code)
                        ?? throw new AppException("Không tìm thấy cuộc hẹn", HttpStatusCode.NotFound);
                        var notification = new eMotoCare.BO.Entities.Notification
                        {
                            Id = Guid.NewGuid(),
                            Title = "Yêu cầu RMA đã được cập nhật trạng thái.",
                            Message = "Yêu cầu bảo hành: " + entity.Code + " đã được cập nhật. Vui lòng mở app để xem thông tin chi tiết.",
                            ReceiverId = appointment.Customer.AccountId.Value,
                            Type = NotificationEnum.WARRANTY_STATUS,
                            IsRead = false,
                            SentAt = DateTime.Now,
                            ReferenceId = entity.Id
                        };
                        await _unitOfWork.Notifications.CreateAsync(notification);
                    }
                    entity.Status = req.Status.Value;
                }

                

                if (req.CreateById != null)
                    entity.CreateById = req.CreateById.Value;




                await _unitOfWork.RMAs.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated RMA {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update RMA failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }


        }

        public async Task<RMAResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.RMAs.GetByIdAsync(id);
                if (entity is null)
                    throw new AppException("Không tìm thấy RMA", HttpStatusCode.NotFound);
                var rma = _mapper.Map<RMAResponse>(entity);
                return rma; 
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById RMA failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<List<RMAResponse?>> GetByCustomerIdAsync(Guid customerId)
        {
            var rmas = await _unitOfWork.RMAs.GetByCustomerIdAsync(customerId);
            if (!rmas.Any())
                throw new AppException("Không tìm thấy RMA", HttpStatusCode.NotFound);
            return _mapper.Map<List<RMAResponse>>(rmas);
        }

    }
}
