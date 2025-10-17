
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
using System.Numerics;

namespace eMototCare.BLL.Services.MaintenancePlanServices
{
    public class MaintenancePlanService : IMaintenancePlanService
    {
        private readonly ILogger<MaintenancePlanService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MaintenancePlanService(ILogger<MaintenancePlanService> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PageResult<MaintenancePlanResponse>> GetPagedAsync(
            string? code,
            string? description,
            string? name,
            int? totalStage,
            Status? status,
            MaintenanceUnit? maintenanceUnit,
            int page = 1,
            int pageSize = 10
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.MaintenancePlans.GetPagedAsync(
                    code,
                    description,
                    name,
                    totalStage,
                    status,
                    maintenanceUnit,
                    page,
                    pageSize
                );
                var rows = _mapper.Map<List<MaintenancePlanResponse>>(items);
                return new PageResult<MaintenancePlanResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Maintenance Plan failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Guid> CreateAsync(MaintenancePlanRequest req)
        {

            try
            {
                var code = req.Code.Trim();
                var name = req.Name.Trim();

                if (await _unitOfWork.MaintenancePlans.ExistsCodeAsync(code))
                    throw new AppException("Code đã tồn tại", HttpStatusCode.Conflict);
                if (await _unitOfWork.MaintenancePlans.ExistsNameAsync(name))
                    throw new AppException("Tên đã tồn tại", HttpStatusCode.Conflict);

                var entity = _mapper.Map<MaintenancePlan>(req);
                entity.Id = Guid.NewGuid();
                entity.Code = code;
                entity.Name = name;

                await _unitOfWork.MaintenancePlans.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created maintenance plan");
                return entity.Id;

            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create maintenance plan failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.MaintenancePlans.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy Maintenance Plan",
                        HttpStatusCode.NotFound
                    );

                await _unitOfWork.MaintenancePlans.DeleteAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted MaintenancePlan {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete MaintenancePlan failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, MaintenancePlanRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.MaintenancePlans.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy Maintenance Plan",
                        HttpStatusCode.NotFound
                    );

                var code = req.Code.Trim();
                var name = req.Name.Trim();


                if (
                    !string.Equals(entity.Code, code, StringComparison.OrdinalIgnoreCase)
                    && await _unitOfWork.MaintenancePlans.ExistsCodeAsync(code)
                )
                    throw new AppException("Code đã tồn tại", HttpStatusCode.Conflict);
                if (
                    !string.Equals(entity.Name, name, StringComparison.OrdinalIgnoreCase)
                    && await _unitOfWork.MaintenancePlans.ExistsNameAsync(name)
                )
                    throw new AppException("Tên đã tồn tại", HttpStatusCode.Conflict);


                _mapper.Map(req, entity);
                entity.Code = code;
                entity.Name = name;


                await _unitOfWork.MaintenancePlans.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated Maintenance Plan {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Maintenance Plan failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }


        }

        public async Task<MaintenancePlanResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var mp = await _unitOfWork.MaintenancePlans.GetByIdAsync(id);
                if (mp is null)
                    throw new AppException("Không tìm thấy Maintenance Plan", HttpStatusCode.NotFound);

                return _mapper.Map<MaintenancePlanResponse>(mp);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById Maintenance Plan failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
