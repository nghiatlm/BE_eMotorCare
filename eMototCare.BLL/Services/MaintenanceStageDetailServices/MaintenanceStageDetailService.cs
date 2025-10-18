

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace eMototCare.BLL.Services.MaintenanceStageDetailServices
{
    public class MaintenanceStageDetailService : IMaintenanceStageDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MaintenanceStageDetailService> _logger;

        public MaintenanceStageDetailService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MaintenanceStageDetailService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<MaintenanceStageDetailResponse>> GetPagedAsync(
            Guid? maintenanceStageId,
            Guid? partId,
            ActionType[]? actionType,
            string? description,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.MaintenanceStageDetails.GetPagedAsync(
                    maintenanceStageId, partId, actionType, description, page, pageSize
                );
                var rows = _mapper.Map<List<MaintenanceStageDetailResponse>>(items);
                return new PageResult<MaintenanceStageDetailResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Maintenance Stage Detail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<MaintenanceStageDetailResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var en = await _unitOfWork.MaintenanceStageDetails.GetByIdAsync(id);
                if (en is null)
                    throw new AppException("Không tìm thấy Maintenance Stage Detail", HttpStatusCode.NotFound);

                return _mapper.Map<MaintenanceStageDetailResponse>(en);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById Maintenance Stage Detail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Guid> CreateAsync(MaintenanceStageDetailRequest req)
        {

            try
            {


                var entity = _mapper.Map<MaintenanceStageDetail>(req);
                entity.Id = Guid.NewGuid();

                await _unitOfWork.MaintenanceStageDetails.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created MaintenanceStageDetail ");
                return entity.Id;

            }
            catch (AppException e)
            {
                throw new AppException (e.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create MaintenanceStageDetail failed: {Message}", ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.MaintenanceStageDetails.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy MaintenanceStageDetail",
                        HttpStatusCode.NotFound
                    );

                await _unitOfWork.MaintenanceStageDetails.DeleteAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted MaintenanceStageDetail {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete MaintenanceStageDetail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, MaintenanceStageDetailRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.MaintenanceStageDetails.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy MaintenanceStageDetail",
                        HttpStatusCode.NotFound
                    );

                _mapper.Map(req, entity);


                await _unitOfWork.MaintenanceStageDetails.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated MaintenanceStageDetail {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update MaintenanceStageDetail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }


        }
    }
}
