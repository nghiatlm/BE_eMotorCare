using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;
using AutoMapper;
using eMotoCare.BO.Common.AISettings;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace eMototCare.BLL.Services.BatteryCheckServices
{
    public class BatteryCheckService : IBatteryCheckService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BatteryCheckService> _logger;
        private readonly AiSettings _aiSettings;
        private readonly IMapper _mapper;
        public BatteryCheckService(
            IUnitOfWork unitOfWork,
            ILogger<BatteryCheckService> logger,
            IOptions<AiSettings> aiOptions,
            IMapper mapper
        )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _aiSettings = aiOptions.Value;
            _mapper = mapper;
        }

        public async Task<BatteryCheckAnalysisResponse> ImportFromCsvAsync(
            Guid evCheckDetailId,
            Stream fileStream,
            CancellationToken ct = default
        )
        {
            try
            {
                var existed = await _unitOfWork.BatteryChecks.GetByEVCheckDetailIdAsync(
                    evCheckDetailId
                );

                if (existed != null)
                {
                    throw new AppException(
                        "Đã tồn tại dữ liệu Battery cho EVCheckDetail này. Vui lòng xóa/cập nhật thay vì import lại.",
                        HttpStatusCode.Conflict
                    );
                }
                if (fileStream == null)
                    throw new AppException(
                        "File upload không được null.",
                        HttpStatusCode.BadRequest
                    );

                if (!fileStream.CanRead)
                    throw new AppException("Không thể đọc file upload.", HttpStatusCode.BadRequest);
                if (evCheckDetailId == Guid.Empty)
                    throw new AppException(
                        "EVCheckDetailId không hợp lệ",
                        HttpStatusCode.BadRequest
                    );
                if (fileStream.CanSeek)
                {
                    if (fileStream.Length == 0)
                        throw new AppException("File upload đang rỗng.", HttpStatusCode.BadRequest);

                    fileStream.Position = 0;
                }

                var evDetail = await _unitOfWork.EVCheckDetails.GetByIdAsync(evCheckDetailId);
                if (evDetail == null)
                    throw new AppException("Không tìm thấy EVCheckDetail", HttpStatusCode.NotFound);
                var times = new List<int>();
                var voltages = new List<float>();
                var currents = new List<float>();
                var powers = new List<float>();
                var capacities = new List<float>();
                var energies = new List<float>();
                var temps = new List<float>();
                var socs = new List<float>();
                var sohs = new List<float>();

                using (var reader = new StreamReader(fileStream))
                {
                    string? line;
                    bool isHeader = true;

                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if (isHeader)
                        {
                            isHeader = false;
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        var p = line.Split(',');
                        if (p.Length < 9)
                            continue;

                        times.Add(int.Parse(p[0], CultureInfo.InvariantCulture));
                        voltages.Add(float.Parse(p[1], CultureInfo.InvariantCulture));
                        currents.Add(float.Parse(p[2], CultureInfo.InvariantCulture));
                        powers.Add(float.Parse(p[3], CultureInfo.InvariantCulture));
                        capacities.Add(float.Parse(p[4], CultureInfo.InvariantCulture));
                        energies.Add(float.Parse(p[5], CultureInfo.InvariantCulture));
                        temps.Add(float.Parse(p[6], CultureInfo.InvariantCulture));
                        socs.Add(float.Parse(p[7], CultureInfo.InvariantCulture));
                        sohs.Add(float.Parse(p[8], CultureInfo.InvariantCulture));
                    }
                }

                if (!times.Any())
                    throw new AppException(
                        "File không có dữ liệu battery hợp lệ.",
                        HttpStatusCode.BadRequest
                    );
                var entity = new BatteryCheck
                {
                    Id = Guid.NewGuid(),
                    EVCheckDetailId = evCheckDetailId,
                    Time = times.ToArray(),
                    Voltage = voltages.ToArray(),
                    current = currents.ToArray(),
                    Power = powers.ToArray(),
                    Capacity = capacities.ToArray(),
                    Energy = energies.ToArray(),
                    Temp = temps.ToArray(),
                    SOC = socs.ToArray(),
                    SOH = sohs.ToArray(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    EnergyCapability = null,
                    ChargeDischargeEfficiency = null,
                    DegradationStatus = null,
                    RemainingUsefulLife = null,
                    Safety = null,
                };

                await _unitOfWork.BatteryChecks.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                var analysis = await BuildAnalysisAsync(entity, ct);

                _logger.LogInformation(
                    "Imported BatteryCheck {Id} for EVCheckDetail {DetailId}",
                    entity.Id,
                    entity.EVCheckDetailId
                );

                return analysis;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "ImportFromCsvAsync BatteryCheck failed: {Message}",
                    ex.Message
                );
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        //phân tích
        private async Task<BatteryCheckAnalysisResponse> BuildAnalysisAsync(
            BatteryCheck entity,
            CancellationToken ct
        )
        {
            int n = entity.Time.Length;

            float minVoltage = entity.Voltage.Min();
            float maxVoltage = entity.Voltage.Max();
            float avgVoltage = entity.Voltage.Average();

            float minCurrent = entity.current.Min();
            float maxCurrent = entity.current.Max();
            float avgCurrent = entity.current.Average();

            float minTemp = entity.Temp.Min();
            float maxTemp = entity.Temp.Max();
            float avgTemp = entity.Temp.Average();

            float minSoc = entity.SOC.Min();
            float maxSoc = entity.SOC.Max();
            float avgSoc = entity.SOC.Average();

            float minSoh = entity.SOH.Min();
            float maxSoh = entity.SOH.Max();
            float avgSoh = entity.SOH.Average();

            var prompt = BuildBatteryPrompt(
                n,
                minVoltage,
                maxVoltage,
                avgVoltage,
                minCurrent,
                maxCurrent,
                avgCurrent,
                minTemp,
                maxTemp,
                avgTemp,
                minSoc,
                maxSoc,
                avgSoc,
                minSoh,
                maxSoh,
                avgSoh
            );

            string? aiJson = null;

            try
            {
                var provider = _aiSettings.Provider?.ToLowerInvariant();
                string rawText;

                if (provider == "gemini")
                {
                    rawText = await CallGeminiAsync(prompt, ct);
                }
                else if (provider == "openai")
                {
                    rawText = await CallOpenAiAsync(prompt, ct);
                }
                else
                {
                    _logger.LogWarning(
                        "Provider AI không hợp lệ: {Provider}",
                        _aiSettings.Provider
                    );
                    rawText = string.Empty;
                }
                aiJson = ExtractJson(rawText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AI phân tích battery thất bại, dùng rule fallback.");
            }
            if (string.IsNullOrWhiteSpace(aiJson))
            {
                aiJson = BuildFallbackSolutionJson(
                    minVoltage,
                    maxVoltage,
                    avgVoltage,
                    minCurrent,
                    maxCurrent,
                    avgCurrent,
                    minTemp,
                    maxTemp,
                    avgTemp,
                    minSoc,
                    maxSoc,
                    avgSoc,
                    minSoh,
                    maxSoh,
                    avgSoh
                );
            }
            BatteryConclusionResponse? conclusionObj = null;
            try
            {
                conclusionObj = JsonSerializer.Deserialize<BatteryConclusionResponse>(aiJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Deserialize kết quả AI BatteryCheck thất bại. aiJson = {Json}",
                    aiJson
                );
            }

            if (conclusionObj != null)
            {
                entity.EnergyCapability = JsonSerializer.Serialize(conclusionObj.energyCapability);
                entity.ChargeDischargeEfficiency = JsonSerializer.Serialize(
                    conclusionObj.chargeDischargeEfficiency
                );
                entity.DegradationStatus = JsonSerializer.Serialize(
                    conclusionObj.degradationStatus
                );
                entity.RemainingUsefulLife = JsonSerializer.Serialize(
                    conclusionObj.remainingUsefulLife
                );
                entity.Safety = JsonSerializer.Serialize(conclusionObj.safety);

                var solution = conclusionObj.solution;

                if (string.IsNullOrWhiteSpace(solution))
                    solution = "Bảo hành hoặc thay thế";

                entity.Solution =
                    solution == "Bình thường" ? "Bình thường" : "Bảo hành hoặc thay thế";
            }
            else
            {
                _logger.LogWarning("Không thể parse JSON AI — Solution fallback.");
                entity.Solution = "Bảo hành hoặc thay thế";
            }

            await _unitOfWork.BatteryChecks.UpdateAsync(entity);
            await _unitOfWork.SaveAsync();

            return new BatteryCheckAnalysisResponse
            {
                Id = entity.Id,
                EVCheckDetailId = entity.EVCheckDetailId,
                SampleCount = n,

                MinVoltage = minVoltage,
                MaxVoltage = maxVoltage,
                AvgVoltage = avgVoltage,

                MinCurrent = minCurrent,
                MaxCurrent = maxCurrent,
                AvgCurrent = avgCurrent,

                MinTemp = minTemp,
                MaxTemp = maxTemp,
                AvgTemp = avgTemp,

                MinSOC = minSoc,
                MaxSOC = maxSoc,
                AvgSOC = avgSoc,

                MinSOH = minSoh,
                MaxSOH = maxSoh,
                AvgSOH = avgSoh,

                Conclusion = conclusionObj,
            };
        }

        //dự phòng khi AI lỗi
        private string BuildFallbackSolutionJson(
            float minVoltage,
            float maxVoltage,
            float avgVoltage,
            float minCurrent,
            float maxCurrent,
            float avgCurrent,
            float minTemp,
            float maxTemp,
            float avgTemp,
            float minSoc,
            float maxSoc,
            float avgSoc,
            float minSoh,
            float maxSoh,
            float avgSoh
        )
        {
            var energyCapability =
                avgSoc >= 80
                    ? "Mức SOC trung bình cao, pin vẫn cung cấp năng lượng tốt cho nhu cầu di chuyển hàng ngày."
                    : "Mức SOC trung bình thấp, pin có dấu hiệu suy giảm khả năng cung cấp năng lượng.";

            var chargeDischargeEfficiency =
                maxCurrent > 0 && maxVoltage > 0
                    ? "Dòng và áp tương đối ổn định, không ghi nhận biến thiên bất thường lớn, hiệu suất nạp/xả ở mức chấp nhận được."
                    : "Dữ liệu dòng/áp chưa đủ rõ để đánh giá chính xác hiệu suất nạp/xả.";

            var degradationStatus =
                avgSoh >= 80
                    ? $"SOH trung bình khoảng {avgSoh:F1}%, pin chỉ lão hoá nhẹ."
                    : $"SOH trung bình khoảng {avgSoh:F1}%, pin đã suy giảm đáng kể, nên theo dõi sát và cân nhắc phương án thay thế.";

            var remainingUsefulLife =
                avgSoh >= 80
                    ? "Tuổi thọ còn lại ước tính vẫn còn tương đối tốt, có thể sử dụng thêm một thời gian nữa trước khi cần thay pin."
                    : "Tuổi thọ còn lại không còn nhiều, nên lập kế hoạch thay pin trong thời gian tới để tránh sự cố ngoài ý muốn.";

            var safety =
                maxTemp > 60
                    ? $"Nhiệt độ cực đại đạt khoảng {maxTemp:F1}°C, vượt ngưỡng khuyến nghị, cần kiểm tra hệ thống tản nhiệt và cách sử dụng."
                    : $"Nhiệt độ hoạt động nằm trong vùng an toàn (tối đa {maxTemp:F1}°C), chưa ghi nhận nguy cơ mất an toàn rõ rệt.";

            var obj = new
            {
                energyCapability,
                chargeDischargeEfficiency,
                degradationStatus,
                remainingUsefulLife,
                safety,
            };

            return JsonSerializer.Serialize(obj);
        }

        //prompt
        private string BuildBatteryPrompt(
            int sampleCount,
            float minVoltage,
            float maxVoltage,
            float avgVoltage,
            float minCurrent,
            float maxCurrent,
            float avgCurrent,
            float minTemp,
            float maxTemp,
            float avgTemp,
            float minSoc,
            float maxSoc,
            float avgSoc,
            float minSoh,
            float maxSoh,
            float avgSoh
        )
        {
            return $@"
                     Bạn là kỹ sư chẩn đoán pin xe máy điện.

                     Dữ liệu kiểm tra (đã tổng hợp):
                     • Số mẫu: {sampleCount}

                     Điện áp (V):
                     - Min: {minVoltage:F2}
                     - Max: {maxVoltage:F2}
                     - Avg: {avgVoltage:F2}

                     Dòng điện (A):
                     - Min: {minCurrent:F2}
                     - Max: {maxCurrent:F2}
                     - Avg: {avgCurrent:F2}

                     Nhiệt độ (°C):
                     - Min: {minTemp:F2}
                     - Max: {maxTemp:F2}
                     - Avg: {avgTemp:F2}

                     SOC (% pin hiện tại):
                     - Min: {minSoc:F2}
                     - Max: {maxSoc:F2}
                     - Avg: {avgSoc:F2}

                     SOH (% sức khỏe pin):
                     - Min: {minSoh:F2}
                     - Max: {maxSoh:F2}
                     - Avg: {avgSoh:F2}

                     Hãy phân tích **theo đúng 5 tiêu chí dưới đây**, viết rõ 5 đoạn, mỗi đoạn 1–3 câu:

                      1) Khả năng cung cấp năng lượng hiện tại  
                         - Dựa vào Capacity/Ah, Energy/Wh, SOC %, so sánh với giá trị danh định (đánh giá pin còn khỏe hay suy).  

                      2) Hiệu suất nạp/xả  
                         - Dựa vào dòng/áp/power, đánh giá hiệu suất nạp/xả, có thất thoát năng lượng không.  

                      3) Tình trạng xuống cấp (SOH, nội trở, số chu kỳ…)  
                         - Đánh giá mức độ hao mòn của pin dựa vào SOH & hành vi điện áp/dòng.  

                      4) Dự báo tuổi thọ còn lại (RUL – Remaining Useful Life)  
                         - Ước lượng pin còn dùng được bao lâu dựa theo xu hướng SOH.  

                      5) Khả năng an toàn  
                         - Đánh giá nguy cơ quá nhiệt, sụt áp bất thường hoặc cell lỗi.

                      Sau đó hãy đưa ra **kết luận tổng thể** về tình trạng pin theo 2 mức:
                         - ""Bình thường""
                         - hoặc ""Bảo hành hoặc thay thế""
                      Trả lời theo mẫu JSON sau, KHÔNG thêm trường khác:
                         {{
                           ""energyCapability"": ""..."",
                           ""chargeDischargeEfficiency"": ""..."",
                           ""degradationStatus"": ""..."",
                           ""remainingUsefulLife"": ""..."",
                           ""safety"": ""...""
                           ""solution"": ""Bình thường"" hoặc ""Bảo hành hoặc thay thế""
                         }}

                     Lưu ý:
                        - Bắt buộc trả về JSON hợp lệ.
                        - Trường ""solution"" CHỈ ĐƯỢC nhận một trong hai giá trị chính xác:
                          ""Bình thường"" hoặc ""Bảo hành hoặc thay thế"".
                        - Trả lời tiếng Việt, dễ hiểu, tập trung vào ý chính.";
        }

        //gọi API OpenAi
        private async Task<string> CallOpenAiAsync(string prompt, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(_aiSettings.OpenAIApiKey))
                throw new InvalidOperationException("Chưa cấu hình OpenAIApiKey.");

            using var http = new HttpClient();
            http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer",
                    _aiSettings.OpenAIApiKey
                );

            var payload = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "system", content = "Bạn là kỹ sư pin EV." },
                    new { role = "user", content = prompt },
                },
            };

            var json = JsonSerializer.Serialize(payload);
            var response = await http.PostAsync(
                "https://api.openai.com/v1/chat/completions",
                new StringContent(json, Encoding.UTF8, "application/json"),
                ct
            );

            var result = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("OpenAI API lỗi {Status}: {Body}", response.StatusCode, result);
                throw new HttpRequestException($"OpenAI API error {(int)response.StatusCode}");
            }

            using var doc = JsonDocument.Parse(result);

            return doc.RootElement.GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString() ?? "Không có phản hồi từ OpenAI.";
        }

        //gọi API Gemini
        private async Task<string> CallGeminiAsync(string prompt, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(_aiSettings.GeminiApiKey))
                throw new InvalidOperationException("Chưa cấu hình GeminiApiKey.");

            using var http = new HttpClient();

            var payload = new
            {
                contents = new[] { new { parts = new[] { new { text = prompt } } } },
            };

            var json = JsonSerializer.Serialize(payload);

            const string model = "gemini-2.5-flash";

            var url =
                $"https://generativelanguage.googleapis.com/v1/models/{model}:generateContent?key={_aiSettings.GeminiApiKey}";

            var response = await http.PostAsync(
                url,
                new StringContent(json, Encoding.UTF8, "application/json"),
                ct
            );

            var result = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Gemini API lỗi {Status}: {Body}", response.StatusCode, result);
                throw new HttpRequestException($"Gemini API error {(int)response.StatusCode}");
            }

            using var doc = JsonDocument.Parse(result);

            return doc
                .RootElement.GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString()!;
        }

        private static string? ExtractJson(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return null;
            raw = raw.Trim();
            if (raw.StartsWith("{") && raw.EndsWith("}"))
                return raw;
            var start = raw.IndexOf('{');
            var end = raw.LastIndexOf('}');
            if (start >= 0 && end > start)
            {
                return raw.Substring(start, end - start + 1);
            }

            return null;
        }

        public async Task<BatteryCheckAnalysisResponse> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new AppException("BatteryCheckId không hợp lệ.", HttpStatusCode.BadRequest);
            var entity =
                await _unitOfWork.BatteryChecks.GetByIdAsync(id)
                ?? throw new AppException("Không tìm thấy BatteryCheck", HttpStatusCode.NotFound);

            return BuildSummaryFromEntity(entity);
        }

        public async Task<PageResult<BatteryCheckAnalysisResponse>> GetPagedAsync(
            Guid? evCheckDetailId,
            DateTime? fromDate,
            DateTime? toDate,
            string? sortBy,
            bool sortDesc,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.BatteryChecks.GetPagedAsync(
                    evCheckDetailId,
                    fromDate,
                    toDate,
                    sortBy,
                    sortDesc,
                    page,
                    pageSize
                );

                if (evCheckDetailId.HasValue && evCheckDetailId.Value == Guid.Empty)
                    throw new AppException(
                        "EVCheckDetailId không hợp lệ.",
                        HttpStatusCode.BadRequest
                    );

                if (fromDate.HasValue && toDate.HasValue && fromDate > toDate)
                    throw new AppException(
                        "Từ ngày (fromDate) phải nhỏ hơn hoặc bằng đến ngày (toDate).",
                        HttpStatusCode.BadRequest
                    );

                if (page <= 0)
                    throw new AppException("Page phải >= 1.", HttpStatusCode.BadRequest);

                if (pageSize <= 0 || pageSize > 200)
                    throw new AppException(
                        "PageSize phải trong khoảng 1–200.",
                        HttpStatusCode.BadRequest
                    );

                var allowedSort = new[] { "createdat", "updatedat" };
                var sortKey = (sortBy ?? "createdAt").Trim().ToLowerInvariant();

                if (!allowedSort.Contains(sortKey))
                    throw new AppException(
                        "sortBy không hợp lệ. Chỉ hỗ trợ: createdAt, updatedAt.",
                        HttpStatusCode.BadRequest
                    );
                var rows = items.Select(BuildSummaryFromEntity).ToList();

                return new PageResult<BatteryCheckAnalysisResponse>(
                    rows,
                    pageSize,
                    page,
                    (int)total
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged BatteryCheck failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        private static string NormalizeText(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            value = value.Trim();
            if (value.Length >= 2 && value[0] == '"' && value[^1] == '"')
            {
                try
                {
                    var inner = JsonSerializer.Deserialize<string>(value);
                    if (!string.IsNullOrWhiteSpace(inner))
                        return inner;
                }
                catch { }
            }

            return value;
        }

        private BatteryCheckAnalysisResponse BuildSummaryFromEntity(BatteryCheck entity)
        {
            int n = entity.Time.Length;

            float minVoltage = entity.Voltage.Min();
            float maxVoltage = entity.Voltage.Max();
            float avgVoltage = entity.Voltage.Average();

            float minCurrent = entity.current.Min();
            float maxCurrent = entity.current.Max();
            float avgCurrent = entity.current.Average();

            float minTemp = entity.Temp.Min();
            float maxTemp = entity.Temp.Max();
            float avgTemp = entity.Temp.Average();

            float minSoc = entity.SOC.Min();
            float maxSoc = entity.SOC.Max();
            float avgSoc = entity.SOC.Average();

            float minSoh = entity.SOH.Min();
            float maxSoh = entity.SOH.Max();
            float avgSoh = entity.SOH.Average();

            var vehicleEntity = entity.EVCheckDetail?.EVCheck?.Appointment?.Vehicle;
            var vehicleDto = vehicleEntity != null
                ? _mapper.Map<VehicleResponse>(vehicleEntity)
                : null;

            return new BatteryCheckAnalysisResponse
            {
                Id = entity.Id,
                EVCheckDetailId = entity.EVCheckDetailId,
                SampleCount = n,

                MinVoltage = minVoltage,
                MaxVoltage = maxVoltage,
                AvgVoltage = avgVoltage,

                MinCurrent = minCurrent,
                MaxCurrent = maxCurrent,
                AvgCurrent = avgCurrent,

                MinTemp = minTemp,
                MaxTemp = maxTemp,
                AvgTemp = avgTemp,

                MinSOC = minSoc,
                MaxSOC = maxSoc,
                AvgSOC = avgSoc,

                MinSOH = minSoh,
                MaxSOH = maxSoh,
                AvgSOH = avgSoh,

                Conclusion = new BatteryConclusionResponse
                {
                    energyCapability = NormalizeText(entity.EnergyCapability),
                    chargeDischargeEfficiency = NormalizeText(entity.ChargeDischargeEfficiency),
                    degradationStatus = NormalizeText(entity.DegradationStatus),
                    remainingUsefulLife = NormalizeText(entity.RemainingUsefulLife),
                    safety = NormalizeText(entity.Safety),
                    solution = entity.Solution ?? string.Empty,
                },
                Vehicle = vehicleDto,
            };
        }
    }
}
