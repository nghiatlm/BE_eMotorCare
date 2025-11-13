using eMotoCare.BO.Enums;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.BackgroundServices
{
    public class ServiceCenterSlotAutoCloseService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ServiceCenterSlotAutoCloseService> _logger;

        public ServiceCenterSlotAutoCloseService(
            IServiceScopeFactory scopeFactory,
            ILogger<ServiceCenterSlotAutoCloseService> logger
        )
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ServiceCenterSlotAutoCloseService started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var now = DateTime.Now;
                    var today = DateOnly.FromDateTime(now.Date);
                    var timeNow = now.TimeOfDay;

                    var slots = await db
                        .ServiceCenterSlots.Where(s => s.IsActive && s.Date == today)
                        .ToListAsync(stoppingToken);

                    int changed = 0;
                    foreach (var s in slots)
                    {
                        var endTime = GetEndTime(s.SlotTime);
                        if (timeNow >= endTime && s.IsActive)
                        {
                            s.IsActive = false;
                            changed++;
                        }
                    }

                    if (changed > 0)
                    {
                        await db.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation(
                            "Auto closed {Count} slots for {Date}",
                            changed,
                            today
                        );
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in ServiceCenterSlotAutoCloseService");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation("ServiceCenterSlotAutoCloseService stopping");
        }

        private TimeSpan GetEndTime(SlotTime slot)
        {
            return slot switch
            {
                SlotTime.H07_08 => new TimeSpan(8, 0, 0),
                SlotTime.H08_09 => new TimeSpan(9, 0, 0),
                SlotTime.H09_10 => new TimeSpan(10, 0, 0),
                SlotTime.H10_11 => new TimeSpan(11, 0, 0),
                SlotTime.H11_12 => new TimeSpan(12, 0, 0),
                SlotTime.H13_14 => new TimeSpan(14, 0, 0),
                SlotTime.H14_15 => new TimeSpan(15, 0, 0),
                SlotTime.H15_16 => new TimeSpan(16, 0, 0),
                SlotTime.H16_17 => new TimeSpan(17, 0, 0),
                SlotTime.H17_18 => new TimeSpan(18, 0, 0),
                _ => new TimeSpan(23, 59, 59),
            };
        }
    }
}
