
using eMotoCare.BLL.HashPassword;
using eMotoCare.BLL.JwtServices;
using eMotoCare.BLL.Services.AuthenticateService;
using eMotoCare.BLL.Services.OtpServices;
using eMotoCare.DAL;
using eMotoCare.DAL.Repositories.AccountRepository;
using eMotoCare.Infrastructure.Security;

namespace eMotoCare.API.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IJwtService, JwtService>();


            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();


            return services;
        }
    }
}
