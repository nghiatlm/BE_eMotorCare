using System.Net;
using AutoMapper;
using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.ServiceCenterServices
{
    public class ServiceCenterService : IServiceCenterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceCenterService> _logger;

        public ServiceCenterService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ServiceCenterService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<ServiceCenterResponse>> GetPagedAsync(
            string? search,
            StatusEnum? status,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.ServiceCenters.GetPagedAsync(
                    search,
                    status,
                    page,
                    pageSize
                );
                var rows = _mapper.Map<List<ServiceCenterResponse>>(items);
                return new PageResult<ServiceCenterResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged ServiceCenter failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceCenterResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var sc = await _unitOfWork.ServiceCenters.GetByIdAsync(id);
                if (sc is null)
                    throw new AppException("Không tìm thấy ServiceCenter", HttpStatusCode.NotFound);

                return _mapper.Map<ServiceCenterResponse>(sc);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById ServiceCenter failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Guid> CreateAsync(ServiceCenterRequest req)
        {
            try
            {
                var code = req.Code.Trim();
                var email = req.Email.Trim().ToLowerInvariant();
                var phone = req.Phone.Trim();

                if (await _unitOfWork.ServiceCenters.ExistsCodeAsync(code))
                    throw new AppException("Mã ServiceCenter đã tồn tại", HttpStatusCode.Conflict);
                if (await _unitOfWork.ServiceCenters.ExistsEmailAsync(email))
                    throw new AppException("Email đã tồn tại", HttpStatusCode.Conflict);
                if (await _unitOfWork.ServiceCenters.ExistsPhoneAsync(phone))
                    throw new AppException("Số điện thoại đã tồn tại", HttpStatusCode.Conflict);

                var entity = _mapper.Map<ServiceCenter>(req);
                entity.Id = Guid.NewGuid();
                entity.Code = code;
                entity.Email = email;
                entity.Phone = phone;
                var count = _unitOfWork.Customers.FindAll().Count;
                entity.Code = $"SVCT-{count + 1:D5}";
                await _unitOfWork.ServiceCenters.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation(
                    "Created ServiceCenter {Code} ({Id})",
                    entity.Code,
                    entity.Id
                );
                return entity.Id;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create ServiceCenter failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, ServiceCenterRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.ServiceCenters.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy ServiceCenter",
                        HttpStatusCode.NotFound
                    );

                var newCode = req.Code.Trim();
                var newEmail = req.Email.Trim().ToLowerInvariant();
                var newPhone = req.Phone.Trim();

                if (
                    !string.Equals(entity.Code, newCode, StringComparison.OrdinalIgnoreCase)
                    && await _unitOfWork.ServiceCenters.ExistsCodeAsync(newCode)
                )
                    throw new AppException("Mã ServiceCenter đã tồn tại", HttpStatusCode.Conflict);

                if (
                    !string.Equals(entity.Email, newEmail, StringComparison.OrdinalIgnoreCase)
                    && await _unitOfWork.ServiceCenters.ExistsEmailAsync(newEmail)
                )
                    throw new AppException("Email đã tồn tại", HttpStatusCode.Conflict);

                if (
                    !string.Equals(entity.Phone, newPhone, StringComparison.OrdinalIgnoreCase)
                    && await _unitOfWork.ServiceCenters.ExistsPhoneAsync(newPhone)
                )
                    throw new AppException("Số điện thoại đã tồn tại", HttpStatusCode.Conflict);

                _mapper.Map(req, entity);
                entity.Code = newCode;
                entity.Email = newEmail;
                entity.Phone = newPhone;

                await _unitOfWork.ServiceCenters.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated ServiceCenter {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update ServiceCenter failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.ServiceCenters.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy ServiceCenter",
                        HttpStatusCode.NotFound
                    );

                await _unitOfWork.ServiceCenters.DeleteAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted ServiceCenter {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete ServiceCenter failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
