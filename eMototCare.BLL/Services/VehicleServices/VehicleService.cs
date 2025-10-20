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

namespace eMototCare.BLL.Services.VehicleServices
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<VehicleService> _logger;

        public VehicleService(IUnitOfWork uow, IMapper mapper, ILogger<VehicleService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<VehicleResponse>> GetPagedAsync(
            string? search,
            StatusEnum? status,
            Guid? modelId,
            Guid? customerId,
            DateTime? fromPurchaseDate,
            DateTime? toPurchaseDate,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _uow.Vehicles.GetPagedAsync(
                    search,
                    status,
                    modelId,
                    customerId,
                    fromPurchaseDate,
                    toPurchaseDate,
                    page,
                    pageSize
                );

                var rows = _mapper.Map<List<VehicleResponse>>(items);
                return new PageResult<VehicleResponse>(rows, pageSize, page, (int)total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Vehicle failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<VehicleResponse?> GetByIdAsync(Guid id)
        {
            var v =
                await _uow.Vehicles.GetByIdAsync(id)
                ?? throw new AppException("Không tìm thấy xe", HttpStatusCode.NotFound);
            return _mapper.Map<VehicleResponse>(v);
        }

        public async Task<Guid> CreateAsync(VehicleRequest req)
        {
            try
            {
                var entity = _mapper.Map<Vehicle>(req);
                entity.Id = Guid.NewGuid();

                await _uow.Vehicles.CreateAsync(entity);
                await _uow.SaveAsync();

                _logger.LogInformation("Created Vehicle {Id} ({Vin})", entity.Id, entity.VinNUmber);
                return entity.Id;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Vehicle failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, VehicleRequest req)
        {
            try
            {
                var entity =
                    await _uow.Vehicles.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy xe", HttpStatusCode.NotFound);

                _mapper.Map(req, entity);
                await _uow.Vehicles.UpdateAsync(entity);
                await _uow.SaveAsync();

                _logger.LogInformation("Updated Vehicle {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Vehicle failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _uow.Vehicles.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy xe", HttpStatusCode.NotFound);

                await _uow.Vehicles.DeleteAsync(entity);
                await _uow.SaveAsync();

                _logger.LogInformation("Deleted Vehicle {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete Vehicle failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
