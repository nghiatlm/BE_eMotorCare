using System.Globalization;
using System.Net;
using System.Text.Json;
using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.MaintenanceStageServices
{
    public class MaintenanceStageService : IMaintenanceStageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MaintenanceStageService> _logger;

        public MaintenanceStageService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<MaintenanceStageService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<MaintenanceStageResponse>> GetPagedAsync(
            Guid? maintenancePlanId,
            string? description,
            DurationMonth? durationMonth,
            Mileage? mileage,
            string? name,
            Status? status,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.MaintenanceStages.GetPagedAsync(
                    maintenancePlanId,
                    description,
                    durationMonth,
                    mileage,
                    name,
                    status,
                    page,
                    pageSize
                );
                var rows = _mapper.Map<List<MaintenanceStageResponse>>(items);
                return new PageResult<MaintenanceStageResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Maintenance Stage failed: {Message}", ex.Message);
                //throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
                throw new AppException(ex.Message);
            }
        }

        public async Task<Guid> CreateAsync(MaintenanceStageRequest req)
        {
            try
            {
                var entity = _mapper.Map<MaintenanceStage>(req);
                entity.Id = Guid.NewGuid();
                entity.Status = Status.ACTIVE;

                await _unitOfWork.MaintenanceStages.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created Maintenance Stage ");
                return entity.Id;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Maintenance Stage failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.MaintenanceStages.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy Maintenance Stage",
                        HttpStatusCode.NotFound
                    );

                entity.Status = Status.IN_ACTIVE;
                await _unitOfWork.MaintenanceStages.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted Maintenance Stage {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete Maintenance Stage failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, MaintenanceStageUpdateRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.MaintenanceStages.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy Maintenance Stage",
                        HttpStatusCode.NotFound
                    );

                _mapper.Map(req, entity);

                await _unitOfWork.MaintenanceStages.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated Maintenance Stage {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Maintenance Stage failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<MaintenanceStageResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.MaintenanceStages.GetByIdAsync(id);
                if (entity is null)
                    throw new AppException(
                        "Không tìm thấy Maintenance Stage",
                        HttpStatusCode.NotFound
                    );

                return _mapper.Map<MaintenanceStageResponse>(entity);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById Maintenance Stage failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<int> ImportFromCsvAsync(Stream csvStream)
        {
            try
            {
                using var reader = new StreamReader(csvStream);

                if (reader.EndOfStream)
                    throw new AppException("File CSV trống.", HttpStatusCode.BadRequest);

                var headerLine = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(headerLine))
                    throw new AppException(
                        "File CSV thiếu dòng header.",
                        HttpStatusCode.BadRequest
                    );

                var headers = headerLine.Split(',', StringSplitOptions.TrimEntries);

                int GetIndex(string name) =>
                    Array.FindIndex(
                        headers,
                        h => string.Equals(h, name, StringComparison.OrdinalIgnoreCase)
                    );

                int idxPlanId = GetIndex("MaintenancePlanId");
                int idxName = GetIndex("Name");
                int idxDescription = GetIndex("Description");
                int idxMileage = GetIndex("Mileage");
                int idxDuration = GetIndex("DurationMonth");
                int idxEstimated = GetIndex("EstimatedTime");

                if (idxPlanId < 0 || idxName < 0 || idxMileage < 0 || idxDuration < 0)
                    throw new AppException(
                        "File CSV phải có các cột: MaintenancePlanId, Name, Mileage, DurationMonth.",
                        HttpStatusCode.BadRequest
                    );

                var list = new List<MaintenanceStage>();
                var lineNumber = 1;

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    lineNumber++;

                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var cols = line.Split(',', StringSplitOptions.TrimEntries);
                    if (cols.Length < headers.Length)
                    {
                        _logger.LogWarning("Dòng {Line} không đủ cột, bỏ qua.", lineNumber);
                        continue;
                    }

                    if (!Guid.TryParse(cols[idxPlanId], out var planId))
                    {
                        _logger.LogWarning(
                            "Dòng {Line} MaintenancePlanId không hợp lệ, bỏ qua.",
                            lineNumber
                        );
                        continue;
                    }

                    var name = cols[idxName];
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        _logger.LogWarning("Dòng {Line} thiếu Name, bỏ qua.", lineNumber);
                        continue;
                    }

                    if (!Enum.TryParse<Mileage>(cols[idxMileage], true, out var mileage))
                    {
                        _logger.LogWarning("Dòng {Line} Mileage không hợp lệ, bỏ qua.", lineNumber);
                        continue;
                    }

                    if (!Enum.TryParse<DurationMonth>(cols[idxDuration], true, out var duration))
                    {
                        _logger.LogWarning(
                            "Dòng {Line} DurationMonth không hợp lệ, bỏ qua.",
                            lineNumber
                        );
                        continue;
                    }

                    string? desc = idxDescription >= 0 ? cols[idxDescription] : null;

                    int? estimatedTime = null;
                    if (idxEstimated >= 0 && !string.IsNullOrWhiteSpace(cols[idxEstimated]))
                    {
                        if (
                            int.TryParse(
                                cols[idxEstimated],
                                NumberStyles.Integer,
                                CultureInfo.InvariantCulture,
                                out var est
                            )
                        )
                            estimatedTime = est;
                    }

                    var entity = new MaintenanceStage
                    {
                        Id = Guid.NewGuid(),
                        MaintenancePlanId = planId,
                        Name = name,
                        Description = desc,
                        Mileage = mileage,
                        DurationMonth = duration,
                        EstimatedTime = estimatedTime,
                        Status = Status.ACTIVE,
                    };

                    list.Add(entity);
                }

                if (list.Count == 0)
                    throw new AppException(
                        "Không có dòng hợp lệ nào trong file CSV.",
                        HttpStatusCode.BadRequest
                    );

                foreach (var ms in list)
                    await _unitOfWork.MaintenanceStages.CreateAsync(ms);

                await _unitOfWork.SaveAsync();

                _logger.LogInformation(
                    "Import {Count} MaintenanceStage từ CSV thành công.",
                    list.Count
                );
                return list.Count;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "ImportFromCsvAsync(MaintenanceStage) failed: {Message}",
                    ex.Message
                );
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<int> ImportFromJsonAsync(Stream jsonStream)
        {
            try
            {
                using var reader = new StreamReader(jsonStream);
                var json = await reader.ReadToEndAsync();

                var items = JsonSerializer.Deserialize<List<MaintenanceStageRequest>>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (items == null || items.Count == 0)
                    throw new AppException(
                        "File JSON không chứa dữ liệu.",
                        HttpStatusCode.BadRequest
                    );

                var list = new List<MaintenanceStage>();

                foreach (var req in items)
                {
                    var entity = _mapper.Map<MaintenanceStage>(req);
                    entity.Id = Guid.NewGuid();
                    entity.Status = Status.ACTIVE;
                    list.Add(entity);
                }

                foreach (var ms in list)
                    await _unitOfWork.MaintenanceStages.CreateAsync(ms);

                await _unitOfWork.SaveAsync();

                _logger.LogInformation(
                    "Import {Count} MaintenanceStage từ JSON thành công.",
                    list.Count
                );
                return list.Count;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "ImportFromJsonAsync(MaintenanceStage) failed: {Message}",
                    ex.Message
                );
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
