

using AutoMapper;
using eMotoCare.BO.Common.src;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using Microsoft.Extensions.Logging;
using System.Net;

namespace eMototCare.BLL.Services.ImportNoteServices
{
    public class ImportNoteService : IImportNoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ImportNoteService> _logger;
        private readonly Utils _utils;

        public ImportNoteService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ImportNoteService> logger, Utils utils)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _utils = utils;
        }

        public async Task<PageResult<ImportNoteResponse>> GetPagedAsync(
             string? code,
             DateTime? fromDate,
             DateTime? toDate,
             string? importFrom,
             string? supplier,
             ImportType? importType,
             decimal? totalAmount,
             Guid? importById,
             Guid? serviceCenterId,
             ImportNoteStatus? importNoteStatus,
             int page,
             int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.ImportNotes.GetPagedAsync(
                code,
                fromDate,
                toDate,
                importFrom,
                supplier,
                importType,
                totalAmount,
                importById,
                serviceCenterId,
                importNoteStatus,
                page,
                pageSize
                );
                var rows = _mapper.Map<List<ImportNoteResponse>>(items);
                return new PageResult<ImportNoteResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged ImportNote failed: {Message}", ex.Message);
                //throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
                throw new AppException(ex.Message);
            }
        }

        public async Task<Guid> CreateAsync(ImportNoteRequest req)
        {

            try
            {

                var importNoteId = Guid.NewGuid();
                var entity = _mapper.Map<ImportNote>(req);
                entity.Id = importNoteId;
                entity.ImportDate = DateTime.UtcNow;
                entity.Code = $"IMPORT-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
                entity.ImportNoteStatus = ImportNoteStatus.APPROVE;
                if (req.PartItemId != null && req.PartItemId.Any())
                {
                    foreach (var partItemId in req.PartItemId)
                    {
                        var partItem = await _unitOfWork.PartItems.GetByIdAsync(partItemId.Value);

                        if (partItem == null)
                            throw new Exception($"PartItem {partItemId} không tồn tại.");

                        var serviceCenterInventory = await _unitOfWork.ServiceCenterInventories.GetByServiceCenterId(req.ServiceCenterId);

                        // partItem.ImportNoteId = importNoteId;
                        partItem.ServiceCenterInventoryId = serviceCenterInventory.Id;

                        await _unitOfWork.PartItems.UpdateAsync(partItem);
                    }
                }

                await _unitOfWork.ImportNotes.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created ImportNote");
                return entity.Id;

            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create ImportNote failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.ImportNotes.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy Part",
                        HttpStatusCode.NotFound
                    );

                entity.ImportNoteStatus = ImportNoteStatus.CANCELLED;
                await _unitOfWork.ImportNotes.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted ImportNote {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete ImportNote failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, ImportNoteUpdateRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.ImportNotes.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy ImportNotes",
                        HttpStatusCode.NotFound
                    );

                var code = req.Code.Trim();
                if (
                    !string.Equals(entity.Code, code, StringComparison.OrdinalIgnoreCase)
                    && await _unitOfWork.ImportNotes.ExistsCodeAsync(code)
                )
                    throw new AppException("Code đã tồn tại", HttpStatusCode.Conflict);


                _mapper.Map(req, entity);
                entity.Code = code;

                await _unitOfWork.ImportNotes.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated ImportNote {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update ImportNote failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }


        }

        public async Task<ImportNoteResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.ImportNotes.GetByIdAsync(id);
                if (entity is null)
                    throw new AppException("Không tìm thấy ImportNote", HttpStatusCode.NotFound);

                return _mapper.Map<ImportNoteResponse>(entity);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById ImportNote failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
