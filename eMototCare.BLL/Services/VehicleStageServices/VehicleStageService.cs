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
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.VehicleStageServices
{
    public class VehicleStageService : IVehicleStageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<VehicleStageService> _logger;

        public VehicleStageService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<VehicleStageService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<VehicleStageResponse>> GetPagedAsync(
            Guid? vehicleId,
            Guid? maintenanceStageId,
            VehicleStageStatus? status,
            DateTime? fromDate,
            DateTime? toDate,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.VehicleStages.GetPagedAsync(
                    vehicleId,
                    maintenanceStageId,
                    status,
                    fromDate,
                    toDate,
                    page,
                    pageSize
                );
                if (page <= 0)
                    throw new AppException("Page phải > 0", HttpStatusCode.BadRequest);

                if (pageSize <= 0)
                    throw new AppException("PageSize phải > 0", HttpStatusCode.BadRequest);

                if (vehicleId.HasValue && vehicleId.Value == Guid.Empty)
                    throw new AppException("VehicleId không hợp lệ", HttpStatusCode.BadRequest);

                if (maintenanceStageId.HasValue && maintenanceStageId.Value == Guid.Empty)
                    throw new AppException(
                        "MaintenanceStageId không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (status.HasValue && !Enum.IsDefined(typeof(VehicleStageStatus), status.Value))
                    throw new AppException(
                        "Trạng thái mốc bảo dưỡng không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (fromDate.HasValue && toDate.HasValue && fromDate > toDate)
                    throw new AppException(
                        "fromDate không được lớn hơn toDate",
                        HttpStatusCode.BadRequest
                    );

                var rows = _mapper.Map<List<VehicleStageResponse>>(items);
                return new PageResult<VehicleStageResponse>(rows, pageSize, page, (int)total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged VehicleStage failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<VehicleStageResponse?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);
            var entity =
                await _unitOfWork.VehicleStages.GetByIdAsync(id)
                ?? throw new AppException("Không tìm thấy mốc bảo dưỡng", HttpStatusCode.NotFound);
            return _mapper.Map<VehicleStageResponse>(entity);
        }

        public async Task<Guid> CreateAsync(VehicleStageRequest req)
        {
            try
            {
                var entity = _mapper.Map<VehicleStage>(req);
                entity.Id = Guid.NewGuid();
                if (req == null)
                    throw new AppException("Request không được null", HttpStatusCode.BadRequest);

                if (req.VehicleId == Guid.Empty)
                    throw new AppException("VehicleId không hợp lệ", HttpStatusCode.BadRequest);

                if (req.MaintenanceStageId == Guid.Empty)
                    throw new AppException(
                        "MaintenanceStageId không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (!Enum.IsDefined(typeof(MaintenanceUnit), req.ActualMaintenanceUnit))
                    throw new AppException(
                        "Đơn vị bảo dưỡng không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (!Enum.IsDefined(typeof(VehicleStageStatus), req.Status))
                    throw new AppException(
                        "Trạng thái mốc bảo dưỡng không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (req.ActualMaintenanceMileage < 0)
                    throw new AppException(
                        "ActualMaintenanceMileage không được âm",
                        HttpStatusCode.BadRequest
                    );

                if (
                    req.ExpectedStartDate.HasValue
                    && req.ExpectedEndDate.HasValue
                    && req.ExpectedStartDate.Value > req.ExpectedEndDate.Value
                )
                {
                    throw new AppException(
                        "ExpectedStartDate không được lớn hơn ExpectedEndDate",
                        HttpStatusCode.BadRequest
                    );
                }

                var vehicle =
                    await _unitOfWork.Vehicles.GetByIdAsync(req.VehicleId)
                    ?? throw new AppException("Không tìm thấy xe", HttpStatusCode.BadRequest);

                var stage =
                    await _unitOfWork.MaintenanceStages.GetByIdAsync(req.MaintenanceStageId)
                    ?? throw new AppException(
                        "Không tìm thấy mốc bảo dưỡng trong kế hoạch",
                        HttpStatusCode.BadRequest
                    );

                await _unitOfWork.VehicleStages.CreateAsync(entity);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation(
                    "Created VehicleStage {Id} for Vehicle {VehicleId}",
                    entity.Id,
                    entity.VehicleId
                );
                return entity.Id;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create VehicleStage failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, VehicleStageRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.VehicleStages.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy mốc bảo dưỡng",
                        HttpStatusCode.NotFound
                    );
                if (id == Guid.Empty)
                    throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);

                if (req == null)
                    throw new AppException("Request không được null", HttpStatusCode.BadRequest);

                if (req.VehicleId == Guid.Empty)
                    throw new AppException("VehicleId không hợp lệ", HttpStatusCode.BadRequest);

                if (req.MaintenanceStageId == Guid.Empty)
                    throw new AppException(
                        "MaintenanceStageId không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (!Enum.IsDefined(typeof(MaintenanceUnit), req.ActualMaintenanceUnit))
                    throw new AppException(
                        "Đơn vị bảo dưỡng không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (!Enum.IsDefined(typeof(VehicleStageStatus), req.Status))
                    throw new AppException(
                        "Trạng thái mốc bảo dưỡng không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (req.ActualMaintenanceMileage < 0)
                    throw new AppException(
                        "ActualMaintenanceMileage không được âm",
                        HttpStatusCode.BadRequest
                    );

                if (
                    req.ExpectedStartDate.HasValue
                    && req.ExpectedEndDate.HasValue
                    && req.ExpectedStartDate.Value > req.ExpectedEndDate.Value
                )
                {
                    throw new AppException(
                        "ExpectedStartDate không được lớn hơn ExpectedEndDate",
                        HttpStatusCode.BadRequest
                    );
                }
                var vehicle =
                    await _unitOfWork.Vehicles.GetByIdAsync(req.VehicleId)
                    ?? throw new AppException("Không tìm thấy xe", HttpStatusCode.BadRequest);

                var stage =
                    await _unitOfWork.MaintenanceStages.GetByIdAsync(req.MaintenanceStageId)
                    ?? throw new AppException(
                        "Không tìm thấy mốc bảo dưỡng trong kế hoạch",
                        HttpStatusCode.BadRequest
                    );
                _mapper.Map(req, entity);
                await _unitOfWork.VehicleStages.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation("Updated VehicleStage {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update VehicleStage failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.VehicleStages.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy mốc bảo dưỡng",
                        HttpStatusCode.NotFound
                    );
                if (id == Guid.Empty)
                    throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);
                await _unitOfWork.VehicleStages.DeleteAsync(entity);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation("Deleted VehicleStage {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete VehicleStage failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
