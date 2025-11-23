using System.Globalization;
using System.Net;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Exceptions;
using eMotoCare.DAL;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.BatteryCheckServices
{
    public class BatteryCheckService : IBatteryCheckService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BatteryCheckService> _logger;

        public BatteryCheckService(IUnitOfWork unitOfWork, ILogger<BatteryCheckService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
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

                // Đảm bảo EVCheckDetail tồn tại
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
                        // Bỏ qua dòng header
                        if (isHeader)
                        {
                            isHeader = false;
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        var parts = line.Split(',');
                        if (parts.Length < 9)
                            continue; // hoặc throw nếu cần

                        // parse với InvariantCulture để chấm/phẩy chuẩn
                        int time = int.Parse(parts[0], CultureInfo.InvariantCulture);
                        float voltage = float.Parse(parts[1], CultureInfo.InvariantCulture);
                        float current = float.Parse(parts[2], CultureInfo.InvariantCulture);
                        float power = float.Parse(parts[3], CultureInfo.InvariantCulture);
                        float capacity = float.Parse(parts[4], CultureInfo.InvariantCulture);
                        float energy = float.Parse(parts[5], CultureInfo.InvariantCulture);
                        float temp = float.Parse(parts[6], CultureInfo.InvariantCulture);
                        float soc = float.Parse(parts[7], CultureInfo.InvariantCulture);
                        float soh = float.Parse(parts[8], CultureInfo.InvariantCulture);

                        times.Add(time);
                        voltages.Add(voltage);
                        currents.Add(current);
                        powers.Add(power);
                        capacities.Add(capacity);
                        energies.Add(energy);
                        temps.Add(temp);
                        socs.Add(soc);
                        sohs.Add(soh);
                    }
                }

                if (times.Count == 0)
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

                // Phân tích dữ liệu
                var analysis = BuildAnalysis(entity);

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

        private BatteryCheckAnalysisResponse BuildAnalysis(BatteryCheck entity)
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

            // Rule đánh giá đơn giản, bạn có thể chỉnh tuỳ ý
            var conclusionParts = new List<string>();

            if (maxTemp > 60)
                conclusionParts.Add(
                    "Nhiệt độ cực đại cao, cần kiểm tra hệ thống làm mát/bố trí cell."
                );
            else
                conclusionParts.Add("Nhiệt độ hoạt động trong ngưỡng an toàn.");

            if (avgSoh < 80)
                conclusionParts.Add(
                    "SOH trung bình thấp (<80%), khuyến nghị kiểm tra tình trạng pin."
                );
            else
                conclusionParts.Add("SOH ở mức chấp nhận được.");

            if (minVoltage < 2.8f)
                conclusionParts.Add(
                    "Có thời điểm điện áp cell xuống quá thấp, nguy cơ suy giảm tuổi thọ."
                );

            var conclusion = string.Join(" ", conclusionParts);

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

                Conclusion = conclusion,
            };
        }
    }
}
