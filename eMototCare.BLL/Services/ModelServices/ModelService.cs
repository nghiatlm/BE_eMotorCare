using System.Net;
using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using eMototCare.BLL.Services.FirebaseServices;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.ModelServices
{
    public class ModelService : IModelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ModelService> _logger;
        private readonly IFirebaseService _firebase;

        public ModelService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ModelService> logger,
            IFirebaseService firebase
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _firebase = firebase;
        }

        public async Task<PageResult<ModelResponse>> GetPagedAsync(
            string? search,
            Status? status,
            Guid? modelId,
            Guid? maintenancePlanId,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.Models.GetPagedAsync(
                    search,
                    status,
                    modelId,
                    maintenancePlanId,
                    page,
                    pageSize
                );
                if (page <= 0)
                    throw new AppException("Page phải > 0", HttpStatusCode.BadRequest);

                if (pageSize <= 0)
                    throw new AppException("PageSize phải > 0", HttpStatusCode.BadRequest);

                if (modelId.HasValue && modelId.Value == Guid.Empty)
                    throw new AppException("ModelId không hợp lệ", HttpStatusCode.BadRequest);

                if (maintenancePlanId.HasValue && maintenancePlanId.Value == Guid.Empty)
                    throw new AppException(
                        "MaintenancePlanId không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (status.HasValue && !Enum.IsDefined(typeof(Status), status.Value))
                    throw new AppException(
                        "Trạng thái model không hợp lệ",
                        HttpStatusCode.BadRequest
                    );
                var rows = _mapper.Map<List<ModelResponse>>(items);
                return new PageResult<ModelResponse>(rows, pageSize, page, (int)total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Model failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ModelResponse> GetByIdAsync(Guid id)
        {
            var model = await _unitOfWork.Models.GetByIdAsync(id);
            if (id == Guid.Empty)
                throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);

            if (model == null)
                throw new AppException("Không tìm thấy Model", HttpStatusCode.NotFound);

            return _mapper.Map<ModelResponse>(model);
        }

        public async Task<Guid> CreateAsync(ModelRequest req)
        {
            try
            {
                var plan =
                    await _unitOfWork.MaintenancePlans.GetByIdAsync(req.MaintenancePlanId)
                    ?? throw new AppException(
                        "Không tìm thấy Maintenance Plan",
                        HttpStatusCode.BadRequest
                    );

                if (await _unitOfWork.Models.ExistsNameAsync(req.Name))
                    throw new AppException("Tên Model đã tồn tại.", HttpStatusCode.Conflict);
                if (req == null)
                    throw new AppException("Request không được null", HttpStatusCode.BadRequest);

                var name = (req.Name ?? string.Empty).Trim();
                var manufacturer = (req.Manufacturer ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(name))
                    throw new AppException(
                        "Tên Model không được để trống",
                        HttpStatusCode.BadRequest
                    );

                if (string.IsNullOrWhiteSpace(manufacturer))
                    throw new AppException(
                        "Hãng sản xuất không được để trống",
                        HttpStatusCode.BadRequest
                    );

                if (req.MaintenancePlanId == Guid.Empty)
                    throw new AppException(
                        "MaintenancePlanId không hợp lệ",
                        HttpStatusCode.BadRequest
                    );
                var entity = _mapper.Map<Model>(req);
                entity.Id = Guid.NewGuid();
                entity.Code = await GenerateCodeAsync();
                entity.Status = Status.ACTIVE;

                await _unitOfWork.Models.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created Model {Code} ({Id})", entity.Code, entity.Id);
                return entity.Id;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Model failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, ModelUpdateRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.Models.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy Model", HttpStatusCode.NotFound);
                if (id == Guid.Empty)
                    throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);

                if (req == null)
                    throw new AppException("Request không được null", HttpStatusCode.BadRequest);

                if (req.MaintenancePlanId.HasValue && req.MaintenancePlanId.Value == Guid.Empty)
                    throw new AppException(
                        "MaintenancePlanId không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (req.Status.HasValue && !Enum.IsDefined(typeof(Status), req.Status.Value))
                    throw new AppException(
                        "Trạng thái model không hợp lệ",
                        HttpStatusCode.BadRequest
                    );
                if (req.MaintenancePlanId.HasValue)
                {
                    var plan =
                        await _unitOfWork.MaintenancePlans.GetByIdAsync(req.MaintenancePlanId.Value)
                        ?? throw new AppException(
                            "Không tìm thấy Maintenance Plan",
                            HttpStatusCode.BadRequest
                        );
                }

                if (
                    !string.IsNullOrWhiteSpace(req.Name)
                    && await _unitOfWork.Models.ExistsNameAsync(req.Name, id)
                )
                    throw new AppException("Tên Model đã tồn tại.", HttpStatusCode.Conflict);

                entity.Name = req.Name?.Trim() ?? entity.Name;
                entity.Manufacturer = req.Manufacturer?.Trim() ?? entity.Manufacturer;
                entity.MaintenancePlanId = req.MaintenancePlanId ?? entity.MaintenancePlanId;
                entity.Status = req.Status ?? entity.Status;

                await _unitOfWork.Models.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated Model {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Model failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.Models.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy Model", HttpStatusCode.NotFound);
                if (id == Guid.Empty)
                    throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);
                if (entity.Status == Status.IN_ACTIVE)
                {
                    throw new AppException(
                        "Model này đã bị vô hiệu hoá rồi.",
                        HttpStatusCode.Conflict
                    );
                }

                entity.Status = Status.IN_ACTIVE;

                await _unitOfWork.Models.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Soft-deleted Model {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete Model failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        private async Task<string> GenerateCodeAsync()
        {
            const string prefix = "MDL-";
            var rnd = new Random();
            int guard = 0;
            string code;
            do
            {
                var suffix = rnd.Next(0, 99999).ToString("D5");
                code = prefix + suffix;
                guard++;
            } while (await _unitOfWork.Models.ExistsCodeAsync(code) && guard < 10);

            return code;
        }

        public async Task<ModelResponse> SyncModelAsync(SyncModelRequest request)
        {
            try
            {
                if (request == null)
                    throw new AppException("Request không được null", HttpStatusCode.BadRequest);

                var value = request.modelIdOrName?.Trim();
                if (string.IsNullOrWhiteSpace(value))
                    throw new AppException(
                        "Giá trị đồng bộ (id hoặc name) không được để trống",
                        HttpStatusCode.BadRequest
                    );

                if (!_firebase.IsFirestoreConfigured())
                    throw new AppException(
                        "Hệ thống chưa được cấu hình Firestore",
                        HttpStatusCode.ServiceUnavailable
                    );

                // 1. Lấy model từ Firebase (theo id hoặc name)
                var data = await _firebase.GetModelByIdAsync(value);
                if (data == null)
                    throw new AppException(
                        "Không tìm thấy model trong hệ thống OEM",
                        HttpStatusCode.NotFound
                    );

                var name = data.ContainsKey("name") ? data["name"]?.ToString()?.Trim() ?? "" : "";
                var manufacturer = data.ContainsKey("manufacturer")
                    ? data["manufacturer"]?.ToString()?.Trim() ?? ""
                    : "";
                var maintenancePlanIdStr = data.ContainsKey("maintenancePlanId")
                    ? data["maintenancePlanId"]?.ToString()
                    : null;

                if (string.IsNullOrWhiteSpace(name))
                    throw new AppException(
                        "Tên model từ OEM không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                // 2. Nếu model đã tồn tại trong DB (theo Name) -> trả về luôn
                var existed = _unitOfWork
                    .Models.FindAll()
                    .FirstOrDefault(m => m.Name.ToLower() == name.ToLower());

                if (existed != null)
                    return _mapper.Map<ModelResponse>(existed);

                if (string.IsNullOrWhiteSpace(manufacturer))
                    throw new AppException(
                        "Hãng sản xuất từ OEM không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (!Guid.TryParse(maintenancePlanIdStr, out var maintenancePlanId))
                    throw new AppException(
                        "MaintenancePlanId từ OEM không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                // 3. Đảm bảo MaintenancePlan tồn tại trong DB
                var plan = await _unitOfWork.MaintenancePlans.GetByIdAsync(maintenancePlanId);
                if (plan == null)
                {
                    // 3.1. Nếu chưa có -> lấy từ Firestore và lưu xuống DB
                    var planData = await _firebase.GetMaintenancePlanByIdAsync(
                        maintenancePlanIdStr!
                    );
                    if (planData == null)
                        throw new AppException(
                            "Không tìm thấy MaintenancePlan trên hệ thống OEM",
                            HttpStatusCode.BadRequest
                        );

                    var planCode = planData.ContainsKey("code")
                        ? planData["code"]?.ToString()?.Trim() ?? ""
                        : "";
                    var planName = planData.ContainsKey("name")
                        ? planData["name"]?.ToString()?.Trim() ?? ""
                        : "";
                    var planDescription = planData.ContainsKey("description")
                        ? planData["description"]?.ToString() ?? ""
                        : "";

                    Status planStatus = Status.ACTIVE;
                    if (
                        planData.ContainsKey("status")
                        && Enum.TryParse<Status>(planData["status"]?.ToString(), true, out var st)
                    )
                    {
                        planStatus = st;
                    }

                    var unit = planData.ContainsKey("unit")
                        ? planData["unit"]?.ToString()?.Trim() ?? ""
                        : "";
                    if (string.IsNullOrWhiteSpace(unit))
                        unit = "KILOMETER";

                    int totalStages = 0;
                    if (planData.ContainsKey("totalStages"))
                    {
                        var tsStr = planData["totalStages"]?.ToString();
                        if (!string.IsNullOrWhiteSpace(tsStr))
                            int.TryParse(tsStr, out totalStages);
                    }

                    DateTime? effectiveDate = null;
                    if (
                        planData.ContainsKey("effectiveDate")
                        && DateTime.TryParse(planData["effectiveDate"]?.ToString(), out var eff)
                    )
                    {
                        effectiveDate = eff;
                    }

                    plan = new MaintenancePlan
                    {
                        Id = maintenancePlanId,
                        Code = planCode,
                        Name = planName,
                        Description = planDescription,
                        Status = planStatus,
                        Unit = new[]
                        {
                            Enum.TryParse<MaintenanceUnit>(unit, true, out var parsedUnit)
                                ? parsedUnit
                                : MaintenanceUnit.KILOMETER,
                        },
                        TotalStages = totalStages,
                        EffectiveDate = effectiveDate ?? DateTime.MinValue,
                    };

                    await _unitOfWork.MaintenancePlans.CreateAsync(plan);
                    await _unitOfWork.SaveAsync();
                }

                // 4. Tạo mới Model trỏ tới MaintenancePlan trong DB
                var entity = new Model
                {
                    Id = Guid.NewGuid(),
                    Code = await GenerateCodeAsync(),
                    Name = name,
                    Manufacturer = manufacturer,
                    MaintenancePlanId = maintenancePlanId,
                    Status = Status.ACTIVE,
                };

                await _unitOfWork.Models.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                return _mapper.Map<ModelResponse>(entity);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sync Model failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
