
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
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.ProgramService
{
    public class ProgramService : IProgramService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProgramService> _logger;

        public ProgramService(IUnitOfWork unitOfWork, ILogger<ProgramService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> Create(ProgramRequest request)
        {
            try
            {
                var existing = await _unitOfWork.Programs.ExistsTitleAsync(request.Title);
                if (existing) throw new AppException("Program title already exists", HttpStatusCode.BadRequest);
                if (request.StartDate.Date < DateTime.UtcNow.Date) throw new AppException("Start date must be today or a future date.", HttpStatusCode.BadRequest);
                var program = new Program
                {
                    Id = Guid.NewGuid(),
                    Type = request.Type,
                    Title = request.Title,
                    Description = request.Description,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    AttachmentUrl = request.AttachmentUrl,
                    CreatedBy = request.CreatedBy,
                    UpdatedBy = request.UpdatedBy,
                    Status = Status.ACTIVE
                };
                if (request.ProgramDetails != null && request.ProgramDetails.Any())
                {
                    program.ProgramDetails = request.ProgramDetails.Select(d => new ProgramDetail
                    {
                        Id = Guid.NewGuid(),
                        RecallPartId = d.RecallPartId,
                        ServiceType = d.ServiceType,
                        DiscountPercent = d.DiscountPercent,
                        BonusAmount = d.BonusAmount,
                        RecallAction = d.RecallAction,
                        CreatedAt = DateTime.UtcNow
                    }).ToList();
                }
                if (request.VehicleModels != null && request.VehicleModels.Any())
                {
                    var modelIds = request.VehicleModels.Select(vm => vm.VehicleModelId).Distinct().ToList();
                    var missing = new List<Guid>();
                    foreach (var id in modelIds)
                    {
                        var model = await _unitOfWork.Models.GetByIdAsync(id);
                        if (model == null) missing.Add(id);
                    }
                    if (missing.Any()) throw new AppException($"Vehicle model(s) not found: {string.Join(", ", missing)}", HttpStatusCode.BadRequest);
                    program.ProgramModels = modelIds.Select(id => new ProgramModel
                    {
                        ProgramId = program.Id,
                        VehicleModelId = id
                    }).ToList();
                }
                _unitOfWork.Programs.Create(program);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Account failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ProgramDetailResponse?> GetById(Guid id)
        {
            try
            {
                var existing = await _unitOfWork.Programs.FindById(id);
                if (existing == null) throw new AppException("Program not found", HttpStatusCode.NotFound);
                return _mapper.Map<ProgramDetailResponse>(existing);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Account failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<PageResult<ProgramResponse>> GetPaged(string? query, DateTime? startDate, DateTime? endDate, ProgramType? type, Status? status, Guid? modelId, int pageCurrent = 1, int pageSize = 10)
        {
            try
            {
                var pagedResult = await _unitOfWork.Programs.FindParams(query, startDate, endDate, type, status, modelId, pageCurrent, pageSize);
                var mappedItems = _mapper.Map<List<ProgramResponse>>(pagedResult.RowDatas ?? new List<Program>());
                return new PageResult<ProgramResponse>(mappedItems, pagedResult.PageSize, pagedResult.PageCurrent, pagedResult.Total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Account failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}