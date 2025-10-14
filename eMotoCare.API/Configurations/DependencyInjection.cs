using eMotoCare.BLL.HashPassword;
using eMotoCare.BLL.JwtServices;
using eMotoCare.BLL.Services.AdminServices;
using eMotoCare.BLL.Services.AuthenticateService;
using eMotoCare.BLL.Services.BranchServices;
using eMotoCare.BLL.Services.OtpServices;
using eMotoCare.BLL.Services.ServiceCenterServices;
using eMotoCare.DAL;
using eMotoCare.DAL.Repositories.AccountRepository;
using eMotoCare.DAL.Repositories.BranchRepository;
using eMotoCare.DAL.Repositories.ServiceCenterRepository;
using eMotoCare.Infrastructure.Security;

namespace eMotoCare.API.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddScoped<IAdminUserService, AdminUserService>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<IServiceCenterService, ServiceCenterService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IJwtService, JwtService>();

            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IServiceCenterRepository, ServiceCenterRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

            return services;
        }
    }
}
