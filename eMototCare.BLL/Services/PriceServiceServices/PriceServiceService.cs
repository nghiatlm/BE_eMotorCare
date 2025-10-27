using System.Net;
using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.PriceServiceServices
{
    public class PriceServiceService : IPriceServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PriceServiceService> _logger;

        public PriceServiceService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<PriceServiceService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<PriceServiceResponse>> GetPagedAsync(
            string? search,
            Guid? partTypeId,
            Remedies? remedies,
            DateTime? fromEffectiveDate,
            DateTime? toEffectiveDate,
            decimal? minPrice,
            decimal? maxPrice,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.PriceServices.GetPagedAsync(
                    search,
                    partTypeId,
                    remedies,
                    fromEffectiveDate,
                    toEffectiveDate,
                    minPrice,
                    maxPrice,
                    page,
                    pageSize
                );

                var rows = _mapper.Map<List<PriceServiceResponse>>(items);
                return new PageResult<PriceServiceResponse>(rows, pageSize, page, (int)total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged PriceService failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<PriceServiceResponse?> GetByIdAsync(Guid id)
        {
            var item =
                await _unitOfWork.PriceServices.GetByIdAsync(id)
                ?? throw new AppException(
                    "Không tìm thấy bảng giá dịch vụ",
                    HttpStatusCode.NotFound
                );
            return _mapper.Map<PriceServiceResponse>(item);
        }

        public async Task<Guid> CreateAsync(PriceServiceRequest req)
        {
            try
            {
                if (await _unitOfWork.PriceServices.ExistsCodeAsync(req.Code))
                    throw new AppException("Mã Code đã tồn tại", HttpStatusCode.Conflict);

                var entity = _mapper.Map<PriceService>(req);
                entity.Id = Guid.NewGuid();

                await _unitOfWork.PriceServices.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation(
                    "Created PriceService {Id} ({Code})",
                    entity.Id,
                    entity.Code
                );
                return entity.Id;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create PriceService failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, PriceServiceRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.PriceServices.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy bảng giá dịch vụ",
                        HttpStatusCode.NotFound
                    );

                if (await _unitOfWork.PriceServices.ExistsCodeAsync(req.Code, ignoreId: id))
                    throw new AppException("Mã Code đã tồn tại", HttpStatusCode.Conflict);

                _mapper.Map(req, entity);
                await _unitOfWork.PriceServices.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated PriceService {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update PriceService failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.PriceServices.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy bảng giá dịch vụ",
                        HttpStatusCode.NotFound
                    );

                await _unitOfWork.PriceServices.DeleteAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted PriceService {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete PriceService failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
