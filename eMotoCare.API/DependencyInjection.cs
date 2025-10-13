using eMotoCare.Application;
using eMotoCare.Infrastructure;

namespace eMotoCare.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection services)
        {
            services.AddApplicationDI();
            services.AddInfrastructureDI();
            return services;
        }
    }
}
