


using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Exceptions;
using eMotoCare.DAL;
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

        public async Task<Guid> CreateAsync(EVCheckRequest req)
        {

            try
            {

                var entity = _mapper.Map<EVCheck>(req);
                entity.Id = Guid.NewGuid();

                await _unitOfWork.EVChecks.CreateAsync(entity);

                var appointment = await _unitOfWork.Appointments.GetByIdAsync(req.AppointmentId);
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


                var stages = await _unitOfWork.MaintenanceStages.FindByMaintenancePlanIdAsync(plan.Id);

                var matchedStage = stages
                                            .Select(s => new
                                            {
                                                Stage = s,
                                                Km = int.Parse(s.Mileage.ToString().Replace("KM", "").Trim())
                                            })
                                            .Where(x => x.Km <= entity.Odometer)
                                            .OrderByDescending(x => x.Km)
                                            .Select(x => x.Stage)
                                            .FirstOrDefault();
                if (matchedStage == null)
                {
                    throw new AppException("No matching maintenance stage for Odo", HttpStatusCode.NotFound);
                }

                var allVehicleStages = await _unitOfWork.VehicleStages.GetByVehicleIdAsync(vehicle.Id);
                var stageList = allVehicleStages.ToList();
                var stageKmList = stages
                        .Select(s => new { s.Id, Km = int.Parse(s.Mileage.ToString().Replace("KM", "").Trim()) })
                        .ToDictionary(x => x.Id, x => x.Km);
                var nextStage = stages
                                    .Select(s => new
                                    {
                                        Stage = s,
                                        Km = int.Parse(s.Mileage.ToString().Replace("KM", "").Trim())
                                    })
                                    .Where(x => x.Km > entity.Odometer)
                                    .OrderBy(x => x.Km)
                                    .FirstOrDefault();
                foreach (var vs in stageList)
                {
                    if (!stageKmList.TryGetValue(vs.MaintenanceStageId, out var stageKm))
                        continue;

                    if (stageKm <= entity.Odometer)
                    {
                        if (vs.Status != VehicleStageStatus.COMPLETED)
                            vs.Status = VehicleStageStatus.OVERDUE;
                    }
                    else if (nextStage != null && vs.MaintenanceStageId == nextStage.Stage.Id)
                    {
                        vs.Status = VehicleStageStatus.UPCOMING;
                    }
                    else
                    {
                        vs.Status = VehicleStageStatus.FUTURE;
                    }

                    _unitOfWork.VehicleStages.Update(vs);
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
    }
}
