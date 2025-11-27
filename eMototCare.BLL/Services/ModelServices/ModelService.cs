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

namespace eMototCare.BLL.Services.ModelServices
{
    public class ModelService : IModelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ModelService> _logger;

        public ModelService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ModelService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<ModelResponse>> GetPagedAsync(
            string? search,
            Status? status,
            Guid? modelId,
            Guid? maintenancePlanId,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.Models.GetPagedAsync(
                    search,
                    status,
                    modelId,
                    maintenancePlanId,
                    page,
                    pageSize
                );
                if (page <= 0)
                    throw new AppException("Page phải > 0", HttpStatusCode.BadRequest);

                if (pageSize <= 0)
                    throw new AppException("PageSize phải > 0", HttpStatusCode.BadRequest);

                if (modelId.HasValue && modelId.Value == Guid.Empty)
                    throw new AppException("ModelId không hợp lệ", HttpStatusCode.BadRequest);

                if (maintenancePlanId.HasValue && maintenancePlanId.Value == Guid.Empty)
                    throw new AppException(
                        "MaintenancePlanId không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (status.HasValue && !Enum.IsDefined(typeof(Status), status.Value))
                    throw new AppException(
                        "Trạng thái model không hợp lệ",
                        HttpStatusCode.BadRequest
                    );
                var rows = _mapper.Map<List<ModelResponse>>(items);
                return new PageResult<ModelResponse>(rows, pageSize, page, (int)total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Model failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ModelResponse> GetByIdAsync(Guid id)
        {
            var model = await _unitOfWork.Models.GetByIdAsync(id);
            if (id == Guid.Empty)
                throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);

            if (model == null)
                throw new AppException("Không tìm thấy Model", HttpStatusCode.NotFound);

            return _mapper.Map<ModelResponse>(model);
        }

        public async Task<Guid> CreateAsync(ModelRequest req)
        {
            try
            {
                var plan =
                    await _unitOfWork.MaintenancePlans.GetByIdAsync(req.MaintenancePlanId)
                    ?? throw new AppException(
                        "Không tìm thấy Maintenance Plan",
                        HttpStatusCode.BadRequest
                    );

                if (await _unitOfWork.Models.ExistsNameAsync(req.Name))
                    throw new AppException("Tên Model đã tồn tại.", HttpStatusCode.Conflict);
                if (req == null)
                    throw new AppException("Request không được null", HttpStatusCode.BadRequest);

                var name = (req.Name ?? string.Empty).Trim();
                var manufacturer = (req.Manufacturer ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(name))
                    throw new AppException(
                        "Tên Model không được để trống",
                        HttpStatusCode.BadRequest
                    );

                if (string.IsNullOrWhiteSpace(manufacturer))
                    throw new AppException(
                        "Hãng sản xuất không được để trống",
                        HttpStatusCode.BadRequest
                    );

                if (req.MaintenancePlanId == Guid.Empty)
                    throw new AppException(
                        "MaintenancePlanId không hợp lệ",
                        HttpStatusCode.BadRequest
                    );
                var entity = _mapper.Map<Model>(req);
                entity.Id = Guid.NewGuid();
                entity.Code = await GenerateCodeAsync();
                entity.Status = Status.ACTIVE;

                await _unitOfWork.Models.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created Model {Code} ({Id})", entity.Code, entity.Id);
                return entity.Id;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Model failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, ModelUpdateRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.Models.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy Model", HttpStatusCode.NotFound);
                if (id == Guid.Empty)
                    throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);

                if (req == null)
                    throw new AppException("Request không được null", HttpStatusCode.BadRequest);

                if (req.MaintenancePlanId.HasValue && req.MaintenancePlanId.Value == Guid.Empty)
                    throw new AppException(
                        "MaintenancePlanId không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (req.Status.HasValue && !Enum.IsDefined(typeof(Status), req.Status.Value))
                    throw new AppException(
                        "Trạng thái model không hợp lệ",
                        HttpStatusCode.BadRequest
                    );
                if (req.MaintenancePlanId.HasValue)
                {
                    var plan =
                        await _unitOfWork.MaintenancePlans.GetByIdAsync(req.MaintenancePlanId.Value)
                        ?? throw new AppException(
                            "Không tìm thấy Maintenance Plan",
                            HttpStatusCode.BadRequest
                        );
                }

                if (
                    !string.IsNullOrWhiteSpace(req.Name)
                    && await _unitOfWork.Models.ExistsNameAsync(req.Name, id)
                )
                    throw new AppException("Tên Model đã tồn tại.", HttpStatusCode.Conflict);

                entity.Name = req.Name?.Trim() ?? entity.Name;
                entity.Manufacturer = req.Manufacturer?.Trim() ?? entity.Manufacturer;
                entity.MaintenancePlanId = req.MaintenancePlanId ?? entity.MaintenancePlanId;
                entity.Status = req.Status ?? entity.Status;

                await _unitOfWork.Models.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated Model {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Model failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.Models.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy Model", HttpStatusCode.NotFound);
                if (id == Guid.Empty)
                    throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);
                if (entity.Status == Status.IN_ACTIVE)
                {
                    throw new AppException(
                        "Model này đã bị vô hiệu hoá rồi.",
                        HttpStatusCode.Conflict
                    );
                }

                entity.Status = Status.IN_ACTIVE;

                await _unitOfWork.Models.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Soft-deleted Model {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete Model failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        private async Task<string> GenerateCodeAsync()
        {
            const string prefix = "MDL-";
            var rnd = new Random();
            int guard = 0;
            string code;
            do
            {
                var suffix = rnd.Next(0, 99999).ToString("D5");
                code = prefix + suffix;
                guard++;
            } while (await _unitOfWork.Models.ExistsCodeAsync(code) && guard < 10);

            return code;
        }
    }
}
