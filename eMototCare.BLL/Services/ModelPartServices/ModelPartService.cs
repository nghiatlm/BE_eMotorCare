using System.Net;
using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using eMototCare.BLL.Services.ModelPartTypeServices;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.ModelPartServices
{
    public class ModelPartService : IModelPartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ModelPartService> _logger;

        public ModelPartService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ModelPartService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<ModelPartResponse>> GetPagedAsync(
            string? search,
            Guid? id,
            Guid? modelId,
            Guid? partId,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.ModelParts.GetPagedAsync(
                    search,
                    modelId,
                    partId,
                    id,
                    page,
                    pageSize
                );

                var rows = _mapper.Map<List<ModelPartResponse>>(items);
                return new PageResult<ModelPartResponse>(rows, pageSize, page, (int)total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Model failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ModelPartResponse> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.ModelParts.GetByIdAsync(id);

            if (entity == null)
                throw new AppException("Không tìm thấy ModelPart", HttpStatusCode.NotFound);

            return _mapper.Map<ModelPartResponse>(entity);
        }

        public async Task<Guid> CreateAsync(ModelPartRequest req)
        {
            if (req.ModelId == Guid.Empty)
                throw new AppException("ModelId không hợp lệ", HttpStatusCode.BadRequest);

            if (req.PartId == Guid.Empty)
                throw new AppException("PartId không hợp lệ", HttpStatusCode.BadRequest);

            var model = await _unitOfWork.Models.GetByIdAsync(req.ModelId);
            if (model == null)
                throw new AppException("Không tìm thấy Model", HttpStatusCode.NotFound);

            var Part = await _unitOfWork.Parts.GetByIdAsync(req.PartId);
            if (Part == null)
                throw new AppException("Không tìm thấy Part", HttpStatusCode.NotFound);

            var existed = await _unitOfWork.ModelParts.ExistsAsync(req.ModelId, req.PartId);
            if (existed)
                throw new AppException(
                    "Model này đã được gán với Part này rồi.",
                    HttpStatusCode.Conflict
                );

            var entity = _mapper.Map<ModelPart>(req);
            entity.Id = Guid.NewGuid();

            await _unitOfWork.ModelParts.CreateAsync(entity);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation(
                "Created ModelPart {Id} (Model={ModelId}, Part={PartId})",
                entity.Id,
                entity.ModelId,
                entity.PartId
            );

            return entity.Id;
        }

        public async Task UpdateAsync(Guid id, ModelPartUpdateRequest req)
        {
            var entity =
                await _unitOfWork.ModelParts.GetByIdAsync(id)
                ?? throw new AppException("Không tìm thấy ModelPart", HttpStatusCode.NotFound);

            var newModelId = req.ModelId ?? entity.ModelId;
            var newPartId = req.PartId ?? entity.PartId;

            // validate modelId nếu có
            if (req.ModelId.HasValue)
            {
                var model = await _unitOfWork.Models.GetByIdAsync(req.ModelId.Value);
                if (model == null)
                    throw new AppException("Không tìm thấy Model", HttpStatusCode.NotFound);
            }

            // validate PartId nếu có
            if (req.PartId.HasValue)
            {
                var Part = await _unitOfWork.Parts.GetByIdAsync(req.PartId.Value);
                if (Part == null)
                    throw new AppException("Không tìm thấy Part", HttpStatusCode.NotFound);
            }

            // check trùng cặp ModelId + PartId
            var duplicated = await _unitOfWork.ModelParts.ExistsAsync(
                newModelId,
                newPartId,
                ignoreId: id
            );
            if (duplicated)
                throw new AppException(
                    "Model này đã được gán với Part này.",
                    HttpStatusCode.Conflict
                );

            entity.ModelId = newModelId;
            entity.PartId = newPartId;



            await _unitOfWork.ModelParts.UpdateAsync(entity);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Updated ModelPart {Id}", id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity =
                await _unitOfWork.ModelParts.GetByIdAsync(id)
                ?? throw new AppException("Không tìm thấy ModelPart", HttpStatusCode.NotFound);



            

            await _unitOfWork.ModelParts.DeleteAsync(entity);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Soft-deleted (IN_ACTIVE) ModelPart {Id}", id);
        }
    }
}
