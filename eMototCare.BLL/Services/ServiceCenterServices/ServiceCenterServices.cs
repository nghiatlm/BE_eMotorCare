using AutoMapper;
using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
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
            var (items, total) = await _unitOfWork.ServiceCenters.GetPagedAsync(
                search,
                status,
                page,
                pageSize
            );
            var rows = _mapper.Map<List<ServiceCenterResponse>>(items);
            return new PageResult<ServiceCenterResponse>(rows, pageSize, page, (int)total);
        }

        public async Task<ServiceCenterResponse?> GetByIdAsync(Guid id)
        {
            var sc = await _unitOfWork.ServiceCenters.GetByIdAsync(id);
            return sc is null ? null : _mapper.Map<ServiceCenterResponse>(sc);
        }

        public async Task<Guid> CreateAsync(ServiceCenterRequest req)
        {
            var code = req.Code.Trim();
            var email = req.Email.Trim().ToLowerInvariant();
            var phone = req.Phone.Trim();

            if (await _unitOfWork.ServiceCenters.ExistsCodeAsync(code))
                throw new Exception("HAS_EXISTED");
            if (await _unitOfWork.ServiceCenters.ExistsEmailAsync(email))
                throw new Exception("HAS_EXISTED");
            if (await _unitOfWork.ServiceCenters.ExistsPhoneAsync(phone))
                throw new Exception("HAS_EXISTED");

            var entity = _mapper.Map<ServiceCenter>(req);
            entity.Id = Guid.NewGuid();
            entity.Code = code;
            entity.Email = email;
            entity.Phone = phone;

            await _unitOfWork.ServiceCenters.CreateAsync(entity);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Created ServiceCenter {Code} ({Id})", entity.Code, entity.Id);
            return entity.Id;
        }

        public async Task UpdateAsync(Guid id, ServiceCenterRequest req)
        {
            var entity =
                await _unitOfWork.ServiceCenters.GetByIdAsync(id)
                ?? throw new Exception("NOT_Found");

            if (
                !string.Equals(entity.Code, req.Code, StringComparison.OrdinalIgnoreCase)
                && await _unitOfWork.ServiceCenters.ExistsCodeAsync(req.Code.Trim())
            )
                throw new Exception("HAS_EXISTED");

            if (
                !string.Equals(entity.Email, req.Email, StringComparison.OrdinalIgnoreCase)
                && await _unitOfWork.ServiceCenters.ExistsEmailAsync(
                    req.Email.Trim().ToLowerInvariant()
                )
            )
                throw new Exception("HAS_EXISTED");

            if (
                !string.Equals(entity.Phone, req.Phone, StringComparison.OrdinalIgnoreCase)
                && await _unitOfWork.ServiceCenters.ExistsPhoneAsync(req.Phone.Trim())
            )
                throw new Exception("HAS_EXISTED");

            _mapper.Map(req, entity);
            entity.Code = req.Code.Trim();
            entity.Email = req.Email.Trim().ToLowerInvariant();
            entity.Phone = req.Phone.Trim();

            await _unitOfWork.ServiceCenters.UpdateAsync(entity);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Updated ServiceCenter {Id}", id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity =
                await _unitOfWork.ServiceCenters.GetByIdAsync(id)
                ?? throw new Exception("not_found");
            ;

            await _unitOfWork.ServiceCenters.DeleteAsync(entity);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Deleted ServiceCenter {Id}", id);
        }
    }
}
