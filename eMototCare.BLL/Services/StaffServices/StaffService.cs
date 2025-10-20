using System.Net;
using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.StaffServices
{
    public class StaffService : IStaffService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<StaffService> _logger;

        public StaffService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<StaffService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<StaffResponse>> GetPagedAsync(
            string? search,
            PositionEnum? position,
            Guid? serviceCenterId,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.Staffs.GetPagedAsync(
                    search,
                    position,
                    serviceCenterId,
                    page,
                    pageSize
                );
                var rows = _mapper.Map<List<StaffResponse>>(items);
                return new PageResult<StaffResponse>(rows, pageSize, page, (int)total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Staff failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<StaffResponse?> GetByIdAsync(Guid id)
        {
            var staff =
                await _unitOfWork.Staffs.GetByIdAsync(id)
                ?? throw new AppException("Không tìm thấy nhân viên", HttpStatusCode.NotFound);

            return _mapper.Map<StaffResponse>(staff);
        }

        public async Task<Guid> CreateAsync(StaffRequest req)
        {
            try
            {
                var code = req.StaffCode.Trim();
                var citizen = req.CitizenId.Trim();

                if (await _unitOfWork.Staffs.ExistsCodeAsync(code))
                    throw new AppException("Mã nhân viên đã tồn tại", HttpStatusCode.Conflict);

                if (await _unitOfWork.Staffs.ExistsCitizenAsync(citizen))
                    throw new AppException("CCCD đã tồn tại", HttpStatusCode.Conflict);

                var entity = _mapper.Map<Staff>(req);
                entity.Id = Guid.NewGuid();
                entity.StaffCode = code;
                entity.CitizenId = citizen;
                entity.ServiceCenterId = req.ServiceCenterId;
                await _unitOfWork.Staffs.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created Staff {Code} ({Id})", entity.StaffCode, entity.Id);
                return entity.Id;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Staff failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, StaffRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.Staffs.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy nhân viên", HttpStatusCode.NotFound);

                var newCode = req.StaffCode.Trim();
                var newCitizen = req.CitizenId.Trim();

                if (
                    !string.Equals(entity.StaffCode, newCode, StringComparison.OrdinalIgnoreCase)
                    && await _unitOfWork.Staffs.ExistsCodeAsync(newCode)
                )
                    throw new AppException("Mã nhân viên đã tồn tại", HttpStatusCode.Conflict);

                if (
                    !string.Equals(entity.CitizenId, newCitizen, StringComparison.OrdinalIgnoreCase)
                    && await _unitOfWork.Staffs.ExistsCitizenAsync(newCitizen)
                )
                    throw new AppException("CCCD đã tồn tại", HttpStatusCode.Conflict);

                _mapper.Map(req, entity);
                entity.StaffCode = newCode;
                entity.CitizenId = newCitizen;

                await _unitOfWork.Staffs.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated Staff {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Staff failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.Staffs.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy nhân viên", HttpStatusCode.NotFound);

                await _unitOfWork.Staffs.DeleteAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted Staff {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete Staff failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
