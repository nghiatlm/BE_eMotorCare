

using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Exceptions;
using eMotoCare.DAL;
using Microsoft.Extensions.Logging;
using System.Net;

namespace eMototCare.BLL.Services.ExportNoteDetailServices
{
    public class ExportNoteDetailService : IExportNoteDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExportNoteDetailService> _logger;

        public ExportNoteDetailService(IUnitOfWork unitOfWork, ILogger<ExportNoteDetailService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task UpdateAsync(Guid id, ExportNoteDetailUpdateRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.ExportNoteDetails.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy ExportNoteDetails", HttpStatusCode.NotFound);
                var note = entity.ExportNote.Note;
                string code = note.Split(':')[1].Trim();
                var appointment = await _unitOfWork.Appointments.GetByCodeAsync(code);
                if (req.PartItemId != null)
                {
                    entity.PartItemId = req.PartItemId;
                }

                if (req.Note != null)
                {
                    entity.Note = req.Note;
                }

                if (req.ExportIndex != null)
                {
                    entity.ExportIndex = req.ExportIndex;
                }

                if (req.Status != null)
                {
                    if (req.Status == ExportNoteDetailStatus.COMPLETED && req.PartItemId != null && entity.ExportNote.Type == ExportType.REPLACEMENT)
                    {
                        var detail = appointment.EVCheck.EVCheckDetails
                                    .FirstOrDefault(d => d.ProposedReplacePartId == entity.ProposedReplacePartId);
                        if (appointment.VehicleId != null)
                        {
                            var vehiclePartItem = new VehiclePartItem
                            {
                                Id = Guid.NewGuid(),
                                InstallDate = DateTime.UtcNow,
                                VehicleId = appointment.VehicleId.Value,
                                PartItemId = req.PartItemId.Value,
                                ReplaceForId = detail.PartItemId
                            };
                            await _unitOfWork.VehiclePartItems.CreateAsync(vehiclePartItem);
                        } else if (appointment.VehicleStage != null)
                        {
                            var vehiclePartItem = new VehiclePartItem
                            {
                                Id = Guid.NewGuid(),
                                InstallDate = DateTime.UtcNow,
                                VehicleId = appointment.VehicleStage.VehicleId,
                                PartItemId = req.PartItemId.Value,
                                ReplaceForId = detail.PartItemId
                            };
                            await _unitOfWork.VehiclePartItems.CreateAsync(vehiclePartItem);
                        }
                        
                        var partItem = await _unitOfWork.PartItems.GetByIdAsync(req.PartItemId.Value);
                        if (partItem != null)
                        {
                            partItem.Status = PartItemStatus.INSTALLED;
                            partItem.Quantity = 0;
                            partItem.ServiceCenterInventoryId = null;
                            await _unitOfWork.PartItems.UpdateAsync(partItem);
                            if (partItem.WarrantyPeriod != null)
                            {
                                    partItem.WarantyStartDate = DateTime.UtcNow;
                                    partItem.WarantyEndDate = DateTime.UtcNow.AddMonths(partItem.WarrantyPeriod.Value);
                            }
                        }
                        

                    } else if (req.Status == ExportNoteDetailStatus.COMPLETED && entity.PartItemId != null && entity.ExportNote.Type == ExportType.REPLACEMENT)
                    {
                        var detail = appointment.EVCheck.EVCheckDetails
                                    .FirstOrDefault(d => d.ProposedReplacePartId == entity.ProposedReplacePartId);
                        if (appointment.VehicleId != null)
                        {
                            var vehiclePartItem = new VehiclePartItem
                            {
                                Id = Guid.NewGuid(),
                                InstallDate = DateTime.UtcNow,
                                VehicleId = appointment.VehicleId.Value,
                                PartItemId = entity.PartItemId.Value,
                                ReplaceForId = detail.PartItemId
                            };
                            await _unitOfWork.VehiclePartItems.CreateAsync(vehiclePartItem);
                        }
                        else if (appointment.VehicleStage != null)
                        {
                            var vehiclePartItem = new VehiclePartItem
                            {
                                Id = Guid.NewGuid(),
                                InstallDate = DateTime.UtcNow,
                                VehicleId = appointment.VehicleStage.VehicleId,
                                PartItemId = entity.PartItemId.Value,
                                ReplaceForId = detail.PartItemId
                            };
                            await _unitOfWork.VehiclePartItems.CreateAsync(vehiclePartItem);
                        }

                        var partItem = await _unitOfWork.PartItems.GetByIdAsync(entity.PartItemId.Value);
                        if (partItem != null)
                        {
                            partItem.Status = PartItemStatus.INSTALLED;
                            partItem.Quantity = 0;
                            partItem.ServiceCenterInventoryId = null;
                            await _unitOfWork.PartItems.UpdateAsync(partItem);
                            if (partItem.WarrantyPeriod != null)
                            {
                                partItem.WarantyStartDate = DateTime.UtcNow;
                                partItem.WarantyEndDate = DateTime.UtcNow.AddMonths(partItem.WarrantyPeriod.Value);
                            }
                        } 
                    }

                    if (entity.ExportNote.ExportNoteDetails.All(d => d.Status == ExportNoteDetailStatus.COMPLETED))
                    {
                        entity.ExportNote.ExportNoteStatus = ExportNoteStatus.COMPLETED;
                    }
                    entity.Status = req.Status.Value;
                }

                

                await _unitOfWork.ExportNoteDetails.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation("Updated {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }

        }
    }
}
