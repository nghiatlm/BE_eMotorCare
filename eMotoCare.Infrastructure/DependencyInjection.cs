
using eMotoCare.Application.Interfaces;
using eMotoCare.Application.Interfaces.IRepository;
using eMotoCare.Application.Interfaces.IService;
using eMotoCare.Application.Mapper;
using eMotoCare.Infrastructure.JwtServices;
using eMotoCare.Infrastructure.Repositories;
using eMotoCare.Infrastructure.Security;
using eMotoCare.Infrastructure.Sms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using Vonage;


namespace eMotoCare.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            services.AddScoped<IJwtService, JwtService>();


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
