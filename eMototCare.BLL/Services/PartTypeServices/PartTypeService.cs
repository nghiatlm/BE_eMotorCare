

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using eMototCare.BLL.Services.PartTypeServices;
using Microsoft.Extensions.Logging;
using System.Net;
using eMotoCare.BO.Enum;

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
            int page,
            int pageSize
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


                var entity = _mapper.Map<PartType>(req);
                entity.Id = Guid.NewGuid();
                entity.Status = Status.ACTIVE;
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

                entity.Status = Status.IN_ACTIVE;
                await _unitOfWork.PartTypes.UpdateAsync(entity);
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

        public async Task UpdateAsync(Guid id, PartTypeUpdateRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.PartTypes.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy PartType",
                        HttpStatusCode.NotFound
                    );



                _mapper.Map(req, entity);


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

        public async Task<List<eMotoCare.BO.DTO.Responses.Labels.PartType>> GetAll()
        {
            try
            {
                var item = await _unitOfWork.PartTypes.FindAllAsync();
                return _mapper.Map<List<eMotoCare.BO.DTO.Responses.Labels.PartType>>(item);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get All Part Type failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
