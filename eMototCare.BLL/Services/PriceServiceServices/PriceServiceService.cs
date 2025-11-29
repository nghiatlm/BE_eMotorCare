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
                if (page <= 0)
                    throw new AppException("Page phải > 0", HttpStatusCode.BadRequest);

                if (pageSize <= 0)
                    throw new AppException("PageSize phải > 0", HttpStatusCode.BadRequest);

                if (partTypeId.HasValue && partTypeId.Value == Guid.Empty)
                    throw new AppException("PartTypeId không hợp lệ", HttpStatusCode.BadRequest);

                if (remedies.HasValue && !Enum.IsDefined(typeof(Remedies), remedies.Value))
                    throw new AppException(
                        "Loại dịch vụ (Remedies) không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (
                    fromEffectiveDate.HasValue
                    && toEffectiveDate.HasValue
                    && fromEffectiveDate > toEffectiveDate
                )
                {
                    throw new AppException(
                        "fromEffectiveDate không được lớn hơn toEffectiveDate",
                        HttpStatusCode.BadRequest
                    );
                }

                if (minPrice.HasValue && minPrice.Value < 0)
                    throw new AppException("minPrice không được âm", HttpStatusCode.BadRequest);

                if (maxPrice.HasValue && maxPrice.Value < 0)
                    throw new AppException("maxPrice không được âm", HttpStatusCode.BadRequest);

                if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
                    throw new AppException(
                        "minPrice không được lớn hơn maxPrice",
                        HttpStatusCode.BadRequest
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
            if (id == Guid.Empty)
                throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);
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
                var partType = await _unitOfWork.PartTypes.GetByIdAsync(req.PartTypeId);
                if (partType == null)
                {
                    throw new AppException(
                        "Loại phụ tùng không tồn tại",
                        HttpStatusCode.BadRequest
                    );
                }
                if (req == null)
                    throw new AppException("Request không được null", HttpStatusCode.BadRequest);

                if (req.PartTypeId == Guid.Empty)
                    throw new AppException("PartTypeId không hợp lệ", HttpStatusCode.BadRequest);

                if (!Enum.IsDefined(typeof(Remedies), req.Remedies))
                    throw new AppException(
                        "Loại dịch vụ (Remedies) không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                var name = (req.Name ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(name))
                    throw new AppException(
                        "Tên bảng giá không được để trống",
                        HttpStatusCode.BadRequest
                    );

                if (req.LaborCost < 0)
                    throw new AppException("LaborCost không được âm", HttpStatusCode.BadRequest);

                if (req.Price < 0)
                    throw new AppException("Price không được âm", HttpStatusCode.BadRequest);

                if (req.EffectiveDate == default)
                    throw new AppException("EffectiveDate không hợp lệ", HttpStatusCode.BadRequest);

                var existed = _unitOfWork
                    .PriceServices.FindAll()
                    .Any(x => x.PartTypeId == req.PartTypeId && x.Remedies == req.Remedies);

                if (existed)
                {
                    throw new AppException(
                        "Loại phụ tùng này đã có bảng giá cho dịch vụ này rồi",
                        HttpStatusCode.Conflict
                    );
                }

                const string prefix = "PSV-";
                var lastCode = _unitOfWork
                    .PriceServices.FindAll()
                    .Where(x => x.Code.StartsWith(prefix))
                    .OrderByDescending(x => x.Code)
                    .Select(x => x.Code)
                    .FirstOrDefault();

                int nextNumber = 1;

                if (!string.IsNullOrEmpty(lastCode) && lastCode.Length > prefix.Length)
                {
                    var numberPart = lastCode.Substring(prefix.Length);
                    if (int.TryParse(numberPart, out var parsed))
                    {
                        nextNumber = parsed + 1;
                    }
                }

                string newCode;
                do
                {
                    newCode = $"{prefix}{nextNumber:D5}";
                    nextNumber++;
                } while (await _unitOfWork.PriceServices.ExistsCodeAsync(newCode));

                var entity = _mapper.Map<PriceService>(req);
                entity.Code = newCode;
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
                if (id == Guid.Empty)
                    throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);

                if (req == null)
                    throw new AppException("Request không được null", HttpStatusCode.BadRequest);

                if (req.PartTypeId == Guid.Empty)
                    throw new AppException("PartTypeId không hợp lệ", HttpStatusCode.BadRequest);

                if (!Enum.IsDefined(typeof(Remedies), req.Remedies))
                    throw new AppException(
                        "Loại dịch vụ (Remedies) không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                var name = (req.Name ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(name))
                    throw new AppException(
                        "Tên bảng giá không được để trống",
                        HttpStatusCode.BadRequest
                    );

                if (req.LaborCost < 0)
                    throw new AppException("LaborCost không được âm", HttpStatusCode.BadRequest);

                if (req.Price < 0)
                    throw new AppException("Price không được âm", HttpStatusCode.BadRequest);

                if (req.EffectiveDate == default)
                    throw new AppException("EffectiveDate không hợp lệ", HttpStatusCode.BadRequest);

                var entity =
                    await _unitOfWork.PriceServices.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy bảng giá dịch vụ",
                        HttpStatusCode.NotFound
                    );

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
                if (id == Guid.Empty)
                    throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);
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
