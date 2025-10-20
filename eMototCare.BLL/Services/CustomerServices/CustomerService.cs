using System.Net;
using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CustomerService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<CustomerResponse>> GetPagedAsync(
            string? search,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.Customers.GetPagedAsync(
                    search,
                    page,
                    pageSize
                );
                var rows = _mapper.Map<List<CustomerResponse>>(items);
                return new PageResult<CustomerResponse>(rows, pageSize, page, (int)total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Customer failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<CustomerResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.Customers.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy khách hàng", HttpStatusCode.NotFound);
                return _mapper.Map<CustomerResponse>(entity);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById Customer failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Guid> CreateAsync(CustomerRequest req)
        {
            try
            {
                var citizen = req.CitizenId.Trim();

                if (await _unitOfWork.Customers.ExistsForAccountAsync(req.AccountId))
                    throw new AppException(
                        "Tài khoản đã có hồ sơ khách hàng",
                        HttpStatusCode.Conflict
                    );

                if (await _unitOfWork.Customers.ExistsCitizenAsync(citizen))
                    throw new AppException("CCCD đã tồn tại", HttpStatusCode.Conflict);

                var entity = _mapper.Map<Customer>(req);
                entity.Id = Guid.NewGuid();
                entity.CitizenId = citizen;

                await _unitOfWork.Customers.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation(
                    "Created Customer {Id} (Account {AccountId})",
                    entity.Id,
                    entity.AccountId
                );
                return entity.Id;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Customer failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, CustomerRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.Customers.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy khách hàng", HttpStatusCode.NotFound);

                var newCitizen = req.CitizenId.Trim();
                if (
                    !string.Equals(entity.CitizenId, newCitizen, StringComparison.OrdinalIgnoreCase)
                    && await _unitOfWork.Customers.ExistsCitizenAsync(newCitizen, entity.Id)
                )
                    throw new AppException("CCCD đã tồn tại", HttpStatusCode.Conflict);

                _mapper.Map(req, entity);
                entity.CitizenId = newCitizen;

                await _unitOfWork.Customers.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated Customer {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Customer failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.Customers.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy khách hàng", HttpStatusCode.NotFound);

                await _unitOfWork.Customers.DeleteAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted Customer {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete Customer failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
