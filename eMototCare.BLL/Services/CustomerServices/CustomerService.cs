

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.Entities;
using eMotoCare.DAL;
using Microsoft.Extensions.Logging;

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

        public async Task<Guid> CreateAsync(CustomerRequest req)
        {
            var code = req.CitizenId.Trim();


            var entity = _mapper.Map<Customer>(req);
            entity.Id = Guid.NewGuid();
            entity.CitizenId = code;

            await _unitOfWork.Customers.CreateAsync(entity);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Created customer ");
            return entity.Id;
        }
    }
}
