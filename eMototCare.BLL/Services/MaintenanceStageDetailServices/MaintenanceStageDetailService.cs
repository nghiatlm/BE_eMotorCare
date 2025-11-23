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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.MaintenanceStageDetailServices
{
    public class MaintenanceStageDetailService : IMaintenanceStageDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MaintenanceStageDetailService> _logger;

        public MaintenanceStageDetailService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<MaintenanceStageDetailService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<MaintenanceStageDetailResponse>> GetPagedAsync(
            Guid? maintenanceStageId,
            Guid? partId,
            ActionType[]? actionType,
            string? description,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.MaintenanceStageDetails.GetPagedAsync(
                    maintenanceStageId,
                    partId,
                    actionType,
                    description,
                    page,
                    pageSize
                );
                var rows = _mapper.Map<List<MaintenanceStageDetailResponse>>(items);
                return new PageResult<MaintenanceStageDetailResponse>(
                    rows,
                    pageSize,
                    page,
                    (int)total
                );
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "GetPaged Maintenance Stage Detail failed: {Message}",
                    ex.Message
                );
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<MaintenanceStageDetailResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var en = await _unitOfWork.MaintenanceStageDetails.GetByIdAsync(id);
                if (en is null)
                    throw new AppException(
                        "Không tìm thấy Maintenance Stage Detail",
                        HttpStatusCode.NotFound
                    );

                return _mapper.Map<MaintenanceStageDetailResponse>(en);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "GetById Maintenance Stage Detail failed: {Message}",
                    ex.Message
                );
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Guid> CreateAsync(MaintenanceStageDetailRequest req)
        {
            try
            {
                var entity = _mapper.Map<MaintenanceStageDetail>(req);
                entity.Id = Guid.NewGuid();
                entity.Status = Status.ACTIVE;

                await _unitOfWork.MaintenanceStageDetails.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created MaintenanceStageDetail ");
                return entity.Id;
            }
            catch (AppException e)
            {
                throw new AppException(e.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create MaintenanceStageDetail failed: {Message}", ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.MaintenanceStageDetails.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy MaintenanceStageDetail",
                        HttpStatusCode.NotFound
                    );

                entity.Status = Status.IN_ACTIVE;
                await _unitOfWork.MaintenanceStageDetails.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted MaintenanceStageDetail {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete MaintenanceStageDetail failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, MaintenanceStageDetailUpdateRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.MaintenanceStageDetails.GetByIdAsync(id)
                    ?? throw new AppException(
                        "Không tìm thấy MaintenanceStageDetail",
                        HttpStatusCode.NotFound
                    );

                _mapper.Map(req, entity);

                await _unitOfWork.MaintenanceStageDetails.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated MaintenanceStageDetail {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update MaintenanceStageDetail failed: {Message}", ex.Message);
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

                int idxStageId = GetIndex("MaintenanceStageId");
                int idxPartId = GetIndex("PartId");
                int idxActionType = GetIndex("ActionType");
                int idxDesc = GetIndex("Description");
                int idxStatus = GetIndex("Status");

                if (idxStageId < 0 || idxPartId < 0 || idxActionType < 0)
                    throw new AppException(
                        "File CSV phải có các cột: MaintenanceStageId, PartId, ActionType.",
                        HttpStatusCode.BadRequest
                    );

                var list = new List<MaintenanceStageDetail>();
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

                    if (!Guid.TryParse(cols[idxStageId], out var stageId))
                    {
                        _logger.LogWarning(
                            "Dòng {Line} MaintenanceStageId không hợp lệ, bỏ qua.",
                            lineNumber
                        );
                        continue;
                    }

                    if (!Guid.TryParse(cols[idxPartId], out var partId))
                    {
                        _logger.LogWarning("Dòng {Line} PartId không hợp lệ, bỏ qua.", lineNumber);
                        continue;
                    }

                    var actionStr = cols[idxActionType];
                    if (string.IsNullOrWhiteSpace(actionStr))
                    {
                        _logger.LogWarning("Dòng {Line} ActionType trống, bỏ qua.", lineNumber);
                        continue;
                    }

                    // VD: "CHECK,REPLACE"
                    var actionTokens = actionStr.Split(
                        new[] { ',', ';', '|' },
                        StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
                    );

                    var actions = new List<ActionType>();
                    foreach (var token in actionTokens)
                    {
                        if (Enum.TryParse<ActionType>(token, true, out var at))
                            actions.Add(at);
                        else
                            _logger.LogWarning(
                                "Dòng {Line} ActionType không hợp lệ: {Value}",
                                lineNumber,
                                token
                            );
                    }

                    if (!actions.Any())
                    {
                        _logger.LogWarning(
                            "Dòng {Line} không có ActionType hợp lệ, bỏ qua.",
                            lineNumber
                        );
                        continue;
                    }

                    string? desc = idxDesc >= 0 ? cols[idxDesc] : null;

                    Status status = Status.ACTIVE;
                    if (idxStatus >= 0 && !string.IsNullOrWhiteSpace(cols[idxStatus]))
                    {
                        if (!Enum.TryParse<Status>(cols[idxStatus], true, out status))
                            status = Status.ACTIVE;
                    }

                    var entity = new MaintenanceStageDetail
                    {
                        Id = Guid.NewGuid(),
                        MaintenanceStageId = stageId,
                        PartId = partId,
                        ActionType = actions.ToArray(),
                        Description = desc,
                        Status = status,
                    };

                    list.Add(entity);
                }

                if (list.Count == 0)
                    throw new AppException(
                        "Không có dòng hợp lệ nào trong file CSV.",
                        HttpStatusCode.BadRequest
                    );

                foreach (var d in list)
                    await _unitOfWork.MaintenanceStageDetails.CreateAsync(d);

                await _unitOfWork.SaveAsync();

                _logger.LogInformation(
                    "Import {Count} MaintenanceStageDetail từ CSV thành công.",
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
                    "ImportFromCsvAsync(MaintenanceStageDetail) failed: {Message}",
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

                var items = JsonSerializer.Deserialize<List<MaintenanceStageDetailRequest>>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (items == null || items.Count == 0)
                    throw new AppException(
                        "File JSON không chứa dữ liệu.",
                        HttpStatusCode.BadRequest
                    );

                var list = new List<MaintenanceStageDetail>();

                foreach (var req in items)
                {
                    var entity = _mapper.Map<MaintenanceStageDetail>(req);
                    entity.Id = Guid.NewGuid();
                    entity.Status = Status.ACTIVE; // default
                    list.Add(entity);
                }

                foreach (var d in list)
                    await _unitOfWork.MaintenanceStageDetails.CreateAsync(d);

                await _unitOfWork.SaveAsync();

                _logger.LogInformation(
                    "Import {Count} MaintenanceStageDetail từ JSON thành công.",
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
                    "ImportFromJsonAsync(MaintenanceStageDetail) failed: {Message}",
                    ex.Message
                );
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
