using eMototCare.BLL.HashPasswords;
using eMototCare.BLL.JwtServices;
using eMototCare.BLL.Services.AccountService;
using eMototCare.BLL.Services.AccountServices;
using eMototCare.BLL.Services.AuthServices;
using eMototCare.BLL.Services.FirebaseServices;
using eMototCare.BLL.Services.ServiceCenterServices;
using eMototCare.BLL.Services.StaffServices;
using Microsoft.Extensions.DependencyInjection;

namespace eMototCare.BLL.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceDI(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IServiceCenterService, ServiceCenterService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IFirebaseService, FirebaseService>();
            return services;
        }
    }
}
