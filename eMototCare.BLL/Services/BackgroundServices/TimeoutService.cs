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
        private static readonly TimeSpan Period = TimeSpan.FromDays(1);
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

            // Đầu ngày hôm nay theo timezone VN
            DateTime today = string.IsNullOrWhiteSpace(TimeZoneId)
                ? DateTime.UtcNow.Date
                : TimeZoneInfo
                    .ConvertTimeFromUtc(
                        DateTime.UtcNow,
                        TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId!)
                    )
                    .Date;

            // 1) EXPIRED: hôm nay > ExpectedEndDate
            var expired = await db
                .VehicleStages.Where(vs =>
                    vs.Status != VehicleStageStatus.COMPLETED
                    && vs.Status != VehicleStageStatus.EXPIRED
                    && vs.ExpectedEndDate.HasValue
                    && vs.ExpectedEndDate.Value.Date < today
                )
                .ExecuteUpdateAsync(
                    s => s.SetProperty(vs => vs.Status, VehicleStageStatus.EXPIRED),
                    ct
                );

            // 2) UPCOMING: hôm nay nằm trong [ExpectedStartDate; ExpectedEndDate]
            //    - Nếu có cả Start & End: Start <= today <= End
            //    - Nếu chỉ có Start      : Start <= today
            //    - Nếu chỉ có End        : today <= End
            var upcoming = await db
                .VehicleStages.Where(vs =>
                    vs.Status != VehicleStageStatus.COMPLETED
                    && vs.Status != VehicleStageStatus.UPCOMING
                    && (
                        // Có cả Start & End
                        (
                            vs.ExpectedStartDate.HasValue
                            && vs.ExpectedEndDate.HasValue
                            && vs.ExpectedStartDate.Value.Date <= today
                            && vs.ExpectedEndDate.Value.Date >= today
                        )
                        ||
                        // Chỉ có Start
                        (
                            vs.ExpectedStartDate.HasValue
                            && !vs.ExpectedEndDate.HasValue
                            && vs.ExpectedStartDate.Value.Date <= today
                        )
                        ||
                        // Chỉ có End
                        (
                            !vs.ExpectedStartDate.HasValue
                            && vs.ExpectedEndDate.HasValue
                            && vs.ExpectedEndDate.Value.Date >= today
                        )
                    )
                )
                .ExecuteUpdateAsync(
                    s => s.SetProperty(vs => vs.Status, VehicleStageStatus.UPCOMING),
                    ct
                );

            // 3) NO_START: hôm nay < ExpectedStartDate
            var noStartFuture = await db
                .VehicleStages.Where(vs =>
                    vs.Status != VehicleStageStatus.COMPLETED
                    && vs.Status != VehicleStageStatus.NO_START
                    && vs.ExpectedStartDate.HasValue
                    && vs.ExpectedStartDate.Value.Date > today
                )
                .ExecuteUpdateAsync(
                    s => s.SetProperty(vs => vs.Status, VehicleStageStatus.NO_START),
                    ct
                );

            // 4) NO_START: chưa có start/end (chưa cấu hình mốc thời gian)
            var noStartNoDates = await db
                .VehicleStages.Where(vs =>
                    vs.Status != VehicleStageStatus.COMPLETED
                    && vs.Status != VehicleStageStatus.NO_START
                    && !vs.ExpectedStartDate.HasValue
                    && !vs.ExpectedEndDate.HasValue
                )
                .ExecuteUpdateAsync(
                    s => s.SetProperty(vs => vs.Status, VehicleStageStatus.NO_START),
                    ct
                );

            var totalUpdated = expired + upcoming + noStartFuture + noStartNoDates;

            if (totalUpdated > 0)
            {
                _logger.LogInformation(
                    "VehicleStage status updated on {Today}: EXPIRED={Expired}, UPCOMING={Upcoming}, NO_START_FUTURE={NoStartFuture}, NO_START_NO_DATES={NoStartNoDates}",
                    today.ToString("yyyy-MM-dd"),
                    expired,
                    upcoming,
                    noStartFuture,
                    noStartNoDates
                );
            }
        }
    }
}
