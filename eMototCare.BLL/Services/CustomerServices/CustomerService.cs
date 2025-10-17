

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace eMototCare.BLL.Services.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CustomerService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<CustomerResponse>> GetPagedAsync(
            string? firstName,
            string? lastName,
            string? address,
            string? citizenId,
            Guid? accountId,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.Customers.GetPagedAsync(
                    firstName,
                    lastName,
                    address,
                    citizenId,
                    accountId,
                    page,
                    pageSize
                );
                var rows = _mapper.Map<List<CustomerResponse>>(items);
                return new PageResult<CustomerResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Customer failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }


        public async Task<Guid> CreateAsync(CustomerRequest req)
        {
            
            try
            {
                var code = req.CitizenId.Trim();

                if (await _unitOfWork.Customers.ExistsCitizenAsync(code))
                    throw new AppException("Citizen Id đã tồn tại", HttpStatusCode.Conflict);

                var entity = _mapper.Map<Customer>(req);
                entity.Id = Guid.NewGuid();
                entity.CitizenId = code;

                await _unitOfWork.Customers.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created customer ");
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

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.Customers.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy Customer",
                        HttpStatusCode.NotFound
                    );

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

        public async Task UpdateAsync(Guid id, CustomerRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.Customers.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy Customers",
                        HttpStatusCode.NotFound
                    );

                var newCitizenId = req.CitizenId.Trim();


                if (
                    !string.Equals(entity.CitizenId, newCitizenId, StringComparison.OrdinalIgnoreCase)
                    && await _unitOfWork.Customers.ExistsCitizenAsync(newCitizenId)
                )
                    throw new AppException("Mã Citizen đã tồn tại", HttpStatusCode.Conflict);


                _mapper.Map(req, entity);
                entity.CitizenId = newCitizenId;


                await _unitOfWork.Customers.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated Customers {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Customers failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }


        }

        public async Task<CustomerResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(id);
                if (customer is null)
                    throw new AppException("Không tìm thấy Customers", HttpStatusCode.NotFound);

                return _mapper.Map<CustomerResponse>(customer);
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
    }
}
