

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

namespace eMototCare.BLL.Services.PartItemServices
{
    public class PartItemService : IPartItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PartItemService> _logger;
        public PartItemService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PartItemService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<PartItemResponse>> GetPagedAsync(
             Guid? partId,
             string? serialNumber,
             PartItemStatus? status,
             Guid? serviceCenterInventoryId,
             int page,
             int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.PartItems.GetPagedAsync(
                    partId,
                    serialNumber,
                    status,
                    serviceCenterInventoryId,
                    page,
                    pageSize
                );
                var rows = _mapper.Map<List<PartItemResponse>>(items);
                return new PageResult<PartItemResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Part Item failed: {Message}", ex.Message);
                //throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
                throw new AppException(ex.Message);
            }
        }

        public async Task<Guid> CreateAsync(PartItemRequest req)
        {

            try
            {
                var serialNumber = req.SerialNumber.Trim();

                if (await _unitOfWork.PartItems.ExistsSerialNumberAsync(serialNumber))
                    throw new AppException("Serial Number đã tồn tại", HttpStatusCode.Conflict);
                if (req.WarantyEndDate != null && req.WarantyStartDate == null)
                    throw new AppException("Phải có ngày bắt đầu bảo hành", HttpStatusCode.BadRequest);
                if (req.WarantyEndDate == null && req.WarantyStartDate != null)
                    throw new AppException("Phải có ngày kết thúc bảo hành", HttpStatusCode.BadRequest);
                if (req.WarantyStartDate != null && req.WarantyEndDate != null)
                {
                    if (req.WarantyEndDate < req.WarantyStartDate)
                        throw new AppException("Ngày kết thúc bảo hành không thể nhỏ hơn ngày bắt đầu.", HttpStatusCode.BadRequest);
                }
                var entity = _mapper.Map<PartItem>(req);
                entity.Id = Guid.NewGuid();
                entity.SerialNumber = serialNumber;
                entity.Status = PartItemStatus.ACTIVE;

                await _unitOfWork.PartItems.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created Part Item");
                return entity.Id;

            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Part Item failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.PartItems.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy Part Item",
                        HttpStatusCode.NotFound
                    );

                entity.Status = PartItemStatus.IN_ACTIVE;
                await _unitOfWork.PartItems.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted Part Item {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete Part Item failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, PartItemUpdateRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.PartItems.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy Part Item",
                        HttpStatusCode.NotFound
                    );

                


                if (req.PartId != null)
                    entity.PartId =  req.PartId.Value;
                if (req.Quantity != null)
                    entity.Quantity = req.Quantity.Value;
                if (req.SerialNumber != null)
                {
                    var code = req.SerialNumber.Trim();
                    if (
                        !string.Equals(entity.SerialNumber, code, StringComparison.OrdinalIgnoreCase)
                        && await _unitOfWork.PartItems.ExistsSerialNumberAsync(code)
                    )
                        throw new AppException("Serial Number đã tồn tại", HttpStatusCode.Conflict);
                    entity.SerialNumber = code;
                }
                if (req.Price != null)
                    entity.Price = req.Price.Value;
                if (req.Status != null)
                    entity.Status = req.Status.Value;
                if (req.WarrantyPeriod != null)
                    entity.WarrantyPeriod = req.WarrantyPeriod;
                if (req.WarantyStartDate != null && req.WarantyEndDate != null)
                {
                    if (req.WarantyEndDate < req.WarantyStartDate)
                        throw new InvalidOperationException("Warranty end date cannot be earlier than start date.");

                    entity.WarantyStartDate = req.WarantyStartDate;
                    entity.WarantyEndDate = req.WarantyEndDate;
                }
                else
                {
                    if (req.WarantyStartDate != null)
                    {
                        if (entity.WarantyEndDate != null && req.WarantyStartDate > entity.WarantyEndDate)
                            throw new InvalidOperationException("Warranty start date cannot be later than the current end date.");

                        entity.WarantyStartDate = req.WarantyStartDate;
                    }

                    if (req.WarantyEndDate != null)
                    {
                        if (entity.WarantyStartDate != null && req.WarantyEndDate < entity.WarantyStartDate)
                            throw new InvalidOperationException("Warranty end date cannot be earlier than the current start date.");

                        entity.WarantyEndDate = req.WarantyEndDate;
                    }
                }
                if (req.ServiceCenterInventoryId != null)
                    entity.ServiceCenterInventoryId = req.ServiceCenterInventoryId;



                await _unitOfWork.PartItems.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated Part Item {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Part Item failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }


        }

        public async Task<PartItemResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.PartItems.GetByIdAsync(id);
                if (entity is null)
                    throw new AppException("Không tìm thấy Part Item", HttpStatusCode.NotFound);

                return _mapper.Map<PartItemResponse>(entity);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById Part Item failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
