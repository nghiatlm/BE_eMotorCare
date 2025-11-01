﻿


using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using eMotoCare.BO.Enum;
using Microsoft.Extensions.Logging;
using System.Net;

namespace eMototCare.BLL.Services.EVCheckServices
{
    public class EVCheckService : IEVCheckService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EVCheckService> _logger;

        public EVCheckService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EVCheckService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<EVCheckResponse>> GetPagedAsync(
            DateTime? startDate,
            DateTime? endDate,
            EVCheckStatus? status,
            Guid? appointmentId,
            Guid? taskExecutorId,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.EVChecks.GetPagedAsync(
                    startDate,
                    endDate,
                    status,
                    appointmentId,
                    taskExecutorId,
                    page,
                    pageSize
                );
                var rows = _mapper.Map<List<EVCheckResponse>>(items);
                return new PageResult<EVCheckResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged EVCheck failed: {Message}", ex.Message);
                //throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
                throw new AppException(ex.Message);
            }
        }
        public async Task<Guid> CreateAsync(EVCheckRequest req)
        {

            try
            {

                var entity = _mapper.Map<EVCheck>(req);
                entity.Id = Guid.NewGuid();
                entity.Status = EVCheckStatus.IN_PROGRESS;
                entity.CheckDate = DateTime.UtcNow;
                await _unitOfWork.EVChecks.CreateAsync(entity);

                var appointment = await _unitOfWork.Appointments.GetByIdAsync(req.AppointmentId);
                if (appointment == null)
                    throw new AppException("Appointment not found", HttpStatusCode.NotFound);
                if (appointment.Type == ServiceType.MAINTENANCE_TYPE)
                {
                    var vehicleStages = appointment.VehicleStage;
                    var vehicleDetail = await _unitOfWork.Vehicles.GetByIdAsync(vehicleStages.VehicleId);
                    var allVehiclePartItems = await _unitOfWork.VehiclePartItems.GetListByVehicleIdAsync(vehicleDetail.Id);
                    var latestVehiclePartItems = allVehiclePartItems
                                                .GroupBy(vpi => vpi.PartItem.PartId)
                                                .Select(g => g.OrderByDescending(x => x.InstallDate).First())
                                                .ToList();
                    var maintenanceStageDetails = await _unitOfWork.MaintenanceStageDetails.GetByMaintenanceStageIdAsync(vehicleStages.MaintenanceStageId);
                    foreach (var detail in maintenanceStageDetails)
                    {
                        // Tìm VehiclePartItem tương thích với PartId trong MaintenanceStageDetail
                        var matchedVehiclePartItem = latestVehiclePartItems
                            .FirstOrDefault(vpi => vpi.PartItem.PartId == detail.PartId);

                        var evCheckDetail = new EVCheckDetail
                        {
                            Id = Guid.NewGuid(),
                            EVCheckId = entity.Id,
                            MaintenanceStageDetailId = detail.Id,
                            Remedies = Remedies.NONE,
                            PartItemId = matchedVehiclePartItem.PartItemId,
                            Status = EVCheckDetailStatus.IN_PROGRESS,
                        };

                        await _unitOfWork.EVCheckDetails.CreateAsync(evCheckDetail);
                    }
                }

                
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created EVCheck");
                return entity.Id;

            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create EVCheck failed: {Message}", ex.Message);
                throw new AppException(ex.InnerException.Message);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.EVChecks.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy EVCheck",
                        HttpStatusCode.NotFound
                    );

                entity.Status = EVCheckStatus.CANCELLED;
                await _unitOfWork.EVChecks.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted EVCheck {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete EVCheck failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, EVCheckUpdateRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.EVChecks.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy EVCheck",
                        HttpStatusCode.NotFound
                    );

                if (req.CheckDate != null)
                    entity.CheckDate = req.CheckDate.Value;
                if (req.TotalAmout != null)
                    entity.TotalAmout = req.TotalAmout.Value;
                if (req.Status != null)
                    entity.Status = req.Status.Value;
                if (req.AppointmentId != null)
                    entity.AppointmentId = req.AppointmentId.Value;
                if (req.TaskExecutorId != null)
                    entity.TaskExecutorId = req.TaskExecutorId.Value;
                if (req.Odometer != null)
                    entity.Odometer = req.Odometer.Value;


                if (req.Odometer != null)
                {
                    var appointment = await _unitOfWork.Appointments.GetByIdAsync(entity.AppointmentId);
                    if (appointment?.VehicleStage == null)
                        throw new AppException("VehicleStage not found", HttpStatusCode.NotFound);

                    var vehicleStage = appointment.VehicleStage;

                    var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(vehicleStage.VehicleId);
                    if (vehicle == null)
                        throw new AppException("Vehicle not found", HttpStatusCode.NotFound);

                    var model = vehicle.Model;

                    var plan = await _unitOfWork.MaintenancePlans.GetByIdAsync(model.MaintenancePlanId);
                    if (plan == null)
                        throw new AppException("Maintenance plan not found", HttpStatusCode.NotFound);

                    var maintenanceStages = await _unitOfWork.MaintenanceStages.GetListByPlanIdAsync(plan.Id);
                    if (!maintenanceStages.Any())
                        throw new AppException("No maintenance stages found", HttpStatusCode.NotFound);


                    var allVehicleStages = await _unitOfWork.VehicleStages.GetByVehicleIdAsync(vehicle.Id);
                    if (!allVehicleStages.Any())
                        throw new AppException("No vehicle stages found", HttpStatusCode.NotFound);

                    var matchedStage = maintenanceStages
                                        .Where(vs => (int)vs.Mileage <= entity.Odometer)
                                        .OrderByDescending(vs => (int)vs.Mileage)
                                        .FirstOrDefault();

                    if (matchedStage == null)
                        throw new AppException("No matching maintenance stage for Odo", HttpStatusCode.NotFound);

                    var nextStage = maintenanceStages
                                        .Where(vs => (int)vs.Mileage > entity.Odometer)
                                        .OrderBy(vs => (int)vs.Mileage)
                                        .FirstOrDefault();

                    foreach (var vs in allVehicleStages)
                    {
                        var stage = maintenanceStages.FirstOrDefault(ms => ms.Id == vs.MaintenanceStageId);
                        if (stage == null)
                            continue;

                        if ((int)stage.Mileage <= entity.Odometer)
                        {
                            if (vs.Status != VehicleStageStatus.COMPLETED)
                                vs.Status = VehicleStageStatus.EXPIRED;
                        }
                        else if (nextStage != null && stage.Id == nextStage.Id)
                        {
                            vs.Status = VehicleStageStatus.UPCOMING;
                        }
                        else
                        {
                            vs.Status = VehicleStageStatus.NO_START;
                        }

                        _unitOfWork.VehicleStages.Update(vs);
                    }
                }
                await _unitOfWork.EVChecks.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated EVCheck {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update EVCheck failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }


        }

        public async Task<EVCheckResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.EVChecks.GetByIdAsync(id);
                if (entity is null)
                    throw new AppException("Không tìm thấy EVCheck", HttpStatusCode.NotFound);

                return _mapper.Map<EVCheckResponse>(entity);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById EVCheck failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
