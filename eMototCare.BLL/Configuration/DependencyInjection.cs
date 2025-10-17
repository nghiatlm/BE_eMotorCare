using eMototCare.BLL.HashPasswords;
using eMototCare.BLL.JwtServices;
using eMototCare.BLL.Services.AccountService;
using eMototCare.BLL.Services.AccountServices;
using eMototCare.BLL.Services.AuthServices;
using eMototCare.BLL.Services.ServiceCenterServices;
using eMototCare.BLL.Services.StaffServices;
using eMototCare.BLL.Services.CustomerServices;
using Microsoft.Extensions.DependencyInjection;
using eMototCare.BLL.Services.MaintenancePlanServices;
using eMototCare.BLL.Services.MaintenanceStageServices;

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
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IMaintenancePlanService, MaintenancePlanService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IMaintenanceStageService, MaintenanceStageService>();
            return services;
        }
    }
}
