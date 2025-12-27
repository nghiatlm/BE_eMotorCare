
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.BackgroundServices
{
    public class MaintenanceScheduleNotification : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ServiceCenterSlotAutoCloseService> _logger;

        public MaintenanceScheduleNotification(
            IServiceScopeFactory scopeFactory,
            ILogger<ServiceCenterSlotAutoCloseService> logger
        )
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("VehicleStageDailyNotificationService started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.UtcNow.AddHours(7);


                    if (now.Hour == 8 && now.Minute < 2)
                    //if (now.Hour >= 22)
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                        var today = now.Date;

                        var stages = await db.VehicleStages
                            .Include(v => v.Vehicle)
                                .ThenInclude(v => v.Customer)
                            .Where(v =>
                                v.ExpectedStartDate != null &&
                                v.ExpectedEndDate != null &&
                                v.ExpectedStartDate.Value.Date <= today &&
                                v.ExpectedEndDate.Value.Date >= today &&
                                v.Status == VehicleStageStatus.UPCOMING
                                
                            )
                            .ToListAsync(stoppingToken);

                        int created = 0;

                        foreach (var stage in stages)
                        {
                            
                            var receiverId = stage.Vehicle.Customer.AccountId;

                            bool existed = await db.Notifications.AnyAsync(n =>
                                n.ReceiverId == receiverId &&
                                n.Type == NotificationEnum.MAINTENANCE_REMINDER &&
                                n.SentAt.Date == today,
                                stoppingToken);

                            if (existed)
                                continue;

                            db.Notifications.Add(new Notification
                            {
                                Id = Guid.NewGuid(),
                                ReceiverId = receiverId.Value,
                                Title = "Thông báo lịch bảo dưỡng",
                                Message = "Bạn có lịch hẹn bảo dưỡng cho xe sắp đến hạn. Vui lòng kiểm tra trong ứng dụng.",
                                Type = NotificationEnum.MAINTENANCE_REMINDER,
                                SentAt = now,
                                IsRead = false
                                
                            });

                            created++;
                        }

                        if (created > 0)
                        {
                            await db.SaveChangesAsync(stoppingToken);
                            _logger.LogInformation(
                                "Created {Count} daily maintenance notifications for {Date}",
                                created,
                                today);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in VehicleStageDailyNotificationService");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation("VehicleStageDailyNotificationService stopping");
        }
    }
}
