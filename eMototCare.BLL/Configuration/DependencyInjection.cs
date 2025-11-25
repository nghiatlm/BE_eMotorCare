using eMotoCare.BO.Common.src;
using eMototCare.BLL.HashPasswords;
using eMototCare.BLL.JwtServices;
using eMototCare.BLL.Services.AccountService;
using eMototCare.BLL.Services.AccountServices;
using eMototCare.BLL.Services.AppointmentServices;
using eMototCare.BLL.Services.AuthServices;
using eMototCare.BLL.Services.BackgroundServices;
using eMototCare.BLL.Services.BatteryCheckServices;
using eMototCare.BLL.Services.CustomerServices;
using eMototCare.BLL.Services.DashboardServices;
using eMototCare.BLL.Services.EmailServices;
using eMototCare.BLL.Services.EVCheckDetailServices;
using eMototCare.BLL.Services.EVCheckServices;
using eMototCare.BLL.Services.ExportServices;
using eMototCare.BLL.Services.FirebaseServices;
using eMototCare.BLL.Services.ImportNoteServices;
using eMototCare.BLL.Services.MaintenancePlanServices;
using eMototCare.BLL.Services.MaintenanceStageDetailServices;
using eMototCare.BLL.Services.MaintenanceStageServices;
using eMototCare.BLL.Services.ModelServices;
using eMototCare.BLL.Services.PartItemServices;
using eMototCare.BLL.Services.PartServices;
using eMototCare.BLL.Services.PartTypeServices;
using eMototCare.BLL.Services.PayosServices;
using eMototCare.BLL.Services.PriceServiceServices;
using eMototCare.BLL.Services.ProgramService;
using eMototCare.BLL.Services.RMADetailServices;
using eMototCare.BLL.Services.RMAServices;
using eMototCare.BLL.Services.ServiceCenterInventoryServices;
using eMototCare.BLL.Services.ServiceCenterServices;
using eMototCare.BLL.Services.ServiceCenterSlotServices;
using eMototCare.BLL.Services.StaffServices;
using eMototCare.BLL.Services.VehiclePartItemServices;
using eMototCare.BLL.Services.VehicleServices;
using eMototCare.BLL.Services.VehicleStageServices;
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
            services.AddScoped<IServiceCenterSlotService, ServiceCenterSlotService>();
            services.AddScoped<IVehicleStageService, VehicleStageService>();
            services.AddScoped<IPriceServiceService, PriceServiceService>();
            services.AddScoped<IImportNoteService, ImportNoteService>();
            services.AddScoped<IExportService, ExportService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPayosService, PayosService>();
            services.AddScoped<Utils>();
            services.AddScoped<IPartItemService, PartItemService>();
            services.AddScoped<IRMAService, RMAService>();
            services.AddScoped<IServiceCenterInventoryService, ServiceCenterInventoryService>();
            services.AddScoped<IRMADetailService, RMADetailService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IBatteryCheckService, BatteryCheckService>();
            services.AddScoped<IProgramService, ProgramService>();
            services.AddScoped<IModelService, ModelService>();

            services.AddHostedService<TimeoutService>();
            services.AddHostedService<ServiceCenterSlotAutoCloseService>();
            services.AddHostedService<CampaignBackgroundService>();
            return services;
        }
    }
}
