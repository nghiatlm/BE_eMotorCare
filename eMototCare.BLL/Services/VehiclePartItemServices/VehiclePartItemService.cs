using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.VehiclePartItemServices
{
    public class VehiclePartItemService : IVehiclePartItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<VehiclePartItemService> _logger;

        public VehiclePartItemService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<VehiclePartItemService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<VehiclePartItemResponse>> GetPagedAsync(
            string? search,
            Guid? vehicleId,
            Guid? partItemId,
            Guid? replaceForId,
            DateTime? fromInstallDate,
            DateTime? toInstallDate,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.VehiclePartItems.GetPagedAsync(
                    search,
                    vehicleId,
                    partItemId,
                    replaceForId,
                    fromInstallDate,
                    toInstallDate,
                    page,
                    pageSize
                );

                var rows = _mapper.Map<List<VehiclePartItemResponse>>(items);
                return new PageResult<VehiclePartItemResponse>(rows, pageSize, page, (int)total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged VehiclePartItem failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<VehiclePartItemResponse?> GetByIdAsync(Guid id)
        {
            var v =
                await _unitOfWork.VehiclePartItems.GetByIdAsync(id)
                ?? throw new AppException(
                    "Không tìm thấy linh kiện gắn trên xe",
                    HttpStatusCode.NotFound
                );

            return _mapper.Map<VehiclePartItemResponse>(v);
        }

        public async Task<Guid> CreateAsync(VehiclePartItemRequest req)
        {
            try
            {
                var entity = _mapper.Map<VehiclePartItem>(req);
                entity.Id = Guid.NewGuid();

                await _unitOfWork.VehiclePartItems.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation(
                    "Created VehiclePartItem {Id} for Vehicle {VehicleId}",
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
                _logger.LogError(ex, "Create VehiclePartItem failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, VehiclePartItemRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.VehiclePartItems.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy linh kiện gắn trên xe",
                        HttpStatusCode.NotFound
                    );

                _mapper.Map(req, entity);
                await _unitOfWork.VehiclePartItems.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated VehiclePartItem {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update VehiclePartItem failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.VehiclePartItems.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy linh kiện gắn trên xe",
                        HttpStatusCode.NotFound
                    );

                await _unitOfWork.VehiclePartItems.DeleteAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted VehiclePartItem {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete VehiclePartItem failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
