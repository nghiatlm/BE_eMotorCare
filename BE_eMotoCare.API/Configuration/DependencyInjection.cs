using BE_eMotoCare.API.Realtime.Services;
using eMotoCare.DAL.Configuration;
using eMototCare.BLL.Configuration;
using eMototCare.BLL.Services.NotificationServices;

namespace BE_eMotoCare.API.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection services)
        {
            services.AddScoped<INotifierService, NotifierService>();
            services.AddScoped<INotifierExportNoteService, NotifierExportNoteService>();
            services.AddScoped<INotifierCampaignService, NotificationCampaignService>();
            services.AddScoped<INotifierAppointmentService, NotificationAppointmentService>();
            services.AddServiceDI().AddRepoDI().AddSwaggerDependencies().MapperInjection();
            return services;
        }
    }
}
