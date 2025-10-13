using eMotoCare.BLL.SmsSender;
using System.Text.Json.Serialization;
using Vonage;

namespace eMotoCare.API.Configurations
{
    public static class SMSConfig
    {
        public static IServiceCollection AddSMSConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    });
            var vonageSection = configuration.GetSection("Vonage");
            var apiKey = vonageSection["ApiKey"];
            var apiSecret = vonageSection["ApiSecret"];
            var fromNumber = vonageSection["FromNumber"];


            services.AddScoped<ISmsSender>(_ =>
                    new VonageSmsSender(apiKey, apiSecret, fromNumber));
            return services;
        }
    }
}
