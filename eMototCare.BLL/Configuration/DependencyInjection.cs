

using Microsoft.Extensions.DependencyInjection;

namespace eMototCare.BLL.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceDI(this IServiceCollection services)
        {
            // services.AddScoped<IPetShopMemberService, PetShopMemberService>();
            // services.AddScoped<IPetService, PetService>();

            return services;
        }
    }
}