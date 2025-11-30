

using AutoMapper;
using eMotoCare.BO.Common.src;
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

namespace eMototCare.BLL.Services.ServiceCenterInventoryServices
{
    public class ServiceCenterInventoryService : IServiceCenterInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceCenterInventoryService> _logger;

        public ServiceCenterInventoryService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ServiceCenterInventoryService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<ServiceCenterInventoryResponse>> GetPagedAsync(
            Guid? serviceCenterId,
            string? serviceCenterInventoryName,
            Status? status,
            string? partCode,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.ServiceCenterInventories.GetPagedAsync(
                    serviceCenterId,
                    serviceCenterInventoryName,
                    status,
                    partCode,
                    null,
                    null,
                    page,
                    pageSize
                );
                var rows = _mapper.Map<List<ServiceCenterInventoryResponse>>(items);
                return new PageResult<ServiceCenterInventoryResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged failed: {Message}", ex.Message);
                //throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
                throw new AppException(ex.Message);
            }
        }

        public async Task<Guid> CreateAsync(ServiceCenterInventoryRequest req)
        {

            try
            {

                var entity = _mapper.Map<ServiceCenterInventory>(req);
                var sc = await _unitOfWork.ServiceCenterInventories.GetByServiceCenterId(req.ServiceCenterId);

                if (sc != null) throw new AppException(
                    "SC đã tồn tại kho.",
                    HttpStatusCode.NotFound
                );


                entity.Id = Guid.NewGuid();
                entity.Status = Status.ACTIVE;

                await _unitOfWork.ServiceCenterInventories.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created");
                return entity.Id;

            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.ServiceCenterInventories.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy kho",
                        HttpStatusCode.NotFound
                    );

                entity.Status = Status.IN_ACTIVE;
                await _unitOfWork.ServiceCenterInventories.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Disable inventory {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Disable failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, ServiceCenterInventoryUpdateRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.ServiceCenterInventories.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy kho",
                        HttpStatusCode.NotFound
                    );



                if (req.ServiceCenterId != null)
                    entity.ServiceCenterId = req.ServiceCenterId.Value;

                if (req.ServiceCenterInventoryName != null)
                    entity.ServiceCenterInventoryName = req.ServiceCenterInventoryName;

                if (req.Status != null)
                    entity.Status = req.Status.Value;

                await _unitOfWork.ServiceCenterInventories.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }


        }

        public async Task<ServiceCenterInventoryResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.ServiceCenterInventories.GetByIdAsync(id);
                if (entity is null)
                    throw new AppException("Không tìm thấy kho", HttpStatusCode.NotFound);

                return _mapper.Map<ServiceCenterInventoryResponse>(entity);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
