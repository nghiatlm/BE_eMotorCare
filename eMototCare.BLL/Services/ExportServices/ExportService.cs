﻿

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

namespace eMototCare.BLL.Services.ExportServices
{
    public class ExportService : IExportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExportService> _logger;
        private readonly IMapper _mapper;

        public ExportService(IUnitOfWork unitOfWork, ILogger<ExportService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
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
                var code = req.Code.Trim();

                if (await _unitOfWork.ExportNotes.ExistsCodeAsync(code))
                    throw new AppException("Code đã tồn tại", HttpStatusCode.Conflict);

                var entity = _mapper.Map<ExportNote>(req);
                entity.Id = Guid.NewGuid();
                entity.Code = code;
                entity.ExportNoteStatus = ExportNoteStatus.PENDING;
                entity.ExportDate = DateTime.UtcNow;

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

                var code = req.Code.Trim();
                if (
                    !string.Equals(entity.Code, code, StringComparison.OrdinalIgnoreCase)
                    && await _unitOfWork.ExportNotes.ExistsCodeAsync(code)
                )
                    throw new AppException("Code đã tồn tại", HttpStatusCode.Conflict);


                _mapper.Map(req, entity);
                entity.Code = code;

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
    }
}
