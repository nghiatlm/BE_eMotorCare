

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using eMototCare.BLL.Services.PartTypeServices;
using Microsoft.Extensions.Logging;
using System.Net;

namespace eMototCare.BLL.Services.PartServices
{
    public class PartTypeService : IPartTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PartTypeService> _logger;

        public PartTypeService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PartTypeService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<PartTypeResponse>> GetPagedAsync(
            
            string? name,
            string? description,
            int page = 1,
            int pageSize = 10
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.PartTypes.GetPagedAsync(
                    name,
                    description,
                    page,
                    pageSize
                );
                var rows = _mapper.Map<List<PartTypeResponse>>(items);
                return new PageResult<PartTypeResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged PartType failed: {Message}", ex.Message);
                //throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
                throw new AppException(ex.Message);
            }
        }

        public async Task<Guid> CreateAsync(PartTypeRequest req)
        {

            try
            {
                var name = req.Name.Trim();

                if (await _unitOfWork.PartTypes.ExistsNameAsync(name))
                    throw new AppException("Tên đã tồn tại", HttpStatusCode.Conflict);

                var entity = _mapper.Map<PartType>(req);
                entity.Id = Guid.NewGuid();
                entity.Name = name;

                await _unitOfWork.PartTypes.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created Part Type");
                return entity.Id;

            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Part Type failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.PartTypes.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy PartType",
                        HttpStatusCode.NotFound
                    );

                await _unitOfWork.PartTypes.DeleteAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted PartType {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete PartType failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, PartTypeRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.PartTypes.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy PartType",
                        HttpStatusCode.NotFound
                    );

                var name = req.Name.Trim();

                if (
                    !string.Equals(entity.Name, name, StringComparison.OrdinalIgnoreCase)
                    && await _unitOfWork.PartTypes.ExistsNameAsync(name)
                )
                    throw new AppException("Tên đã tồn tại", HttpStatusCode.Conflict);

                _mapper.Map(req, entity);
                entity.Name = name;


                await _unitOfWork.PartTypes.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated PartType {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update PartType failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }


        }

        public async Task<PartTypeResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.PartTypes.GetByIdAsync(id);
                if (entity is null)
                    throw new AppException("Không tìm thấy PartType", HttpStatusCode.NotFound);

                return _mapper.Map<PartTypeResponse>(entity);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById Part Type failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
