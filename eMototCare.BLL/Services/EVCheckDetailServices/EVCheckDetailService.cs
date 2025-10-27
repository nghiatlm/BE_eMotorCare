

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

namespace eMototCare.BLL.Services.EVCheckDetailServices
{
    public class EVCheckDetailService : IEVCheckDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EVCheckDetailService> _logger;

        public EVCheckDetailService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EVCheckDetailService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<EVCheckDetailResponse>> GetPagedAsync(
             Guid? maintenanceStageDetailId,
             Guid? campaignDetailId,
             Guid? partItemId,
             Guid? eVCheckId,
             Guid? replacePartId,
             string? result,
             Remedies? remedies,
             string? unit,
             decimal? quantity,
             decimal? pricePart,
             decimal? priceService,
             decimal? totalAmount,
             EVCheckDetailStatus? status,
             int page = 1,
             int pageSize = 10
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.EVCheckDetails.GetPagedAsync(
                                                                maintenanceStageDetailId,
                                                                campaignDetailId,
                                                                partItemId,
                                                                eVCheckId,
                                                                replacePartId,
                                                                result,
                                                                remedies,
                                                                unit,
                                                                quantity,
                                                                pricePart,
                                                                priceService,
                                                                totalAmount,
                                                                status,
                                                                page,
                                                                pageSize
                );
                var rows = _mapper.Map<List<EVCheckDetailResponse>>(items);
                return new PageResult<EVCheckDetailResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged EVCheckDetail failed: {Message}", ex.Message);
                //throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
                throw new AppException(ex.Message);
            }
        }

        public async Task<Guid> CreateAsync(EVCheckDetailRequest req)
        {

            try
            {


                var entity = _mapper.Map<EVCheckDetail>(req);
                entity.Id = Guid.NewGuid();

                await _unitOfWork.EVCheckDetails.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created EVCheckDetail");
                return entity.Id;

            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create EVCheckDetail failed: {Message}", ex.Message);
                throw new AppException(ex.InnerException.Message);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.EVCheckDetails.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy EVCheckDetail",
                        HttpStatusCode.NotFound
                    );

                await _unitOfWork.EVCheckDetails.DeleteAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted EVCheckDetail {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete EVCheckDetail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, EVCheckDetailRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.EVCheckDetails.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy EVCheckDetail",
                        HttpStatusCode.NotFound
                    );



                _mapper.Map(req, entity);


                await _unitOfWork.EVCheckDetails.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated EVCheckDetail {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update EVCheckDetail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }


        }

        public async Task<EVCheckDetailResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.EVCheckDetails.GetByIdAsync(id);
                if (entity is null)
                    throw new AppException("Không tìm thấy EVCheckDetail", HttpStatusCode.NotFound);

                return _mapper.Map<EVCheckDetailResponse>(entity);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById EVCheckDetail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
