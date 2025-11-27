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
                if (page <= 0)
                    throw new AppException("Page phải > 0", HttpStatusCode.BadRequest);

                if (pageSize <= 0)
                    throw new AppException("PageSize phải > 0", HttpStatusCode.BadRequest);

                if (vehicleId.HasValue && vehicleId.Value == Guid.Empty)
                    throw new AppException("VehicleId không hợp lệ", HttpStatusCode.BadRequest);

                if (partItemId.HasValue && partItemId.Value == Guid.Empty)
                    throw new AppException("PartItemId không hợp lệ", HttpStatusCode.BadRequest);

                if (replaceForId.HasValue && replaceForId.Value == Guid.Empty)
                    throw new AppException("ReplaceForId không hợp lệ", HttpStatusCode.BadRequest);

                if (
                    fromInstallDate.HasValue
                    && toInstallDate.HasValue
                    && fromInstallDate > toInstallDate
                )
                {
                    throw new AppException(
                        "fromInstallDate không được lớn hơn toInstallDate",
                        HttpStatusCode.BadRequest
                    );
                }
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
            if (id == Guid.Empty)
                throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);
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
                if (req == null)
                    throw new AppException("Request không được null", HttpStatusCode.BadRequest);

                if (req.VehicleId == Guid.Empty)
                    throw new AppException("VehicleId không hợp lệ", HttpStatusCode.BadRequest);

                if (req.PartItemId == Guid.Empty)
                    throw new AppException("PartItemId không hợp lệ", HttpStatusCode.BadRequest);

                if (req.ReplaceForId.HasValue && req.ReplaceForId.Value == Guid.Empty)
                    throw new AppException("ReplaceForId không hợp lệ", HttpStatusCode.BadRequest);

                if (req.InstallDate == default)
                    throw new AppException("InstallDate không hợp lệ", HttpStatusCode.BadRequest);
                var entity = _mapper.Map<VehiclePartItem>(req);
                entity.Id = Guid.NewGuid();
                var vehicle =
                    await _unitOfWork.Vehicles.GetByIdAsync(req.VehicleId)
                    ?? throw new AppException("Không tìm thấy xe", HttpStatusCode.BadRequest);

                var partItem =
                    await _unitOfWork.PartItems.GetByIdAsync(req.PartItemId)
                    ?? throw new AppException("Không tìm thấy PartItem", HttpStatusCode.BadRequest);

                if (req.ReplaceForId.HasValue)
                {
                    var oldItem = await _unitOfWork.VehiclePartItems.GetByIdAsync(
                        req.ReplaceForId.Value
                    );
                    if (oldItem == null)
                        throw new AppException(
                            "Không tìm thấy linh kiện cần thay thế (ReplaceForId).",
                            HttpStatusCode.BadRequest
                        );
                }
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
                if (id == Guid.Empty)
                    throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);

                if (req == null)
                    throw new AppException("Request không được null", HttpStatusCode.BadRequest);

                if (req.VehicleId == Guid.Empty)
                    throw new AppException("VehicleId không hợp lệ", HttpStatusCode.BadRequest);

                if (req.PartItemId == Guid.Empty)
                    throw new AppException("PartItemId không hợp lệ", HttpStatusCode.BadRequest);

                if (req.ReplaceForId.HasValue && req.ReplaceForId.Value == Guid.Empty)
                    throw new AppException("ReplaceForId không hợp lệ", HttpStatusCode.BadRequest);

                if (req.InstallDate == default)
                    throw new AppException("InstallDate không hợp lệ", HttpStatusCode.BadRequest);
                var entity =
                    await _unitOfWork.VehiclePartItems.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy linh kiện gắn trên xe",
                        HttpStatusCode.NotFound
                    );
                var vehicle =
                    await _unitOfWork.Vehicles.GetByIdAsync(req.VehicleId)
                    ?? throw new AppException("Không tìm thấy xe", HttpStatusCode.BadRequest);

                var partItem =
                    await _unitOfWork.PartItems.GetByIdAsync(req.PartItemId)
                    ?? throw new AppException("Không tìm thấy PartItem", HttpStatusCode.BadRequest);

                if (req.ReplaceForId.HasValue)
                {
                    var oldItem = await _unitOfWork.VehiclePartItems.GetByIdAsync(
                        req.ReplaceForId.Value
                    );
                    if (oldItem == null)
                        throw new AppException(
                            "Không tìm thấy linh kiện cần thay thế (ReplaceForId).",
                            HttpStatusCode.BadRequest
                        );
                }
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
                if (id == Guid.Empty)
                    throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);
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
