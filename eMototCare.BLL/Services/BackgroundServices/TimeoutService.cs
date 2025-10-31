using eMotoCare.DAL.context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.BackgroundServices
{
    public class TimeoutService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TimeoutService> _logger;

        public TimeoutService(IServiceScopeFactory scopeFactory, ILogger<TimeoutService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Lặp vô tận cho đến khi token báo huỷ
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // 1. Đợi một khoảng thời gian trước khi chạy lần kế
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);

                    // 2. Tạo scope mới để lấy DbContext (vì BackgroundService không phải request)
                    using var scope = _scopeFactory.CreateScope();
                    var dbContext =
                        scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    // 3. Xác định thời điểm hiện tại
                    var now = DateTime.UtcNow;

                    //// 4. Truy vấn các transaction chờ và đã quá 15 phút
                    //var expired = await dbContext.Transactions
                    //    .Where(t => t.Status == EnumStatusPayment.PENDING
                    //             && t.CreatedAt.AddMinutes(15) < now)
                    //    .ToListAsync(stoppingToken);

                    //if (expired.Any())
                    //{
                    //    // 5. Cập nhật trạng thái huỷ
                    //    expired.ForEach(t => t.Status = EnumStatusPayment.CANCELLED);
                    //    await dbContext.SaveChangesAsync(stoppingToken);

                    //    _logger.LogInformation("Cancelled {Count} expired transactions at {Time}", expired.Count, now);
                    //}
                }
                catch (TaskCanceledException)
                {
                    // Nếu task bị huỷ, không làm gì cả
                    _logger.LogInformation("OrderTimeoutService was cancelled.");
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in OrderTimeoutService");
                }
            }
        }
    }
}
