using eMotoCare.DAL.Configuration;
using eMotoCare.DAL.Repositories.ServiceCenterInventoryRepository;
using eMototCare.BLL.Configuration;
using eMototCare.BLL.Services.ServiceCenterServices;

namespace BE_eMotoCare.API.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection services)
        {
            services.AddServiceDI().AddRepoDI();

            return services;
        }
    }
}
