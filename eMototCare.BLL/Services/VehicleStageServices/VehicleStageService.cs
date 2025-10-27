using System.Net;
using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.VehicleStageServices
{
    public class VehicleStageService : IVehicleStageService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<VehicleStageService> _logger;

        public VehicleStageService(
            IUnitOfWork uow,
            IMapper mapper,
            ILogger<VehicleStageService> logger
        )
        {
            _uow = uow;
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
                var (items, total) = await _uow.VehicleStages.GetPagedAsync(
                    vehicleId,
                    maintenanceStageId,
                    status,
                    fromDate,
                    toDate,
                    page,
                    pageSize
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
            var entity =
                await _uow.VehicleStages.GetByIdAsync(id)
                ?? throw new AppException("Không tìm thấy mốc bảo dưỡng", HttpStatusCode.NotFound);
            return _mapper.Map<VehicleStageResponse>(entity);
        }

        public async Task<Guid> CreateAsync(VehicleStageRequest req)
        {
            try
            {
                var entity = _mapper.Map<VehicleStage>(req);
                entity.Id = Guid.NewGuid();

                await _uow.VehicleStages.CreateAsync(entity);
                await _uow.SaveAsync();
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
                    await _uow.VehicleStages.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy mốc bảo dưỡng",
                        HttpStatusCode.NotFound
                    );

                _mapper.Map(req, entity);
                await _uow.VehicleStages.UpdateAsync(entity);
                await _uow.SaveAsync();
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
                    await _uow.VehicleStages.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy mốc bảo dưỡng",
                        HttpStatusCode.NotFound
                    );

                await _uow.VehicleStages.DeleteAsync(entity);
                await _uow.SaveAsync();
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
