
using eMotoCare.Application.Interfaces;
using eMotoCare.Application.Interfaces.IRepository;
using eMotoCare.Application.Mapper;
using eMotoCare.Infrastructure.Repositories;
using eMotoCare.Infrastructure.Security;
using eMotoCare.Infrastructure.Sms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            // Auto Mapper Configurations
            services.AddAutoMapper(typeof(AccountMapper));


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
