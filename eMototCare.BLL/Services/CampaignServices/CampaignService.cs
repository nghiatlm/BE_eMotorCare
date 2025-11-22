using System.Net;
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

namespace eMototCare.BLL.Services.CampaignServices
{
    public class CampaignService : ICampaignService
    {
        private readonly IUnitOfWork _unitofWork;
        private readonly IMapper _mapper;
        private ILogger<CampaignService> _logger;

        public CampaignService(
            IUnitOfWork unitofWork,
            IMapper mapper,
            ILogger<CampaignService> logger
        )
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<CampaignResponse>> GetPagedAsync(
            string? code,
            string? name,
            CampaignType? campaignType,
            DateTime? fromDate,
            DateTime? toDate,
            CampaignStatus? status,
            string? modelName,
            Guid? vehicleId,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitofWork.Campaigns.GetPagedAsync(
                    code,
                    name,
                    campaignType,
                    fromDate,
                    toDate,
                    status,
                    modelName,
                    vehicleId,
                    page,
                    pageSize
                );
                var rows = _mapper.Map<List<CampaignResponse>>(items);
                return new PageResult<CampaignResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Campaign failed: {Message}", ex.Message);
                //throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
                throw new AppException(ex.Message);
            }
        }

        public async Task<Guid> CreateAsync(CampaignRequest req)
        {
            try
            {
                if (req.StartDate <= DateTime.UtcNow.Date)
                    throw new ArgumentException("Ngày bắt đầu phải sau ngày hôm nay.");
                if (req.EndDate <= req.StartDate)
                    throw new ArgumentException("Ngày kết thúc phải sau ngày bắt đầu.");

                var entity = _mapper.Map<Campaign>(req);
                entity.Id = Guid.NewGuid();
                entity.Code = $"CAMP-{entity.StartDate:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
                entity.Status = CampaignStatus.ACTIVE;

                await _unitofWork.Campaigns.CreateAsync(entity);
                await _unitofWork.SaveAsync();

                _logger.LogInformation("Created Campaign");
                return entity.Id;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Campaign failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitofWork.Campaigns.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy Campaign", HttpStatusCode.NotFound);

                entity.Status = CampaignStatus.CANCELED;
                await _unitofWork.Campaigns.UpdateAsync(entity);
                await _unitofWork.SaveAsync();

                _logger.LogInformation("Canceled Campaign {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cancle Campaign failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, CampaignUpdateRequest req)
        {
            try
            {
                var entity =
                    await _unitofWork.Campaigns.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy Campaign", HttpStatusCode.NotFound);

                if (req.Code != null)
                {
                    var code = req.Code.Trim();
                    if (
                        !string.Equals(entity.Code, code, StringComparison.OrdinalIgnoreCase)
                        && await _unitofWork.Campaigns.ExistsCodeAsync(code)
                    )
                        throw new AppException("Code đã tồn tại", HttpStatusCode.Conflict);
                    entity.Code = code;
                }

                if (req.Name != null)
                    entity.Name = req.Name;

                if (req.Description != null)
                    entity.Description = req.Description;

                if (req.Type != null)
                    entity.Type = req.Type.Value;

                if (req.Status != null)
                    entity.Status = req.Status.Value;

                if (req.StartDate.HasValue && !req.EndDate.HasValue)
                {
                    if (req.StartDate.Value.Date <= DateTime.Today)
                        throw new AppException(
                            "Ngày bắt đầu phải sau hôm nay.",
                            HttpStatusCode.BadRequest
                        );

                    if (req.StartDate.Value >= entity.EndDate)
                        throw new AppException(
                            "Ngày bắt đầu phải trước ngày kết thúc hiện tại.",
                            HttpStatusCode.BadRequest
                        );

                    entity.StartDate = req.StartDate.Value;
                }

                if (!req.StartDate.HasValue && req.EndDate.HasValue)
                {
                    if (req.EndDate.Value <= entity.StartDate)
                        throw new AppException(
                            "Ngày kết thúc phải sau ngày bắt đầu hiện tại.",
                            HttpStatusCode.BadRequest
                        );

                    entity.EndDate = req.EndDate.Value;
                }

                if (req.StartDate.HasValue && req.EndDate.HasValue)
                {
                    if (req.StartDate.Value.Date <= DateTime.Today)
                        throw new AppException(
                            "Ngày bắt đầu phải sau hôm nay.",
                            HttpStatusCode.BadRequest
                        );

                    if (req.StartDate.Value >= req.EndDate.Value)
                        throw new AppException(
                            "Ngày bắt đầu phải trước ngày kết thúc.",
                            HttpStatusCode.BadRequest
                        );

                    entity.StartDate = req.StartDate.Value;
                    entity.EndDate = req.EndDate.Value;
                }

                await _unitofWork.Campaigns.UpdateAsync(entity);
                await _unitofWork.SaveAsync();

                _logger.LogInformation("Updated Campaign {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Campaign failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<CampaignResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitofWork.Campaigns.GetByIdAsync(id);
                if (entity is null)
                    throw new AppException("Không tìm thấy Campaign", HttpStatusCode.NotFound);

                return _mapper.Map<CampaignResponse>(entity);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById Campaign failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
