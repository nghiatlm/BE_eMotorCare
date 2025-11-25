

using AutoMapper;
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
using System.Net;

namespace eMototCare.BLL.Services.ImportNoteServices
{
    public class ImportNoteService : IImportNoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ImportNoteService> _logger;
        private readonly Utils _utils;

        public ImportNoteService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ImportNoteService> logger, Utils utils)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _utils = utils;
        }

        public async Task<PageResult<ImportNoteResponse>> GetPagedAsync(
             string? code,
             DateTime? fromDate,
             DateTime? toDate,
             string? importFrom,
             string? supplier,
             ImportType? importType,
             decimal? totalAmount,
             Guid? importById,
             Guid? serviceCenterId,
             ImportNoteStatus? importNoteStatus,
             int page,
             int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.ImportNotes.GetPagedAsync(
                code,
                fromDate,
                toDate,
                importFrom,
                supplier,
                importType,
                totalAmount,
                importById,
                serviceCenterId,
                importNoteStatus,
                page,
                pageSize
                );
                var rows = _mapper.Map<List<ImportNoteResponse>>(items);
                return new PageResult<ImportNoteResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged ImportNote failed: {Message}", ex.Message);
                //throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
                throw new AppException(ex.Message);
            }
        }

        public async Task<Guid> CreateAsync(ImportNoteRequest req)
        {
            try
            {
                Guid partId = Guid.Empty;
                var storeKeeper = await _unitOfWork.Staffs.GetByIdAsync(req.ImportById);
                if (storeKeeper == null) throw new AppException("Không tìm thấy người nhập kho", HttpStatusCode.NotFound);
                var serviceCenter = await _unitOfWork.ServiceCenters.GetByIdAsync(req.ServiceCenterId);
                if (serviceCenter == null) throw new AppException("Không tìm thấy Service Center", HttpStatusCode.NotFound);
                var parttype = await _unitOfWork.PartTypes.GetByIdAsync(req.PartRequest.PartTypeId);
                if (parttype == null) throw new AppException("Không tìm thấy Part Type", HttpStatusCode.NotFound);

                var entity = _mapper.Map<ImportNote>(req);
                entity.ImportDate = DateTime.UtcNow;
                entity.Code = $"IMPORT-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
                if (req.PartRequest.PartId == null)
                {
                    var newPart = await CreatePart(req.PartRequest.Name!, req.PartRequest.Image!, req.PartRequest.PartTypeId);
                    partId = newPart.Id;
                }
                else
                {
                    var part = await _unitOfWork.Parts.GetByIdAsync(req.PartRequest.PartId.Value);
                    if (part == null) throw new AppException("Không tìm thấy Part", HttpStatusCode.NotFound);
                    partId = part.Id;
                }
                if (req.PartRequest.PartItemRequest == null) throw new AppException("Part Item Request is required", HttpStatusCode.BadRequest);
                // ensure the service center has inventory
                var inventory = await _unitOfWork.ServiceCenterInventories.GetByServiceCenterId(serviceCenter.Id);
                if (inventory == null) throw new AppException("Không tìm thấy kho hàng cho Service Center", HttpStatusCode.NotFound);

                List<PartItem> partItems = new List<PartItem>();
                foreach (var item in req.PartRequest.PartItemRequest)
                {
                    if (item.IsManufacturerWarranty && item.SerialNumber == null) throw new AppException("SerialNumber is required", HttpStatusCode.BadRequest);
                    bool checkSerial = await _unitOfWork.PartItems.ExistsSerialNumberAsync(item.SerialNumber);
                    if (checkSerial) throw new AppException($"Serial Number {item.SerialNumber} đã tồn tại", HttpStatusCode.Conflict);
                    var partItem = new PartItem
                    {
                        PartId = partId,
                        SerialNumber = item.SerialNumber,
                        Price = item.Price,
                        WarrantyPeriod = item.WarrantyPeriod,
                        Quantity = item.Quantity,
                        Status = PartItemStatus.ACTIVE,
                        ServiceCenterInventoryId = inventory.Id,
                        IsManufacturerWarranty = item.IsManufacturerWarranty,
                        WarantyStartDate = item.WarantyStartDate,
                        WarantyEndDate = item.WarantyStartDate?.AddMonths(item.WarrantyPeriod ?? 0)
                    };
                    partItems.Add(partItem);
                }
                // assign ids to part items and add them to context
                foreach (var pi in partItems)
                {
                    pi.Id = Guid.NewGuid();
                    _unitOfWork.PartItems.Create(pi);
                }

                entity.TotalAmout = partItems.Sum(pi => pi.Price);
                _unitOfWork.ImportNotes.Create(entity);

                entity.ImportNoteDetails = partItems.Select(pi => new ImportNoteDetail
                {
                    PartItemId = pi.Id,
                    ImportNoteId = entity.Id,
                    Quantity = pi.Quantity,
                    UnitPrice = pi.Price,
                    TotalPrice = pi.Price * pi.Quantity,
                    Note = string.Empty
                }).ToList();

                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created ImportNote");
                return entity.Id;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create ImportNote failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Part> CreatePart(string Name, string image, Guid partTypeId)
        {
            try
            {
                var existing = await _unitOfWork.Parts.ExistsNameAsync(Name);
                if (existing) throw new AppException("Part Name đã tồn tại", HttpStatusCode.Conflict);
                var code = await _utils.GeneratePartCodeAsync();
                if (await _unitOfWork.Parts.ExistsCodeAsync(code)) throw new AppException("Part Code đã tồn tại", HttpStatusCode.Conflict);
                var part = new Part
                {
                    Id = Guid.NewGuid(),
                    PartTypeId = partTypeId,
                    Name = Name,
                    Image = image,
                    Status = Status.ACTIVE,
                    Code = code,
                    CreatedAt = DateTime.UtcNow
                };
                // add part to context (will be saved when SaveAsync is called)
                _unitOfWork.Parts.Create(part);
                return part;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.ImportNotes.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy Part",
                        HttpStatusCode.NotFound
                    );

                // entity.ImportNoteStatus = ImportNoteStatus.CANCELLED;
                await _unitOfWork.ImportNotes.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted ImportNote {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete ImportNote failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, ImportNoteUpdateRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.ImportNotes.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy ImportNotes",
                        HttpStatusCode.NotFound
                    );

                var code = req.Code.Trim();
                if (
                    !string.Equals(entity.Code, code, StringComparison.OrdinalIgnoreCase)
                    && await _unitOfWork.ImportNotes.ExistsCodeAsync(code)
                )
                    throw new AppException("Code đã tồn tại", HttpStatusCode.Conflict);


                _mapper.Map(req, entity);
                entity.Code = code;

                await _unitOfWork.ImportNotes.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated ImportNote {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update ImportNote failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }


        }

        public async Task<ImportNoteResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.ImportNotes.GetByIdAsync(id);
                if (entity is null)
                    throw new AppException("Không tìm thấy ImportNote", HttpStatusCode.NotFound);

                return _mapper.Map<ImportNoteResponse>(entity);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById ImportNote failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
