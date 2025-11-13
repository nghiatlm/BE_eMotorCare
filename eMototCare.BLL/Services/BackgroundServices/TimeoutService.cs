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
        private static readonly TimeSpan Period = TimeSpan.FromMinutes(1);
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
            var pastThreshold = today.AddDays(-10);
            var futureThreshold = today.AddDays(10);
            var activeStatuses = new[]
            {
                VehicleStageStatus.NO_START,
                VehicleStageStatus.UPCOMING,
                VehicleStageStatus.EXPIRED,
            };
            // 1) Set EXPIRED: quá 10 ngày trước today
            var expired = await db
                .VehicleStages.Where(vs =>
                    vs.Status != VehicleStageStatus.COMPLETED
                    && vs.Status != VehicleStageStatus.EXPIRED
                    && vs.DateOfImplementation < pastThreshold
                )
                .ExecuteUpdateAsync(
                    s => s.SetProperty(vs => vs.Status, VehicleStageStatus.EXPIRED),
                    ct
                );
            // 2) Set UPCOMING: nằm trong [-10; +10] ngày tính từ today
            var upcoming = await db
                .VehicleStages.Where(vs =>
                    vs.Status != VehicleStageStatus.COMPLETED
                    && vs.Status != VehicleStageStatus.UPCOMING
                    && vs.DateOfImplementation >= pastThreshold
                    && vs.DateOfImplementation <= futureThreshold
                )
                .ExecuteUpdateAsync(
                    s => s.SetProperty(vs => vs.Status, VehicleStageStatus.UPCOMING),
                    ct
                );

            // 3) Set NO_START: sau hơn 10 ngày kể từ today
            var nostart = await db
                .VehicleStages.Where(vs =>
                    vs.Status != VehicleStageStatus.COMPLETED
                    && vs.Status != VehicleStageStatus.NO_START
                    && vs.DateOfImplementation > futureThreshold
                )
                .ExecuteUpdateAsync(
                    s => s.SetProperty(vs => vs.Status, VehicleStageStatus.NO_START),
                    ct
                );

            if (expired + upcoming + nostart > 0)
            {
                _logger.LogInformation(
                    "VehicleStage status updated on {Today} (past<{Past}, future>{Future}): EXPIRED={Expired}, UPCOMING={Upcoming}, NO_START={NoStart}",
                    today.ToString("yyyy-MM-dd"),
                    pastThreshold.ToString("yyyy-MM-dd"),
                    futureThreshold.ToString("yyyy-MM-dd"),
                    expired,
                    upcoming,
                    nostart
                );
            }
        }
    }
}
