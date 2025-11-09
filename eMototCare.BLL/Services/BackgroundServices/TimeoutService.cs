using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.BackgroundServices
{
    public class TimeoutService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TimeoutService> _logger;
        private static readonly TimeSpan Period = TimeSpan.FromMinutes(10);
        private const string TimeZoneId = "SE Asia Standard Time";

        public TimeoutService(IServiceScopeFactory scopeFactory, ILogger<TimeoutService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("TimeoutService (VehicleStage expiry) started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await RunVehicleStageExpiryAsync(stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in TimeoutService while expiring VehicleStages.");
                }

                try
                {
                    await Task.Delay(Period, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }

            _logger.LogInformation("TimeoutService (VehicleStage expiry) stopped.");
        }

        private async Task RunVehicleStageExpiryAsync(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Xác định mốc “đầu ngày hôm nay” (theo UTC hoặc timezone cấu hình)
            DateTime today = string.IsNullOrWhiteSpace(TimeZoneId)
                ? DateTime.UtcNow.Date
                : TimeZoneInfo
                    .ConvertTimeFromUtc(
                        DateTime.UtcNow,
                        TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId!)
                    )
                    .Date;

            // Điều kiện:
            // - DateOfImplementation < hôm nay
            // - Status chưa phải COMPLETED/EXPIRED (tuỳ nghi có thể ràng NO_START/UPCOMING)
            var affected = await db
                .VehicleStages.Where(vs =>
                    vs.Status != VehicleStageStatus.COMPLETED
                    && vs.Status != VehicleStageStatus.EXPIRED
                    && vs.DateOfImplementation < today
                )
                .ExecuteUpdateAsync(
                    s => s.SetProperty(vs => vs.Status, VehicleStageStatus.EXPIRED),
                    ct
                );

            if (affected > 0)
            {
                _logger.LogInformation(
                    "Expired {Count} vehicle stages (date_of_implementation < {Today}).",
                    affected,
                    today.ToString("yyyy-MM-dd")
                );
            }
        }
    }
}
