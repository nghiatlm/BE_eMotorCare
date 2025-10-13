
using eMotoCare.Application.Interfaces.IService;
using eMotoCare.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace eMotoCare.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<IOtpService, OtpService>();
            return services;
        }
    }
}
