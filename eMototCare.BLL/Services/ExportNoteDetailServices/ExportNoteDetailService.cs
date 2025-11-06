

using AutoMapper;
using eMotoCare.BO.Common.src;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using Microsoft.Extensions.Logging;
using System.Net;

namespace eMototCare.BLL.Services.ExportNoteDetailServices
{
    public class ExportNoteDetailService : IExportNoteDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ExportNoteDetailService> _logger;

        public ExportNoteDetailService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ExportNoteDetailService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<ExportNoteDetailResponse>> GetPagedAsync(
            Guid? exportNoteId,
            Guid? partItemId,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.ExportNoteDetails.GetPagedAsync(
                    exportNoteId,
                    partItemId,
                    page,
                    pageSize
                );
                var rows = _mapper.Map<List<ExportNoteDetailResponse>>(items);
                return new PageResult<ExportNoteDetailResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Export Note Detail failed: {Message}", ex.Message);
                //throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
                throw new AppException(ex.Message);
            }
        }

        public async Task<Guid> CreateAsync(ExportNoteDetailRequest req)
        {

            try
            {

                var entity = _mapper.Map<ExportNoteDetail>(req);
                entity.Id = Guid.NewGuid();

                await _unitOfWork.ExportNoteDetails.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created Export Note Detail");
                return entity.Id;

            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Export Note Detail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.ExportNoteDetails.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy Export Note Detail",
                        HttpStatusCode.NotFound
                    );

                await _unitOfWork.ExportNoteDetails.DeleteAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted Export Note Detail {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete Export Note Detail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, ExportNoteDetailRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.ExportNoteDetails.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy Export Note Detail",
                        HttpStatusCode.NotFound
                    );
                        
                _mapper.Map(req, entity);

                await _unitOfWork.ExportNoteDetails.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated Export Note Detail {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Export Note Detail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }


        }

        public async Task<ExportNoteDetailResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.ExportNoteDetails.GetByIdAsync(id);
                if (entity is null)
                    throw new AppException("Không tìm thấy ExportNoteDetail", HttpStatusCode.NotFound);

                return _mapper.Map<ExportNoteDetailResponse>(entity);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById ExportNoteDetail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
