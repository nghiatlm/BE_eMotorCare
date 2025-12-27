
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
                if (request.StartDate < DateTime.Now.Date)
                {
                    throw new AppException("Ngày bắt đầu phải ở hiện tại hoặc tương lai", HttpStatusCode.BadRequest);
                }
                if (request.EndDate == request.StartDate)
                {
                    throw new AppException("Ngày bắt đầu và kết thúc không được bằng nhau", HttpStatusCode.BadRequest);
                }
                string code = await _unitOfWork.Programs.GenerateProgramCodeAsync(request.ProgramType);
                var program = new Program
                {
                    Code = code,
                    Name = request.Name,
                    Description = request.Description,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    ProgramType = request.ProgramType,
                    SeverityLevel = request.SeverityLevel,
                    CreatedBy = request.CreatedBy,
                    UpdatedBy = request.UpdatedBy.HasValue ? request.UpdatedBy.Value : (Guid?)null,
                    Status = Status.ACTIVE
                };
                await _unitOfWork.Programs.CreateAsync(program);
                var modelExisting = await _unitOfWork.Models.GetByIdAsync(request.ProgramDetailRequest.ModelId.Value);
                if (modelExisting == null)
                {
                    throw new AppException("Model không tồn tại", HttpStatusCode.NotFound);
                }
                var partExisting = await _unitOfWork.Parts.GetByIdAsync(request.ProgramDetailRequest.PartId.Value);
                if (partExisting == null)
                {
                    throw new AppException("Part không tồn tại", HttpStatusCode.NotFound);
                }
                var programDetail = new ProgramDetail
                {
                    ProgramId = program.Id,
                    ModelId = request.ProgramDetailRequest.ModelId,
                    PartId = request.ProgramDetailRequest.PartId,
                    ActionType = request.ProgramDetailRequest.ActionType.Value,
                    Description = request.ProgramDetailRequest.Description,
                    ManufactureYear = request.ProgramDetailRequest.ManufactureYear
                };
                await _unitOfWork.ProgramDetails.CreateAsync(programDetail);
                var result = await _unitOfWork.SaveAsync();
                return result > 0 ? true : false;
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

        public async Task<PageResult<ProgramResponse>> GetPaged(string? query, DateTime? startDate, DateTime? endDate, ProgramType? type, Status? status, Guid? modelId = null, Guid? partId = null, ActionType? actionType = null, int? manufactureYear = null, int pageCurrent = 1, int pageSize = 10)
        {
            try
            {
                var result = await _unitOfWork.Programs.FindParams(query, startDate, endDate, type, status, modelId, partId, actionType, manufactureYear, pageCurrent, pageSize);
                return _mapper.Map<PageResult<ProgramResponse>>(result);
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