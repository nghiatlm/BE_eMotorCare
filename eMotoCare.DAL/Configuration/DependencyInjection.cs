
using eMotoCare.DAL.Base;
using Microsoft.Extensions.DependencyInjection;

namespace eMotoCare.DAL.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepoDI(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            return services;
        }
    }
}