using eMotoCare.Application;
using eMotoCare.Infrastructure;

namespace eMotoCare.API.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationDI();
            services.AddInfrastructureDI(configuration);
            return services;
        }
    }
}
