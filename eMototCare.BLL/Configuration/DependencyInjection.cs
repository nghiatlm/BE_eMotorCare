using eMototCare.BLL.HashPasswords;
using eMototCare.BLL.JwtServices;
using Microsoft.Extensions.DependencyInjection;

namespace eMototCare.BLL.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceDI(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}