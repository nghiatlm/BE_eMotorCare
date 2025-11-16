

using AutoMapper;
using eMotoCare.BO.Common.src;
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

namespace eMototCare.BLL.Services.ExportServices
{
    public class ExportService : IExportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExportService> _logger;
        private readonly IMapper _mapper;
        private readonly Utils _utils;

        public ExportService(IUnitOfWork unitOfWork, ILogger<ExportService> logger, IMapper mapper, Utils utils)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utils = utils;
        }

        public async Task<PageResult<ExportNoteResponse>> GetPagedAsync(
             string? code,
             DateTime? fromDate,
             DateTime? toDate,
             ExportType? exportType,
             string? exportTo,
             int? totalQuantity,
             decimal? totalValue,
             Guid? exportById,
             Guid? serviceCenterId,
             ExportNoteStatus? exportNoteStatus,
             Guid? partItemId,
             int page,
             int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.ExportNotes.GetPagedAsync(
                code,
                fromDate,
                toDate,
                exportType,
                exportTo,
                totalQuantity,
                totalValue,
                exportById,
                serviceCenterId,
                exportNoteStatus,
                partItemId,
                page,
                pageSize
                );
                var rows = _mapper.Map<List<ExportNoteResponse>>(items);
                return new PageResult<ExportNoteResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Export Note failed: {Message}", ex.Message);
                //throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
                throw new AppException(ex.Message);
            }
        }

        public async Task<Guid> CreateAsync(ExportNoteRequest req)
        {

            try
            {
                var exportNoteId = Guid.NewGuid();

                var entity = _mapper.Map<ExportNote>(req);
                entity.Id = exportNoteId;
                entity.Code = $"EXPORT-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
                entity.ExportDate = DateTime.UtcNow;
                if (req.PartItemId != null && req.PartItemId.Any())
                {
                    foreach (var partItemId in req.PartItemId)
                    {
                        var partItem = await _unitOfWork.PartItems.GetByIdAsync(partItemId.Value);

                        if (partItem == null)
                            throw new Exception($"PartItem {partItemId} không tồn tại.");

                        if (partItem.ServiceCenterInventory.ServiceCenterId != req.ServiceCenterId)
                            throw new Exception($"PartItem {partItemId} không thuộc ServiceCenter {req.ServiceCenterId}.");

                        // Update trạng thái hoặc exportNoteId tùy nghiệp vụ
                        partItem.ExportNoteId = exportNoteId;
                        partItem.ServiceCenterInventoryId = null;

                        await _unitOfWork.PartItems.UpdateAsync(partItem);
                    }
                }
                await _unitOfWork.ExportNotes.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created Export Note");
                return entity.Id;

            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Export Note failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.ExportNotes.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy ExportNote",
                        HttpStatusCode.NotFound
                    );

                entity.ExportNoteStatus = ExportNoteStatus.CANCELLED;
                await _unitOfWork.ExportNotes.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted ExportNote {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete ExportNote failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, ExportNoteUpdateRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.ExportNotes.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy ExportNote",
                        HttpStatusCode.NotFound
                    );

                

                if (req.Code != null)
                {
                    var code = req.Code.Trim();
                    if (
                        !string.Equals(entity.Code, code, StringComparison.OrdinalIgnoreCase)
                        && await _unitOfWork.ExportNotes.ExistsCodeAsync(code)
                    )
                        throw new AppException("Code đã tồn tại", HttpStatusCode.Conflict);
                    entity.Code = code;
                }
                if (req.ExportDate != null)
                {
                    entity.ExportDate = req.ExportDate.Value;
                }
                if (req.Type != null)
                {
                    entity.Type = req.Type.Value;
                }
                if (req.ExportTo != null)
                {
                    entity.ExportTo = req.ExportTo.Trim();
                }
                if (req.TotalQuantity != null)
                {
                    entity.TotalQuantity = req.TotalQuantity.Value;
                }
                if (req.TotalValue != null)
                {
                    entity.TotalValue = req.TotalValue.Value;
                }
                if (req.Note != null)
                {
                    entity.Note = req.Note.Trim();
                }

                if (req.ExportById != null)
                {
                    entity.ExportById = req.ExportById;
                }

                if (req.ServiceCenterId != null)
                {
                    entity.ServiceCenterId = req.ServiceCenterId.Value;
                }
                if (req.ExportNoteStatus != null)
                {
                    if (req.ExportNoteStatus == ExportNoteStatus.COMPLETED && entity.Type == ExportType.REPLACEMENT)
                    {
                        var appointmentCode = entity.Note.Replace("Xuất phụ tùng cho appointment: ", "").Trim();
                        var appointment = await _unitOfWork.Appointments.GetByCodeAsync(appointmentCode);
                        var evCheck = await _unitOfWork.EVChecks.GetByIdAsync(appointment.EVCheck.Id);
                        evCheck.Status = EVCheckStatus.REPAIR_IN_PROGRESS;

                        var replaceDetails = evCheck
                                            .EVCheckDetails.Where(d => d.ReplacePartId != null)
                                            .ToList();

                    }

                    if (req.ExportNoteStatus == ExportNoteStatus.CANCELLED && entity.Type == ExportType.REPLACEMENT)
                    entity.ExportNoteStatus = req.ExportNoteStatus.Value;
                }

                await _unitOfWork.ExportNotes.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated ExportNote {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update ExportNote failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }


        }

        public async Task<ExportNoteResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.ExportNotes.GetByIdAsync(id);
                if (entity is null)
                    throw new AppException("Không tìm thấy ExportNote", HttpStatusCode.NotFound);

                return _mapper.Map<ExportNoteResponse>(entity);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById ExportNote failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<List<ExportPartItemResponse>> GetPartItemsByExportNoteIdAsync(Guid exportNoteId)
        {
            try
            {
                var exportNote = await _unitOfWork.ExportNotes.GetByIdAsync(exportNoteId);
                if (exportNote is null)
                    throw new AppException("Không tìm thấy ExportNote", HttpStatusCode.NotFound);
                var partItems = await _unitOfWork.PartItems.GetByExportNoteIdAsync(exportNoteId);
                return _mapper.Map<List<ExportPartItemResponse>>(partItems);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get PartItems by ExportNoteId failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

    }
}
