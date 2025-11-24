using eMotoCare.BO.Enums;
using eMotoCare.DAL;
using eMototCare.BLL.Services.NotificationServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.BackgroundServices
{
    public class CampaignBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CampaignBackgroundService> _logger;
        private const string TimeZoneId = "SE Asia Standard Time";

        public CampaignBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<CampaignBackgroundService> logger
        )
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CampaignBackgroundService started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // await ProcessCampaignsAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while processing campaigns in background service");
                }

                // Chạy mỗi 1 giờ (có thể đổi thành TimeSpan.FromDays(1) nếu muốn)
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }

            _logger.LogInformation("CampaignBackgroundService stopped");
        }

        // private async Task ProcessCampaignsAsync(CancellationToken stoppingToken)
        // {
        //     var tz = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);

        //     var nowUtc = DateTime.UtcNow;
        //     var nowLocal = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, tz);

        //     var todayLocal = nowLocal.Date;
        //     var reminderLocalDate = todayLocal.AddDays(7); // nhắc trước 7 ngày

        //     // local -> utc range cho ngày nhắc
        //     var reminderStartLocal = reminderLocalDate; // 00:00
        //     var reminderEndLocal = reminderLocalDate.AddDays(1); // 00:00 hôm sau

        //     var reminderStartUtc = TimeZoneInfo.ConvertTimeToUtc(reminderStartLocal, tz);
        //     var reminderEndUtc = TimeZoneInfo.ConvertTimeToUtc(reminderEndLocal, tz);

        //     // local -> utc range cho hôm nay để xử lý expired
        //     var todayStartLocal = todayLocal;
        //     var todayStartUtc = TimeZoneInfo.ConvertTimeToUtc(todayStartLocal, tz);

        //     using var scope = _serviceProvider.CreateScope();
        //     var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        //     var notifier = scope.ServiceProvider.GetRequiredService<INotifierCampaignService>();

        //     // var campaignRepo = unitOfWork.Campaigns;

        //     // 1. Campaign sắp bắt đầu (nhắc trước 7 ngày)
        //     // var comingSoonCampaigns = (await campaignRepo.FindAllAsync())
        //     //     .Where(c =>
        //     //         c.Status == CampaignStatus.ACTIVE
        //     //         && c.StartDate >= reminderStartUtc
        //     //         && c.StartDate < reminderEndUtc
        //     //     )
        //     //     .AsQueryable()
        //     //     .ToList();

        //     foreach (var campaign in comingSoonCampaigns)
        //     {
        //         _logger.LogInformation(
        //             "Campaign {Code} starting soon on {StartDate}, sending reminder",
        //             campaign.Code,
        //             campaign.StartDate
        //         );

        //         await notifier.NotifyComingSoonAsync(
        //             "Campaign",
        //             new
        //             {
        //                 campaign.Id,
        //                 campaign.Code,
        //                 campaign.Name,
        //                 campaign.Description,
        //                 campaign.Type,
        //                 campaign.StartDate,
        //                 campaign.EndDate,
        //                 campaign.Status,
        //             }
        //         );
        //     }

        //     // 2. Campaign đã hết hạn -> đổi status COMPLETED
        //     var expiredCampaigns = (await campaignRepo.FindAllAsync())
        //         .Where(c => c.Status == CampaignStatus.ACTIVE && c.EndDate < todayStartUtc)
        //         .ToList();

        //     if (expiredCampaigns.Any())
        //     {
        //         foreach (var campaign in expiredCampaigns)
        //         {
        //             _logger.LogInformation(
        //                 "Campaign {Code} expired at {EndDate}, set status COMPLETED",
        //                 campaign.Code,
        //                 campaign.EndDate
        //             );

        //             campaign.Status = CampaignStatus.COMPLETED;

        //             await notifier.NotifyExpiredAsync(
        //                 "Campaign",
        //                 new
        //                 {
        //                     campaign.Id,
        //                     campaign.Code,
        //                     campaign.Name,
        //                     campaign.Description,
        //                     campaign.Type,
        //                     campaign.StartDate,
        //                     campaign.EndDate,
        //                     campaign.Status,
        //                 }
        //             );
        //         }

        //         await unitOfWork.SaveAsync();
        //     }
        // }
    }
}
