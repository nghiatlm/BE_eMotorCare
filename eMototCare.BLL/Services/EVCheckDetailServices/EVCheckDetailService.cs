using System.Net;
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

namespace eMototCare.BLL.Services.EVCheckDetailServices
{
    public class EVCheckDetailService : IEVCheckDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EVCheckDetailService> _logger;

        public EVCheckDetailService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<EVCheckDetailService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<EVCheckDetailResponse>> GetPagedAsync(
            Guid? maintenanceStageDetailId,
            Guid? campaignDetailId,
            Guid? partItemId,
            Guid? eVCheckId,
            Guid? replacePartId,
            string? result,
            string? unit,
            decimal? quantity,
            decimal? pricePart,
            decimal? priceService,
            decimal? totalAmount,
            EVCheckDetailStatus? status,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.EVCheckDetails.GetPagedAsync(
                    maintenanceStageDetailId,
                    campaignDetailId,
                    partItemId,
                    eVCheckId,
                    replacePartId,
                    result,
                    unit,
                    quantity,
                    pricePart,
                    priceService,
                    totalAmount,
                    status,
                    page,
                    pageSize
                );
                var rows = _mapper.Map<List<EVCheckDetailResponse>>(items);
                return new PageResult<EVCheckDetailResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged EVCheckDetail failed: {Message}", ex.Message);
                //throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
                throw new AppException(ex.Message);
            }
        }

        public async Task<Guid> CreateAsync(EVCheckDetailRequest req)
        {
            try
            {
                var entity = _mapper.Map<EVCheckDetail>(req);
                entity.Id = Guid.NewGuid();
                entity.Status = EVCheckDetailStatus.IN_PROGRESS;

                await _unitOfWork.EVCheckDetails.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created EVCheckDetail");
                return entity.Id;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create EVCheckDetail failed: {Message}", ex.Message);
                throw new AppException(ex.InnerException.Message);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.EVCheckDetails.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy EVCheckDetail",
                        HttpStatusCode.NotFound
                    );

                entity.Status = EVCheckDetailStatus.CANCELED;
                await _unitOfWork.EVCheckDetails.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted EVCheckDetail {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete EVCheckDetail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, EVCheckDetailUpdateRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.EVCheckDetails.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy EVCheckDetail",
                        HttpStatusCode.NotFound
                    );

                if (req.MaintenanceStageDetailId != null)
                    entity.MaintenanceStageDetailId = req.MaintenanceStageDetailId.Value;

                // if (req.CampaignDetailId != null)
                //     entity.CampaignDetailId = req.CampaignDetailId.Value;

                if (req.PartItemId != null)
                    entity.PartItemId = req.PartItemId.Value;

                if (req.EVCheckId != null)
                    entity.EVCheckId = req.EVCheckId.Value;

                if (req.ProposedReplacePartId != null)
                    entity.ProposedReplacePartId = req.ProposedReplacePartId.Value;

                if (req.Result != null)
                    entity.Result = req.Result;

                if (req.Remedies != null)
                    entity.Remedies = req.Remedies.Value;

                if (req.Unit != null)
                    entity.Unit = req.Unit;

                if (req.Quantity != null)
                    entity.Quantity = req.Quantity.Value;

                if (req.PricePart != null)
                    entity.PricePart = req.PricePart.Value;

                if (req.PriceService != null)
                    entity.PriceService = req.PriceService.Value;

                if (req.TotalAmount != null)
                    entity.TotalAmount = req.TotalAmount.Value;

                if (req.Status != null)
                {
                    entity.Status = req.Status.Value;

                    //if (req.Status == EVCheckDetailStatus.COMPLETED && entity.ReplacePartId != null && entity.EVCheck.Appointment.Type == ServiceType.MAINTENANCE_TYPE)
                    //{
                    //    var vehiclePartItem = new VehiclePartItem
                    //    {
                    //        Id = Guid.NewGuid(),
                    //        InstallDate = DateTime.UtcNow,
                    //        VehicleId = entity.EVCheck.Appointment.VehicleStage.VehicleId,
                    //        PartItemId = entity.ReplacePartId.Value,
                    //        ReplaceForId = entity.PartItemId,
                    //    };
                    //    await _unitOfWork.VehiclePartItems.CreateAsync(vehiclePartItem);
                    //}
                    //if (req.Status == EVCheckDetailStatus.COMPLETED && entity.ReplacePartId != null && entity.EVCheck.Appointment.Type == ServiceType.REPAIR_TYPE)
                    //{
                    //    var vehiclePartItem = new VehiclePartItem
                    //    {
                    //        Id = Guid.NewGuid(),
                    //        InstallDate = DateTime.UtcNow,
                    //        VehicleId = entity.EVCheck.Appointment.VehicleId.Value,
                    //        PartItemId = entity.ReplacePartId.Value,
                    //        ReplaceForId = entity.PartItemId,
                    //    };
                    //    await _unitOfWork.VehiclePartItems.CreateAsync(vehiclePartItem);
                    //}
                    //if (req.Status == EVCheckDetailStatus.COMPLETED && entity.ReplacePartId != null && entity.Result == "Thay thế phụ tùng mới từ hãng")
                    //{
                    //    var vehiclePartItem = new VehiclePartItem
                    //    {
                    //        Id = Guid.NewGuid(),
                    //        InstallDate = DateTime.UtcNow,
                    //        VehicleId = entity.EVCheck.Appointment.VehicleId.Value,
                    //        PartItemId = entity.ReplacePartId.Value,
                    //        ReplaceForId = entity.PartItemId,
                    //    };
                    //    entity.ReplacePart.Quantity = 0;
                    //    entity.ReplacePart.Status = PartItemStatus.IN_ACTIVE;
                    //    entity.ReplacePart.ServiceCenterInventoryId = null;
                    //    if (entity.ReplacePart.WarrantyPeriod != null)
                    //    {
                    //        var month = entity.ReplacePart.WarrantyPeriod.Value;
                    //        entity.ReplacePart.WarantyStartDate = DateTime.UtcNow;
                    //        entity.ReplacePart.WarantyEndDate = DateTime.UtcNow.AddMonths(month);
                    //    }
                    //    await _unitOfWork.VehiclePartItems.CreateAsync(vehiclePartItem);
                    //}
                }

                await _unitOfWork.EVCheckDetails.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated EVCheckDetail {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update EVCheckDetail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<EVCheckDetailResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.EVCheckDetails.GetByIdAsync(id);
                if (entity is null)
                    throw new AppException("Không tìm thấy EVCheckDetail", HttpStatusCode.NotFound);

                return _mapper.Map<EVCheckDetailResponse>(entity);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById EVCheckDetail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
