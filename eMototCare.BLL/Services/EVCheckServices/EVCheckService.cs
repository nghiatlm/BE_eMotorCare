using AutoMapper;
using Azure.Core;
using eMotoCare.BO.Common.src;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Ocsp;
using System.Net;
using System.Text.RegularExpressions;
using static Grpc.Core.Metadata;

namespace eMototCare.BLL.Services.EVCheckServices
{
    public class EVCheckService : IEVCheckService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EVCheckService> _logger;

        public EVCheckService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<EVCheckService> logger,
            Utils utils
        )
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
                entity.Status = EVCheckStatus.VEHICLE_INSPECTION;
                entity.CheckDate = DateTime.UtcNow;
                await _unitOfWork.EVChecks.CreateAsync(entity);

                var appointment = await _unitOfWork.Appointments.GetByIdAsync(req.AppointmentId);
                if (appointment == null)
                    throw new AppException("Appointment not found", HttpStatusCode.NotFound);
                //if (appointment.Type == ServiceType.MAINTENANCE_TYPE)
                //{
                //    var vehicleStages = appointment.VehicleStage;
                //    var vehicleDetail = await _unitOfWork.Vehicles.GetByIdAsync(
                //        vehicleStages.VehicleId
                //    );
                //    var allVehiclePartItems =
                //        await _unitOfWork.VehiclePartItems.GetListByVehicleIdAsync(
                //            vehicleDetail.Id
                //        );
                //    var latestVehiclePartItems = allVehiclePartItems
                //        .GroupBy(vpi => vpi.PartItem.PartId)
                //        .Select(g => g.OrderByDescending(x => x.InstallDate).First())
                //        .ToList();
                //    var maintenanceStageDetails =
                //        await _unitOfWork.MaintenanceStageDetails.GetByMaintenanceStageIdAsync(
                //            vehicleStages.MaintenanceStageId
                //        );
                //    foreach (var detail in maintenanceStageDetails)
                //    {
                //        // Tìm VehiclePartItem tương thích với PartId trong MaintenanceStageDetail
                //        var matchedVehiclePartItem = latestVehiclePartItems.FirstOrDefault(vpi =>
                //            vpi.PartItem.PartId == detail.PartId
                //        );

                //        var evCheckDetail = new EVCheckDetail
                //        {
                //            Id = Guid.NewGuid(),
                //            EVCheckId = entity.Id,
                //            MaintenanceStageDetailId = detail.Id,
                //            Remedies = Remedies.NONE,
                //            PartItemId = matchedVehiclePartItem.PartItemId,
                //            Status = EVCheckDetailStatus.IN_PROGRESS,
                //        };

                //        await _unitOfWork.EVCheckDetails.CreateAsync(evCheckDetail);
                //    }
                //}

                if (appointment.Type == ServiceType.CAMPAIGN_TYPE)
                {
                    var campaign = await _unitOfWork.Programs.FindById(appointment.CampaignId.Value);
                    var campaignDetails = campaign.ProgramDetails.ToList();
                    foreach (var detail in campaignDetails)
                    {
                        var vehiclePartItems =
                            await _unitOfWork.VehiclePartItems.GetListByVehicleIdAsync(
                                appointment.VehicleId.Value
                            );
                        var latestVehiclePartItems = vehiclePartItems
                            .GroupBy(vpi => vpi.PartItem.PartId)
                            .Select(g => g.OrderByDescending(x => x.InstallDate).First())
                            .ToList();
                        var matchedVehiclePartItem = latestVehiclePartItems.FirstOrDefault(vpi =>
                            vpi.PartItem.PartId == detail.PartId
                        );
                        if (matchedVehiclePartItem == null)
                        {
                            throw new AppException("Không tìm thấy phụ tùng xe tương ứng với Campaign", HttpStatusCode.NotFound);
                        }
                        var evCheckDetail = new EVCheckDetail
                        {
                            Id = Guid.NewGuid(),
                            EVCheckId = entity.Id,
                            ProgramDetailId = detail.Id,
                            Remedies = Remedies.NONE,
                            PartItemId = matchedVehiclePartItem.PartItemId,
                            Status = EVCheckDetailStatus.IN_PROGRESS,
                        };
                        await _unitOfWork.EVCheckDetails.CreateAsync(evCheckDetail);
                    }
                }

                if (!string.IsNullOrEmpty(appointment.Note) && appointment.Note.Contains("RMA-"))
                {
                    var rmaCode = Regex.Match(appointment.Note, @"RMA-\d+-\d+");
                    var rma = await _unitOfWork.RMAs.GetByCodeAsync(rmaCode.Value);
                    foreach (var detail in rma.RMADetails)
                    {
                        if (detail.ReplacePartId != null)
                        //Trường hợp 1 đổi 1
                        {
                            var evCheckDetail = new EVCheckDetail
                            {
                                Id = Guid.NewGuid(),
                                EVCheckId = entity.Id,
                                Remedies = Remedies.REPLACE,
                                PartItemId = detail.ReplacePartId.Value,
                                Status = EVCheckDetailStatus.IN_PROGRESS,
                                Result = "Thay thế phụ tùng mới từ hãng",
                            };
                            var vehiclePartItem = new VehiclePartItem
                            {
                                Id = Guid.NewGuid(),
                                InstallDate = DateTime.UtcNow,
                                VehicleId = appointment.VehicleId.Value,
                                PartItemId = detail.ReplacePartId.Value,
                                ReplaceForId = detail.EVCheckDetail.PartItemId,
                            };
                            var exportNote = new ExportNote
                            {
                                Id = Guid.NewGuid(),
                                Code = $"EXPORT-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}",
                                ExportDate = DateTime.UtcNow,
                                Type = ExportType.REPLACEMENT,
                                ServiceCenterId = appointment.ServiceCenterId,
                                Note = "Xuất phụ tùng bảo hành gửi về từ hãng: " + appointment.Code,
                                ExportNoteStatus = ExportNoteStatus.COMPLETED,
                                TotalValue = detail.ReplacePart.Price,
                                TotalQuantity = 1,
                                ExportTo = appointment.Customer.LastName + " " + appointment.Customer.FirstName + " - " + appointment.Customer.Account.Phone,
                            };
                            var exportNoteDetail = new ExportNoteDetail
                            {
                                Id = Guid.NewGuid(),
                                ExportNoteId = exportNote.Id,
                                PartItemId = detail.ReplacePartId,
                                Quantity = 1,
                                UnitPrice = detail.ReplacePart.Price,
                                TotalPrice = detail.ReplacePart.Price,
                                Status = ExportNoteDetailStatus.COMPLETED,
                            };
                            await _unitOfWork.ExportNotes.CreateAsync(exportNote);
                            await _unitOfWork.ExportNoteDetails.CreateAsync(exportNoteDetail);
                            await _unitOfWork.VehiclePartItems.CreateAsync(vehiclePartItem);
                            await _unitOfWork.EVCheckDetails.CreateAsync(evCheckDetail);
                        }
                        else if (detail.ReplacePartId == null)
                        //Trường hợp hãng sửa chữa phụ tùng
                        {
                            var evCheckDetail = new EVCheckDetail
                            {
                                Id = Guid.NewGuid(),
                                EVCheckId = entity.Id,
                                Remedies = Remedies.REPAIR,
                                PartItemId = detail.EVCheckDetail.PartItemId,
                                Status = EVCheckDetailStatus.IN_PROGRESS,
                                Result = "Lắp đặt phụ tùng đã được sửa chữa từ hãng",
                            };
                            await _unitOfWork.EVCheckDetails.CreateAsync(evCheckDetail);
                        }
                    }
                }

                var technician = await _unitOfWork.Staffs.GetByIdAsync(req.TaskExecutorId);
                var notification = new Notification
                {
                    Id = Guid.NewGuid(),
                    Title = "Đã gán kỹ thuật viên",
                    Message = "Kỹ thuật viên: " + technician.LastName + " " + technician.FirstName + " đã được chỉ định phụ trách lịch hẹn của bạn.",
                    ReceiverId = appointment.Customer.AccountId.Value,
                    Type = NotificationEnum.APPOINTMENT_REMINDER,
                    IsRead = false,
                    SentAt = DateTime.Now,
                    ReferenceId = appointment.Id
                };

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
                    ?? throw new AppException("Không tìm thấy EVCheck", HttpStatusCode.NotFound);

                entity.Status = EVCheckStatus.CANCELLED;
                entity.EVCheckDetails.ToList().ForEach(d =>
                {
                    d.Status = EVCheckDetailStatus.CANCELED;
                });
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
                    ?? throw new AppException("Không tìm thấy EVCheck", HttpStatusCode.NotFound);

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
                var appt = entity.Appointment;
                if (appt == null && entity.AppointmentId != Guid.Empty)
                {
                    appt = new Appointment { Id = entity.AppointmentId };
                }

                if (req.Odometer != null)
                {
                    if (appt == null)
                        throw new AppException("Appointment not found", HttpStatusCode.NotFound);

                    var vehicleStage = appt.VehicleStage;

                    if (vehicleStage == null && appt.VehicleStageId.HasValue)
                    {
                        vehicleStage = await _unitOfWork.VehicleStages.GetByIdAsync(
                            appt.VehicleStageId.Value
                        );
                        appt.VehicleStage = vehicleStage;
                    }

                    var appointment = await _unitOfWork.Appointments.GetByIdAsync(
                        entity.AppointmentId
                    );

                    if (appointment?.VehicleStage == null)
                        throw new AppException("VehicleStage not found", HttpStatusCode.NotFound);

                    var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(vehicleStage.VehicleId);
                    if (vehicle == null)
                        throw new AppException("Vehicle not found", HttpStatusCode.NotFound);

                    var model = vehicle.Model;

                    var plan = await _unitOfWork.MaintenancePlans.GetByIdAsync(
                        model.MaintenancePlanId
                    );
                    if (plan == null)
                        throw new AppException(
                            "Maintenance plan not found",
                            HttpStatusCode.NotFound
                        );

                    var maintenanceStages =
                        await _unitOfWork.MaintenanceStages.GetListByPlanIdAsync(plan.Id);
                    if (!maintenanceStages.Any())
                        throw new AppException(
                            "No maintenance stages found",
                            HttpStatusCode.NotFound
                        );

                    var allVehicleStages = await _unitOfWork.VehicleStages.GetByVehicleIdAsync(
                        vehicle.Id
                    );
                    if (!allVehicleStages.Any())
                        throw new AppException("No vehicle stages found", HttpStatusCode.NotFound);

                    

                    var nextStage = maintenanceStages
                        .Where(vs => (int)vs.Mileage > entity.Odometer)
                        .OrderBy(vs => (int)vs.Mileage)
                        .FirstOrDefault();
                    var closestStage = maintenanceStages
                        .Where(ms => (int)ms.Mileage <= entity.Odometer)
                        .OrderByDescending(ms => (int)ms.Mileage)
                        .FirstOrDefault();
                    if (req.Odometer > (int)appointment.VehicleStage.MaintenanceStage.Mileage)
                    {
                        foreach (var vs in allVehicleStages)
                        {
                            if (closestStage == null)
                                break;
                            if (vs.Status == VehicleStageStatus.COMPLETED)
                                continue;
                            var stage = maintenanceStages.FirstOrDefault(ms =>
                                ms.Id == vs.MaintenanceStageId
                            );
                            if (stage == null)
                                continue;

                            // Nếu là stage gần nhất
                            if (closestStage != null && stage.Id == closestStage.Id)
                            {
                                var diff = entity.Odometer - (int)stage.Mileage;
                                if (diff > 1000 && vs.Status != VehicleStageStatus.COMPLETED)
                                    vs.Status = VehicleStageStatus.EXPIRED;
                                else
                                    vs.Status = VehicleStageStatus.UPCOMING;
                                    vs.ActualMaintenanceMileage = req.Odometer.Value;
                            }
                            // Nếu là stage trước đó
                            else if (closestStage != null && stage.Mileage < closestStage.Mileage)
                            {
                                if (vs.Status != VehicleStageStatus.COMPLETED)
                                    vs.Status = VehicleStageStatus.EXPIRED;
                            }
                            

                            _unitOfWork.VehicleStages.Update(vs);
                        }

                        if (appointment.Type == ServiceType.MAINTENANCE_TYPE)
                        {
                            var vehicleStages = appointment.VehicleStage;
                            var vehicleDetail = await _unitOfWork.Vehicles.GetByIdAsync(
                                vehicleStages.VehicleId
                            );
                            var allVehiclePartItems =
                                await _unitOfWork.VehiclePartItems.GetListByVehicleIdAsync(
                                    vehicleDetail.Id
                                );
                            var latestVehiclePartItems = allVehiclePartItems
                                .GroupBy(vpi => vpi.PartItem.PartId)
                                .Select(g => g.OrderByDescending(x => x.InstallDate).First())
                                .ToList();
                            Guid maintenanceStageId;
                            if (closestStage != null)
                            {
                                var diff = entity.Odometer - (int)closestStage.Mileage;
                                if (diff > 1000)
                                {
                                    maintenanceStageId = nextStage.Id;
                                }
                                else
                                {
                                    maintenanceStageId = closestStage.Id;
                                }
                            }
                            else if (nextStage == null)
                            {
                                maintenanceStageId = closestStage.Id;
                            }
                            else
                            {

                                maintenanceStageId = nextStage.Id;

                            }


                            var maintenanceStageDetails =
                                    await _unitOfWork.MaintenanceStageDetails.GetByMaintenanceStageIdAsync(
                                        maintenanceStageId
                                    );
                            foreach (var detail in maintenanceStageDetails)
                            {
                                // Tìm VehiclePartItem tương thích với PartId trong MaintenanceStageDetail
                                var matchedVehiclePartItem = latestVehiclePartItems.FirstOrDefault(vpi =>
                                    vpi.PartItem.PartId == detail.PartId
                                );

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

                        
                    } else
                    {
                        if (appointment.Type == ServiceType.MAINTENANCE_TYPE)
                        {
                            var vehicleStages = appointment.VehicleStage;
                            vehicleStages.ActualMaintenanceMileage = req.Odometer.Value;
                            await _unitOfWork.VehicleStages.UpdateAsync(vehicleStages);
                            var vehicleDetail = await _unitOfWork.Vehicles.GetByIdAsync(
                                vehicleStages.VehicleId
                            );
                            var allVehiclePartItems =
                                await _unitOfWork.VehiclePartItems.GetListByVehicleIdAsync(
                                    vehicleDetail.Id
                                );
                            var latestVehiclePartItems = allVehiclePartItems
                                .GroupBy(vpi => vpi.PartItem.PartId)
                                .Select(g => g.OrderByDescending(x => x.InstallDate).First())
                                .ToList();
                            var maintenanceStageDetails =
                                await _unitOfWork.MaintenanceStageDetails.GetByMaintenanceStageIdAsync(
                                    vehicleStages.MaintenanceStageId
                                );
                            foreach (var detail in maintenanceStageDetails)
                            {
                                // Tìm VehiclePartItem tương thích với PartId trong MaintenanceStageDetail
                                var matchedVehiclePartItem = latestVehiclePartItems.FirstOrDefault(vpi =>
                                    vpi.PartItem.PartId == detail.PartId
                                );

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
                    }
                    

                    
                }
                if (req.Status == EVCheckStatus.INSPECTION_COMPLETED)
                {
                    var evCheckDetails = await _unitOfWork.EVCheckDetails.GetByEvCheckId(entity.Id);

                    entity.ServicePrice = evCheckDetails.Sum(d => d.PriceService ?? 0);
                    entity.PartPrice = evCheckDetails.Sum(d => d.PricePart ?? 0);
                    var totalBeforeTax = evCheckDetails.Sum(d => d.TotalAmount ?? 0);
                    entity.VAT = totalBeforeTax * 0.08m;
                    entity.TotalAmout = totalBeforeTax + entity.VAT;

                    foreach (var detail in evCheckDetails)
                    {
                        if (detail.Remedies == Remedies.WARRANTY)
                            detail.Status = EVCheckDetailStatus.COMPLETED;
                        if (detail.Remedies == Remedies.NONE)
                            detail.Status = EVCheckDetailStatus.COMPLETED;
                        _unitOfWork.EVCheckDetails.Update(detail);
                    }
                    var appointment = await _unitOfWork.Appointments.GetByIdAsync(
                        entity.AppointmentId
                    );
                    var notification = new Notification
                    {
                        Id = Guid.NewGuid(),
                        Title = "Đã hoàn thành kiểm tra xe.",
                        Message = "Đã hoàn tất quá trình kiểm tra xe và gửi báo giá. Vui lòng xác nhận qua ứng dụng.",
                        ReceiverId = appointment.Customer.AccountId.Value,
                        Type = NotificationEnum.APPOINTMENT_REMINDER,
                        IsRead = false,
                        SentAt = DateTime.Now,
                        ReferenceId = appointment.Id
                    };
                    _unitOfWork.Notifications.CreateAsync(notification);
                }

                if (req.Status == EVCheckStatus.CANCELLED)
                {
                    var evCheckDetails = await _unitOfWork.EVCheckDetails.GetByEvCheckId(entity.Id);

                    foreach (var detail in evCheckDetails)
                    {
                        detail.Status = EVCheckDetailStatus.CANCELED;
                        _unitOfWork.EVCheckDetails.Update(detail);
                    }
                }

                if (req.Status == EVCheckStatus.REPAIR_COMPLETED)
                {
                    if (appt != null)
                    {
                        if (appt.Status != AppointmentStatus.REPAIR_COMPLETED)
                        {
                            appt.Status = AppointmentStatus.REPAIR_COMPLETED;
                        }
                    }
                    else
                    {
                        await _unitOfWork.Appointments.UpdateStatusByIdAsync(
                            entity.AppointmentId,
                            AppointmentStatus.REPAIR_COMPLETED
                        );
                    }
                    var appointment = await _unitOfWork.Appointments.GetByIdAsync(
                        entity.AppointmentId
                    );
                    var notification = new Notification
                    {
                        Id = Guid.NewGuid(),
                        Title = "Vui lòng thanh toán hoá đơn",
                        Message = "Quá trình bảo dưỡng/sửa chữa đã hoàn thành. Vui lòng thanh toán tại quầy hoặc qua app.",
                        ReceiverId = appointment.Customer.AccountId.Value,
                        Type = NotificationEnum.APPOINTMENT_REMINDER,
                        IsRead = false,
                        SentAt = DateTime.Now,
                        ReferenceId = appointment.Id
                    };
                    _unitOfWork.Notifications.CreateAsync(notification);
                }

                if (req.Status == EVCheckStatus.COMPLETED)
                {
                    if (appt != null)
                    {
                        if (appt.Status != AppointmentStatus.COMPLETED)
                        {
                            appt.Status = AppointmentStatus.COMPLETED;
                        }
                    }
                    else
                    {
                        await _unitOfWork.Appointments.UpdateStatusByIdAsync(
                            entity.AppointmentId,
                            AppointmentStatus.COMPLETED
                        );
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

        public async Task<bool> QuoteApprove(Guid id)
        {
            var evCheck = await _unitOfWork.EVChecks.GetByIdAsync(id);
            if (evCheck == null)
            {
                throw new AppException("EVCheck not found", HttpStatusCode.NotFound);
            }
            evCheck.Status = EVCheckStatus.REPAIR_IN_PROGRESS;
            var replaceDetails = evCheck
                .EVCheckDetails.Where(d => d.ProposedReplacePartId != null)
                .ToList();
            if (replaceDetails.Any())
            {
                var customerName =
                    evCheck.Appointment.Customer.LastName
                    + " "
                    + evCheck.Appointment.Customer.FirstName;
                var phone = evCheck.Appointment.Customer.Account.Phone;
                var exportNoteId = Guid.NewGuid();
                var exportNote = new ExportNote
                {
                    Id = exportNoteId,
                    Code = $"EXPORT-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}",
                    ExportDate = DateTime.UtcNow,
                    Type = ExportType.REPLACEMENT,
                    ServiceCenterId = evCheck.Appointment.ServiceCenterId,
                    Note = "Xuất phụ tùng cho appointment: " + evCheck.Appointment.Code,
                    ExportNoteStatus = ExportNoteStatus.PROCESSING,
                    TotalValue = 0,
                    TotalQuantity = 0,
                    ExportTo = customerName + " - " + phone,
                };
                await _unitOfWork.ExportNotes.CreateAsync(exportNote);

                foreach (var detail in replaceDetails)
                {
                    var isInStock = await _unitOfWork.PartItems.GetAvailablePartItemsByPartIdAsync(detail.ProposedReplacePartId.Value, detail.EVCheck.Appointment.ServiceCenterId);
                    var exportNoteDetail = new ExportNoteDetail
                    {
                        Id = Guid.NewGuid(),
                        ExportNoteId = exportNoteId,
                        PartItem = null,
                        ProposedReplacePartId = detail.ProposedReplacePartId,
                        Quantity = 1,
                        UnitPrice = detail.ProposedReplacePart.PartItems.FirstOrDefault().Price,
                    };
                    if (isInStock.Any())
                    {
                        exportNoteDetail.Status = ExportNoteDetailStatus.STOCK_FOUND;
                    } else
                    {
                        exportNoteDetail.Status = ExportNoteDetailStatus.STOCK_NOT_FOUND;
                    }
                    exportNote.TotalQuantity += 1;
                    exportNoteDetail.TotalPrice = exportNoteDetail.UnitPrice * exportNoteDetail.Quantity;
                    exportNote.TotalValue += exportNoteDetail.TotalPrice;
                    await _unitOfWork.ExportNoteDetails.CreateAsync(exportNoteDetail);
                }
            }

            var evCheckDetails = await _unitOfWork.EVCheckDetails.GetByEvCheckId(evCheck.Id);

            if (evCheckDetails.Any() &&
                                        evCheckDetails.All(d =>
                                            d.Remedies == Remedies.WARRANTY ||
                                            d.Remedies == Remedies.NONE))
            {
                evCheck.Status = EVCheckStatus.COMPLETED;
                evCheck.Appointment.Status = AppointmentStatus.COMPLETED;
            }

            evCheck.Appointment.Status = AppointmentStatus.QUOTE_APPROVED;
            await _unitOfWork.EVChecks.UpdateAsync(evCheck);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<List<EVCheckReplacementResponse>?> GetReplacementsByAppointmentAsync(
            Guid appointmentId
        )
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(appointmentId);

            if (appointment == null)
                throw new AppException("Appointment not found.", HttpStatusCode.NotFound);
            var modelId = Guid.Empty;
            if (appointment.VehicleId != null)
            {
                modelId = appointment.Vehicle.ModelId;
            }
            else if (appointment.VehicleStage != null)
            {
                modelId = appointment.VehicleStage.Vehicle.ModelId;
            }
            else
            {
                throw new AppException(
                    "Vehicle or VehicleStage not found.",
                    HttpStatusCode.NotFound
                );
            }
            var serviceCenterId = appointment.ServiceCenterId;

            var parts = await _unitOfWork.Parts.GetReplacementPartsAsync(modelId, serviceCenterId);

            return parts;
        }
    }
}
