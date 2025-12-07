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
                var (items, total) = await _unitOfWork.ServiceCenters.GetDtoPagedAsync(
                    search,
                    status,
                    page,
                    pageSize
                );
                if (page <= 0)
                    throw new AppException("Page phải > 0", HttpStatusCode.BadRequest);

                if (pageSize <= 0)
                    throw new AppException("PageSize phải > 0", HttpStatusCode.BadRequest);

                if (status.HasValue && !Enum.IsDefined(typeof(StatusEnum), status.Value))
                    throw new AppException("Trạng thái không hợp lệ", HttpStatusCode.BadRequest);
                return new PageResult<ServiceCenterResponse>(
                    items.ToList(),
                    pageSize,
                    page,
                    (int)total
                );
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
                if (id == Guid.Empty)
                    throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);
                var dto = await _unitOfWork.ServiceCenters.GetDtoByIdAsync(id);
                if (dto is null)
                    throw new AppException("Không tìm thấy ServiceCenter", HttpStatusCode.NotFound);

                return dto;
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
                if (req == null)
                    throw new AppException("Request không được null", HttpStatusCode.BadRequest);

                var email = req.Email.Trim().ToLowerInvariant();
                var phone = req.Phone.Trim();

                if (string.IsNullOrWhiteSpace(req.Name))
                    throw new AppException(
                        "Tên ServiceCenter không được để trống",
                        HttpStatusCode.BadRequest
                    );

                if (string.IsNullOrWhiteSpace(email))
                    throw new AppException("Email không được để trống", HttpStatusCode.BadRequest);

                if (string.IsNullOrWhiteSpace(phone))
                    throw new AppException(
                        "Số điện thoại không được để trống",
                        HttpStatusCode.BadRequest
                    );

                if (string.IsNullOrWhiteSpace(req.Address))
                    throw new AppException(
                        "Địa chỉ không được để trống",
                        HttpStatusCode.BadRequest
                    );

                if (!Enum.IsDefined(typeof(StatusEnum), req.Status))
                    throw new AppException("Trạng thái không hợp lệ", HttpStatusCode.BadRequest);

                if (await _unitOfWork.ServiceCenters.ExistsEmailAsync(email))
                    throw new AppException("Email đã tồn tại", HttpStatusCode.Conflict);

                if (await _unitOfWork.ServiceCenters.ExistsPhoneAsync(phone))
                    throw new AppException("Số điện thoại đã tồn tại", HttpStatusCode.Conflict);
                var count = await _unitOfWork.ServiceCenters.FindAllAsync();
                var generatedCode = $"SVCT-{count.Count + 1:D5}";

                var entity = _mapper.Map<ServiceCenter>(req);
                entity.Id = Guid.NewGuid();
                entity.Code = generatedCode;
                entity.Email = email;
                entity.Phone = phone;

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
                if (id == Guid.Empty)
                    throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);

                if (req == null)
                    throw new AppException("Request không được null", HttpStatusCode.BadRequest);
                var entity =
                    await _unitOfWork.ServiceCenters.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy ServiceCenter",
                        HttpStatusCode.NotFound
                    );

                var newCode = req.Code.Trim();
                var newEmail = req.Email.Trim().ToLowerInvariant();
                var newPhone = req.Phone.Trim();
                if (string.IsNullOrWhiteSpace(newCode))
                    throw new AppException(
                        "Mã ServiceCenter không được để trống",
                        HttpStatusCode.BadRequest
                    );

                if (string.IsNullOrWhiteSpace(req.Name))
                    throw new AppException(
                        "Tên ServiceCenter không được để trống",
                        HttpStatusCode.BadRequest
                    );

                if (string.IsNullOrWhiteSpace(newEmail))
                    throw new AppException("Email không được để trống", HttpStatusCode.BadRequest);

                if (string.IsNullOrWhiteSpace(newPhone))
                    throw new AppException(
                        "Số điện thoại không được để trống",
                        HttpStatusCode.BadRequest
                    );

                if (string.IsNullOrWhiteSpace(req.Address))
                    throw new AppException(
                        "Địa chỉ không được để trống",
                        HttpStatusCode.BadRequest
                    );

                if (!Enum.IsDefined(typeof(StatusEnum), req.Status))
                    throw new AppException("Trạng thái không hợp lệ", HttpStatusCode.BadRequest);
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
                if (id == Guid.Empty)
                    throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);

                var entity =
                    await _unitOfWork.ServiceCenters.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy ServiceCenter",
                        HttpStatusCode.NotFound
                    );

                entity.Status = StatusEnum.IN_ACTIVE;

                await _unitOfWork.ServiceCenters.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("ServiceCenter {Id} status changed to IN_ACTIVE", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to change ServiceCenter status: {Message}",
                    ex.Message
                );
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
