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

namespace eMototCare.BLL.Services.ModelPartTypeServices
{
    public class ModelPartTypeService : IModelPartTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ModelPartTypeService> _logger;

        public ModelPartTypeService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ModelPartTypeService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<ModelPartTypeResponse>> GetPagedAsync(
            string? search,
            Status? status,
            Guid? id,
            Guid? modelId,
            Guid? partTypeId,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.ModelPartTypes.GetPagedAsync(
                    search,
                    id,
                    modelId,
                    partTypeId,
                    status,
                    page,
                    pageSize
                );

                var rows = _mapper.Map<List<ModelPartTypeResponse>>(items);
                return new PageResult<ModelPartTypeResponse>(rows, pageSize, page, (int)total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Model failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ModelPartTypeResponse> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.ModelPartTypes.GetByIdAsync(id);

            if (entity == null)
                throw new AppException("Không tìm thấy ModelPartType", HttpStatusCode.NotFound);

            return _mapper.Map<ModelPartTypeResponse>(entity);
        }

        public async Task<Guid> CreateAsync(ModelPartTypeRequest req)
        {
            if (req.ModelId == Guid.Empty)
                throw new AppException("ModelId không hợp lệ", HttpStatusCode.BadRequest);

            if (req.PartTypeId == Guid.Empty)
                throw new AppException("PartTypeId không hợp lệ", HttpStatusCode.BadRequest);

            var model = await _unitOfWork.Models.GetByIdAsync(req.ModelId);
            if (model == null)
                throw new AppException("Không tìm thấy Model", HttpStatusCode.NotFound);

            var partType = await _unitOfWork.PartTypes.GetByIdAsync(req.PartTypeId);
            if (partType == null)
                throw new AppException("Không tìm thấy PartType", HttpStatusCode.NotFound);

            var existed = await _unitOfWork.ModelPartTypes.ExistsAsync(req.ModelId, req.PartTypeId);
            if (existed)
                throw new AppException(
                    "Model này đã được gán với PartType này rồi.",
                    HttpStatusCode.Conflict
                );

            var entity = _mapper.Map<ModelPartType>(req);
            entity.Id = Guid.NewGuid();
            entity.Status = Status.ACTIVE;

            await _unitOfWork.ModelPartTypes.CreateAsync(entity);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation(
                "Created ModelPartType {Id} (Model={ModelId}, PartType={PartTypeId})",
                entity.Id,
                entity.ModelId,
                entity.PartTypeId
            );

            return entity.Id;
        }

        public async Task UpdateAsync(Guid id, ModelPartTypeUpdateRequest req)
        {
            var entity =
                await _unitOfWork.ModelPartTypes.GetByIdAsync(id)
                ?? throw new AppException("Không tìm thấy ModelPartType", HttpStatusCode.NotFound);

            var newModelId = req.ModelId ?? entity.ModelId;
            var newPartTypeId = req.PartTypeId ?? entity.PartTypeId;

            // validate modelId nếu có
            if (req.ModelId.HasValue)
            {
                var model = await _unitOfWork.Models.GetByIdAsync(req.ModelId.Value);
                if (model == null)
                    throw new AppException("Không tìm thấy Model", HttpStatusCode.NotFound);
            }

            // validate partTypeId nếu có
            if (req.PartTypeId.HasValue)
            {
                var partType = await _unitOfWork.PartTypes.GetByIdAsync(req.PartTypeId.Value);
                if (partType == null)
                    throw new AppException("Không tìm thấy PartType", HttpStatusCode.NotFound);
            }

            // check trùng cặp ModelId + PartTypeId
            var duplicated = await _unitOfWork.ModelPartTypes.ExistsAsync(
                newModelId,
                newPartTypeId,
                ignoreId: id
            );
            if (duplicated)
                throw new AppException(
                    "Model này đã được gán với PartType này.",
                    HttpStatusCode.Conflict
                );

            entity.ModelId = newModelId;
            entity.PartTypeId = newPartTypeId;

            if (req.Status.HasValue)
                entity.Status = req.Status.Value;

            await _unitOfWork.ModelPartTypes.UpdateAsync(entity);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Updated ModelPartType {Id}", id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity =
                await _unitOfWork.ModelPartTypes.GetByIdAsync(id)
                ?? throw new AppException("Không tìm thấy ModelPartType", HttpStatusCode.NotFound);

            if (entity.Status == Status.IN_ACTIVE)
                throw new AppException(
                    "ModelPartType này đã bị vô hiệu hoá trước đó.",
                    HttpStatusCode.Conflict
                );

            entity.Status = Status.IN_ACTIVE;

            await _unitOfWork.ModelPartTypes.UpdateAsync(entity);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Soft-deleted (IN_ACTIVE) ModelPartType {Id}", id);
        }
    }
}
