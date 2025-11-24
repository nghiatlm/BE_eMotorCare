using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;
using eMotoCare.BO.Common.AISettings;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Exceptions;
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

        public BatteryCheckService(
            IUnitOfWork unitOfWork,
            ILogger<BatteryCheckService> logger,
            IOptions<AiSettings> aiOptions
        )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _aiSettings = aiOptions.Value;
        }

        public async Task<BatteryCheckAnalysisResponse> ImportFromCsvAsync(
            Guid evCheckDetailId,
            Stream fileStream,
            CancellationToken ct = default
        )
        {
            try
            {
                if (evCheckDetailId == Guid.Empty)
                    throw new AppException(
                        "EVCheckDetailId không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

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

            string aiConclusion;

            try
            {
                var provider = _aiSettings.Provider?.ToLowerInvariant();

                if (provider == "gemini")
                {
                    aiConclusion = await CallGeminiAsync(prompt, ct);
                }
                else if (provider == "openai")
                {
                    aiConclusion = await CallOpenAiAsync(prompt, ct);
                }
                else
                {
                    _logger.LogWarning(
                        "Provider AI không hợp lệ: {Provider}",
                        _aiSettings.Provider
                    );
                    aiConclusion = BuildFallbackConclusion(
                        minVoltage,
                        maxVoltage,
                        minTemp,
                        maxTemp,
                        minSoh,
                        avgSoh
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AI phân tích battery thất bại, dùng rule fallback.");
                aiConclusion = BuildFallbackConclusion(
                    minVoltage,
                    maxVoltage,
                    minTemp,
                    maxTemp,
                    minSoh,
                    avgSoh
                );
            }

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

                Conclusion = aiConclusion,
            };
        }

        //dự phòng khi AI lỗi
        private string BuildFallbackConclusion(
            float minVoltage,
            float maxVoltage,
            float minTemp,
            float maxTemp,
            float minSoh,
            float avgSoh
        )
        {
            var list = new List<string>();

            if (maxTemp > 60)
                list.Add("Nhiệt độ cao vượt ngưỡng an toàn, cần kiểm tra hệ thống tản nhiệt.");
            else
                list.Add("Nhiệt độ pin hoạt động bình thường.");

            if (avgSoh < 80)
                list.Add("SOH trung bình thấp (<80%), pin có dấu hiệu suy giảm.");
            else
                list.Add("SOH ở mức ổn định.");

            if (minVoltage < 2.8f)
                list.Add("Có thời điểm điện áp thấp, nguy cơ giảm tuổi thọ cell.");

            return string.Join(" ", list);
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

                     Yêu cầu:
                       1) Đánh giá tình trạng pin tổng thể (tốt / suy giảm / kém).
                       2) Liệt kê các rủi ro chính dựa trên dữ liệu.
                       3) Đưa ra khuyến nghị cho kỹ thuật viên và khách hàng.

                     Hãy trả lời ngắn gọn (3–6 câu), tiếng Việt, dễ hiểu, tập trung vào ý chính.";
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
    }
}
