

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace eMototCare.BLL.Services.RMADetailServices
{
    public class RMADetailService : IRMADetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<RMADetailService> _logger;

        public RMADetailService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<RMADetailService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<RMADetailResponse>> GetPagedAsync(
             string? rmaNumber,
             string? inspector,
             string? result,
             string? solution,
             Guid? evCheckDetailId,
             Guid? rmaId,
             RMADetailStatus? status,
             int page,
             int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.RMADetails.GetPagedAsync(
                    rmaNumber,
                    inspector,
                    result,
                    solution,
                    evCheckDetailId,
                    rmaId,
                    status,
                    page,
                    pageSize
                );
                var rows = _mapper.Map<List<RMADetailResponse>>(items);
                return new PageResult<RMADetailResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged RMA Detail failed: {Message}", ex.Message);
                //throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
                throw new AppException(ex.Message);
            }
        }

        public async Task<Guid> CreateAsync(RMADetailRequest req)
        {

            try
            {

                var entity = _mapper.Map<RMADetail>(req);
                entity.Id = Guid.NewGuid();
                entity.Status = RMADetailStatus.PENDING;


                await _unitOfWork.RMADetails.CreateAsync(entity);
                await _unitOfWork.SaveAsync();
                var rmaDetail = await _unitOfWork.RMADetails.GetByIdAsync(entity.Id);
                var notification = new Notification
                {
                    Id = Guid.NewGuid(),
                    Title = "Yêu cầu RMA đã được tạo",
                    Message = "Yêu cầu bảo hành cho bộ phận: " + rmaDetail.EVCheckDetail.PartItem.Part.Name + " đã được khởi tạo. Chúng tôi sẽ cập nhật cho bạn sớm nhất có thể khi có kết quả được gửi về từ hãng.",
                    ReceiverId = rmaDetail.EVCheckDetail.EVCheck.Appointment.Customer.AccountId.Value,
                    Type = NotificationEnum.WARRANTY_STATUS,
                    IsRead = false,
                    SentAt = DateTime.Now,
                };
                await _unitOfWork.Notifications.CreateAsync(notification);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation("Created RMA Detail");
                return entity.Id;

            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create RMA Detail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.RMADetails.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy RMA Detail",
                        HttpStatusCode.NotFound
                    );

                entity.Status = RMADetailStatus.CANCELED;
                await _unitOfWork.RMADetails.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Canceled RMA Detail {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cancle RMA Detail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, RMADetailUpdateRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.RMADetails.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy RMA Detail",
                        HttpStatusCode.NotFound
                    );

                if (req.Quantity != null)
                    entity.Quantity = req.Quantity.Value;

                if (req.Reason != null)
                    entity.Reason = req.Reason;

                if (req.RMANumber != null)
                {
                    var rmaNumber = req.RMANumber.Trim();
                    if (
                        !string.Equals(entity.RMANumber, rmaNumber, StringComparison.OrdinalIgnoreCase)
                        && await _unitOfWork.RMADetails.ExistsRmaNumberAsync(rmaNumber)
                    )
                        throw new AppException("RMANumber đã tồn tại", HttpStatusCode.Conflict);
                    entity.RMANumber = rmaNumber;
                }

                if (req.ReleaseDateRMA != null)
                    entity.ReleaseDateRMA = req.ReleaseDateRMA.Value;

                if (req.ExpirationDateRMA != null)
                    entity.ExpirationDateRMA = req.ExpirationDateRMA.Value;

                if (req.Inspector != null)
                    entity.Inspector = req.Inspector;

                if (req.Result != null)
                    entity.Result = req.Result;

                if (req.Solution != null)
                    entity.Solution = req.Solution;

                if (req.EVCheckDetailId != null)
                    entity.EVCheckDetailId = req.EVCheckDetailId.Value;

                if (req.RMAId != null)
                    entity.RMAId = req.RMAId.Value;

                if (req.Status != null)
                {
                    if (req.Status == RMADetailStatus.APPROVED && entity.RMA.Status != RMAStatus.PROCESSING)
                    {
                        entity.RMA.Status = RMAStatus.PROCESSING;
                    }    
                    entity.Status = req.Status.Value;
                }

                if (req.ReplacePart != null)
                {
                    var partItem = _mapper.Map<PartItem>(req.ReplacePart);
                    var partItemId = Guid.NewGuid();
                    partItem.Id = partItemId;
                    partItem.Status = PartItemStatus.INSTALLED;
                    entity.ReplacePartId = partItemId;
                    entity.EVCheckDetail.PartItem.Status = PartItemStatus.MANUFACTURER_RECALL;
                    await _unitOfWork.PartItems.CreateAsync(partItem);
                    
                }
                var rmaDetail = await _unitOfWork.RMADetails.GetByIdAsync(id);
                var notification = new Notification
                {
                    Id = Guid.NewGuid(),
                    Title = "RMA của bạn đã được cập nhật",
                    Message = "Yêu cầu bảo hành cho bộ phận " + rmaDetail.EVCheckDetail.PartItem.Part.Name + " vừa được cập nhật trạng thái. Vui lòng mở ứng dụng để biết thêm chi tiết.",
                    ReceiverId = rmaDetail.EVCheckDetail.EVCheck.Appointment.Customer.AccountId.Value,
                    Type = NotificationEnum.WARRANTY_STATUS,
                    IsRead = false,
                    SentAt = DateTime.Now,
                };
                await _unitOfWork.RMADetails.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated RMA Detail {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update RMA Detail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }


        }

        public async Task<RMADetailResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.RMADetails.GetByIdAsync(id);
                if (entity is null)
                    throw new AppException("Không tìm thấy RMA Detail", HttpStatusCode.NotFound);

                return _mapper.Map<RMADetailResponse>(entity);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById RMA Detail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

    }
}
