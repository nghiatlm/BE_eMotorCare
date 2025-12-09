using System.Globalization;
using System.Net;
using System.Text.Json;
using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.MaintenanceStageServices
{
    public class MaintenanceStageService : IMaintenanceStageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MaintenanceStageService> _logger;

        public MaintenanceStageService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<MaintenanceStageService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<MaintenanceStageResponse>> GetPagedAsync(
            Guid? maintenancePlanId,
            string? description,
            DurationMonth? durationMonth,
            Mileage? mileage,
            string? name,
            Status? status,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.MaintenanceStages.GetPagedAsync(
                    maintenancePlanId,
                    description,
                    durationMonth,
                    mileage,
                    name,
                    status,
                    page,
                    pageSize
                );
                var rows = _mapper.Map<List<MaintenanceStageResponse>>(items);
                return new PageResult<MaintenanceStageResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Maintenance Stage failed: {Message}", ex.Message);
                //throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
                throw new AppException(ex.Message);
            }
        }

        public async Task<Guid> CreateAsync(MaintenanceStageRequest req)
        {
            try
            {
                var entity = _mapper.Map<MaintenanceStage>(req);
                entity.Id = Guid.NewGuid();
                entity.Status = Status.ACTIVE;

                await _unitOfWork.MaintenanceStages.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created Maintenance Stage ");
                return entity.Id;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Maintenance Stage failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.MaintenanceStages.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy Maintenance Stage",
                        HttpStatusCode.NotFound
                    );

                entity.Status = Status.IN_ACTIVE;
                await _unitOfWork.MaintenanceStages.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted Maintenance Stage {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete Maintenance Stage failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, MaintenanceStageUpdateRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.MaintenanceStages.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy Maintenance Stage",
                        HttpStatusCode.NotFound
                    );

                _mapper.Map(req, entity);

                await _unitOfWork.MaintenanceStages.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated Maintenance Stage {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Maintenance Stage failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<MaintenanceStageResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.MaintenanceStages.GetByIdAsync(id);
                if (entity is null)
                    throw new AppException(
                        "Không tìm thấy Maintenance Stage",
                        HttpStatusCode.NotFound
                    );

                return _mapper.Map<MaintenanceStageResponse>(entity);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById Maintenance Stage failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
