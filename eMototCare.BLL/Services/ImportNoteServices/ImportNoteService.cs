﻿

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace eMototCare.BLL.Services.ImportNoteServices
{
    public class ImportNoteService : IImportNoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ImportNoteService> _logger;
        public ImportNoteService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ImportNoteService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
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
                var code = req.Code.Trim();

                if (await _unitOfWork.ImportNotes.ExistsCodeAsync(code))
                    throw new AppException("Code đã tồn tại", HttpStatusCode.Conflict);

                var entity = _mapper.Map<ImportNote>(req);
                entity.Id = Guid.NewGuid();
                entity.Code = code;
                entity.ImportNoteStatus = ImportNoteStatus.RECEIVING;

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
