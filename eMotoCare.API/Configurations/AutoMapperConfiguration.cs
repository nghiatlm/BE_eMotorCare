using eMotoCare.BLL.Mappers;

namespace eMotoCare.API.Configurations
{
    public static class AutoMapperConfiguration
    {
        public static IServiceCollection MapperInjection(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AccountMapper));
            services.AddAutoMapper(typeof(BranchMapper));
            services.AddAutoMapper(typeof(ServiceCenterMapper));
            return services;
        }
    }
}
