

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

namespace eMototCare.BLL.Services.CampaignDetailServices
{
    public class CampaignDetailService : ICampaignDetailService
    {
        private readonly IUnitOfWork _unitofWork;
        private readonly IMapper _mapper;
        private ILogger<CampaignDetailService> _logger;

        public CampaignDetailService(IUnitOfWork unitofWork, IMapper mapper, ILogger<CampaignDetailService> logger)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<CampaignDetailResponse>> GetPagedAsync(
             Guid? campaignId,
             Guid? partId,
             CampaignActionType? actionType,
             bool? isMandatory,
             int? estimatedTime,
             int page,
             int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitofWork.CampaignDetails.GetPagedAsync(
                    campaignId,
                    partId,
                    actionType,
                    isMandatory,
                    estimatedTime,
                    page,
                    pageSize
                );
                var rows = _mapper.Map<List<CampaignDetailResponse>>(items);
                return new PageResult<CampaignDetailResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Campaign Detail failed: {Message}", ex.Message);
                //throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
                throw new AppException(ex.Message);
            }

        }

        public async Task<Guid> CreateAsync(CampaignDetailRequest req)
        {

            try
            {


                var entity = _mapper.Map<CampaignDetail>(req);
                entity.Id = Guid.NewGuid();

                await _unitofWork.CampaignDetails.CreateAsync(entity);
                await _unitofWork.SaveAsync();

                _logger.LogInformation("Created Campaign Detail");
                return entity.Id;

            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Campaign Detail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitofWork.CampaignDetails.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy Campaign Detail",
                        HttpStatusCode.NotFound
                    );


                await _unitofWork.CampaignDetails.DeleteAsync(entity);
                await _unitofWork.SaveAsync();

                _logger.LogInformation("Delete Campaign Detail {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete Campaign failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, CampaignDetailUpdateRequest req)
        {
            try
            {
                var entity =
                    await _unitofWork.CampaignDetails.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy Campaign Detail",
                        HttpStatusCode.NotFound
                    );
                if (req.CampaignId
                    .HasValue)
                    entity.CampaignId = req.CampaignId.Value;
                if (!string.IsNullOrEmpty(req.Description))
                    entity.Description = req.Description;
                if (req.PartId.HasValue)
                    entity.PartId = req.PartId.Value;
                if (req.ActionType.HasValue)
                    entity.ActionType = req.ActionType.Value;
                if (!string.IsNullOrEmpty(req.Note))
                    entity.Note = req.Note;
                if (req.IsMandatory.HasValue)
                    entity.IsMandatory = req.IsMandatory.Value;
                if (req.EstimatedTime.HasValue)
                    entity.EstimatedTime = req.EstimatedTime.Value;



                await _unitofWork.CampaignDetails.UpdateAsync(entity);
                await _unitofWork.SaveAsync();

                _logger.LogInformation("Updated Campaign Detail {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Campaign Detail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }


        }

        public async Task<CampaignDetailResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitofWork.CampaignDetails.GetByIdAsync(id);
                if (entity is null)
                    throw new AppException("Không tìm thấy Campaign Detail", HttpStatusCode.NotFound);

                return _mapper.Map<CampaignDetailResponse>(entity);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById Campaign Detail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
