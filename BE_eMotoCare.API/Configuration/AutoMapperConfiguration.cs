using eMototCare.BLL.Mappers;

namespace BE_eMotoCare.API.Configuration
{
    public static class AutoMapperConfiguration
    {
        public static IServiceCollection MapperInjection(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ServiceCenterMapper));
            services.AddAutoMapper(typeof(AccountMapper));
            services.AddAutoMapper(typeof(StaffMapper));
            services.AddAutoMapper(typeof(CustomerMapper));
            return services;
        }
    }
}
