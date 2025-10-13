
using eMotoCare.Application.Interfaces;
using eMotoCare.Application.Interfaces.IService;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace eMotoCare.Application.Services
{
    public class OtpService : IOtpService
    {
        private readonly IMemoryCache _cache;
        private readonly ISmsSender _smsSender;
        private readonly ILogger<OtpService> _logger;

        public OtpService(IMemoryCache cache, ISmsSender smsSender, ILogger<OtpService> logger)
        {
            _cache = cache;
            _smsSender = smsSender;
            _logger = logger;
        }

        public async Task<string> GenerateAndSendOtpAsync(string phone)
        {
            var code = new Random().Next(100000, 999999).ToString();

            var cacheKey = $"otp_{phone}";
            _cache.Set(cacheKey, code, TimeSpan.FromMinutes(10)); // OTP hết hạn sau 10 phút

            await _smsSender.SendOtpAsync(phone, $"Mã xác thực của bạn là: {code}");
            _logger.LogInformation("Đã gửi OTP {Code} đến {Phone}", code, phone);

            return code;
        }

        public Task<bool> VerifyOtpAsync(string phone, string code)
        {
            var cacheKey = $"otp_{phone}";
            if (_cache.TryGetValue(cacheKey, out string? storedCode))
            {
                if (storedCode == code)
                {
                    _cache.Remove(cacheKey); // Xóa sau khi dùng
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }
    }
}
