
using eMotoCare.BLL.Mappers;

namespace eMotoCare.API.Configurations
{
    public static class AutoMapperConfiguration
    {
        public static IServiceCollection MapperInjection(this IServiceCollection services)
        {

            services.AddAutoMapper(typeof(AccountMapper));

            return services;
        }
    }
}
