using BE_eMotoCare.API.Realtime.Services;
using eMotoCare.DAL.Configuration;
using eMototCare.BLL.Configuration;

namespace BE_eMotoCare.API.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection services)
        {
            services.AddScoped<INotifierExportNoteService, NotifierExportNoteService>();
            services.AddServiceDI().AddRepoDI().AddSwaggerDependencies().MapperInjection();
            return services;
        }
    }
}
