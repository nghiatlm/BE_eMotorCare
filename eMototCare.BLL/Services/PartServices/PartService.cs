

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

namespace eMototCare.BLL.Services.PartServices
{
    public class PartService : IPartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PartService> _logger;

        public PartService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PartService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<PartResponse>> GetPagedAsync(
            Guid? partTypeId,
            string? code,
            string? name,
            Status? status,
            int? quantity,
            int page = 1,
            int pageSize = 10
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.Parts.GetPagedAsync(
                    partTypeId, 
                    code, 
                    name, 
                    status, 
                    quantity, 
                    page, 
                    pageSize
                );
                var rows = _mapper.Map<List<PartResponse>>(items);
                return new PageResult<PartResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Part failed: {Message}", ex.Message);
                //throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
                throw new AppException(ex.Message);
            }
        }

        public async Task<Guid> CreateAsync(PartRequest req)
        {

            try
            {
                var code = req.Code.Trim();

                if (await _unitOfWork.Parts.ExistsCodeAsync(code))
                    throw new AppException("Code đã tồn tại", HttpStatusCode.Conflict);

                var entity = _mapper.Map<Part>(req);
                entity.Id = Guid.NewGuid();
                entity.Code = code;
                entity.Status = Status.ACTIVE;

                await _unitOfWork.Parts.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created Part");
                return entity.Id;

            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Part failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.Parts.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy Part",
                        HttpStatusCode.NotFound
                    );

                entity.Status = Status.IN_ACTIVE;
                await _unitOfWork.Parts.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted Part {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete Part failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, PartUpdateRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.Parts.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy Part",
                        HttpStatusCode.NotFound
                    );

                var code = req.Code.Trim();
                if (
                    !string.Equals(entity.Code, code, StringComparison.OrdinalIgnoreCase)
                    && await _unitOfWork.Parts.ExistsCodeAsync(code)
                )
                    throw new AppException("Code đã tồn tại", HttpStatusCode.Conflict);


                _mapper.Map(req, entity);
                entity.Code = code;

                await _unitOfWork.Parts.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated Part {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Part failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }


        }

        public async Task<PartResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.Parts.GetByIdAsync(id);
                if (entity is null)
                    throw new AppException("Không tìm thấy Part", HttpStatusCode.NotFound);

                return _mapper.Map<PartResponse>(entity);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById Part failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
