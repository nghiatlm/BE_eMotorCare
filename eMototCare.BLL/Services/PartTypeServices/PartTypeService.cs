

using AutoMapper;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using eMototCare.BLL.Services.PartTypeServices;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.PartServices
{
    public class PartTypeService : IPartTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PartTypeService> _logger;

        public PartTypeService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PartTypeService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<PartTypeResponse>> GetPagedAsync(
            
            string? name,
            string? description,
            int page = 1,
            int pageSize = 10
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.PartTypes.GetPagedAsync(
                    name,
                    description,
                    page,
                    pageSize
                );
                var rows = _mapper.Map<List<PartTypeResponse>>(items);
                return new PageResult<PartTypeResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged PartType failed: {Message}", ex.Message);
                //throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
                throw new AppException(ex.Message);
            }
        }
    }
}
