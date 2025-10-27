using eMototCare.BLL.HashPasswords;
using eMototCare.BLL.JwtServices;
using eMototCare.BLL.Services.AccountService;
using eMototCare.BLL.Services.AccountServices;
using eMototCare.BLL.Services.AppointmentServices;
using eMototCare.BLL.Services.AuthServices;
using eMototCare.BLL.Services.CustomerServices;
using eMototCare.BLL.Services.FirebaseServices;
using eMototCare.BLL.Services.MaintenancePlanServices;
using eMototCare.BLL.Services.MaintenanceStageDetailServices;
using eMototCare.BLL.Services.MaintenanceStageServices;
using eMototCare.BLL.Services.PartServices;
using eMototCare.BLL.Services.PartTypeServices;
using eMototCare.BLL.Services.EVCheckServices;
using eMototCare.BLL.Services.ServiceCenterServices;
using eMototCare.BLL.Services.StaffServices;
using eMototCare.BLL.Services.VehiclePartItemServices;
using eMototCare.BLL.Services.VehicleServices;
using Microsoft.Extensions.DependencyInjection;
using eMototCare.BLL.Services.EVCheckDetailServices;

namespace eMototCare.BLL.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceDI(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IServiceCenterService, ServiceCenterService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IMaintenancePlanService, MaintenancePlanService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IPartTypeService, PartTypeService>();
            services.AddScoped<IMaintenanceStageService, MaintenanceStageService>();
            services.AddScoped<IMaintenanceStageDetailService, MaintenanceStageDetailService>();
            services.AddScoped<IPartService, PartService>();
            services.AddScoped<IEVCheckService, EVCheckService>();
            services.AddScoped<IFirebaseService, FirebaseService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IVehiclePartItemService, VehiclePartItemService>();
            services.AddScoped<IEVCheckDetailService, EVCheckDetailService>();

            return services;
        }
    }
}
