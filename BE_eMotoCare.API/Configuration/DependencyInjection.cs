
using eMotoCare.DAL.Configuration;
using eMototCare.BLL.Configuration;

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