
using Microsoft.Extensions.DependencyInjection;

namespace eMotoCare.DAL.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepoDI(this IServiceCollection services)
        {
            // services.AddScoped<IPetShopMemberRepository, PetShopMemberRepository>();
            // services.AddScoped<IPetRepository, PetRepository>();
            return services;
        }
    }
}