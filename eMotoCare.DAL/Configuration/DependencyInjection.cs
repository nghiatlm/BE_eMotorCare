using eMotoCare.DAL.Base;
using eMotoCare.DAL.Repositories.AccountRepository;
using eMotoCare.DAL.Repositories.AppointmentRepository;
using eMotoCare.DAL.Repositories.BatteryCheckRepository;
using eMotoCare.DAL.Repositories.CustomerRepository;
using eMotoCare.DAL.Repositories.EVCheckDetailRepository;
using eMotoCare.DAL.Repositories.EVCheckRepository;
using eMotoCare.DAL.Repositories.ExportNoteRepository;
using eMotoCare.DAL.Repositories.ImportNoteRepository;
using eMotoCare.DAL.Repositories.MaintenancePlanRepository;
using eMotoCare.DAL.Repositories.MaintenanceStageDetailRepository;
using eMotoCare.DAL.Repositories.MaintenanceStageRepository;
using eMotoCare.DAL.Repositories.ModelPartTypeRepository;
using eMotoCare.DAL.Repositories.ModelRepository;
using eMotoCare.DAL.Repositories.PartItemRepository;
using eMotoCare.DAL.Repositories.PartRepository;
using eMotoCare.DAL.Repositories.PartTypeRepository;
using eMotoCare.DAL.Repositories.PaymentRepository;
using eMotoCare.DAL.Repositories.PriceServiceRepository;
using eMotoCare.DAL.Repositories.ProgramDetailRepository;
using eMotoCare.DAL.Repositories.ProgramModelRepository;
using eMotoCare.DAL.Repositories.ProgramRepository;
using eMotoCare.DAL.Repositories.RMADetailRepository;
using eMotoCare.DAL.Repositories.RMARepository;
using eMotoCare.DAL.Repositories.ServiceCenterInventoryRepository;
using eMotoCare.DAL.Repositories.ServiceCenterRepository;
using eMotoCare.DAL.Repositories.ServiceCenterSlotRepository;
using eMotoCare.DAL.Repositories.StaffRepository;
using eMotoCare.DAL.Repositories.VehiclePartItemRepository;
using eMotoCare.DAL.Repositories.VehicleRepository;
using eMotoCare.DAL.Repositories.VehicleStageRepository;
using Microsoft.Extensions.DependencyInjection;

namespace eMotoCare.DAL.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepoDI(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IBatteryCheckRepository, BatteryCheckRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IEVCheckDetailRepository, EVCheckDetailRepository>();
            services.AddScoped<IEVCheckRepository, EVCheckRepository>();
            services.AddScoped<IExportNoteRepository, ExportNoteRepository>();
            services.AddScoped<IImportNoteRepository, ImportNoteRepository>();
            services.AddScoped<IMaintenancePlanRepository, MaintenancePlanRepository>();
            services.AddScoped<
                IMaintenanceStageDetailRepository,
                MaintenanceStageDetailRepository
            >();
            services.AddScoped<IProgramRepository, ProgramRepository>();
            services.AddScoped<IProgramModelRepository, ProgramModelRepository>();
            services.AddScoped<IProgramDetailRepository, ProgramDetailRepository>();
            services.AddScoped<IMaintenanceStageRepository, MaintenanceStageRepository>();
            services.AddScoped<IModelPartTypeRepository, ModelPartTypeRepository>();
            services.AddScoped<IModelRepository, ModelRepository>();
            services.AddScoped<IPartItemRepository, PartItemRepository>();
            services.AddScoped<IPartRepository, PartRepository>();
            services.AddScoped<IPartTypeRepository, PartTypeRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPriceServiceRepository, PriceServiceRepository>();
            services.AddScoped<IRMADetailRepository, RMADetailRepository>();
            services.AddScoped<IRMARepository, RMARepository>();
            services.AddScoped<
                IServiceCenterInventoryRepository,
                ServiceCenterInventoryRepository
            >();
            services.AddScoped<IServiceCenterRepository, ServiceCenterRepository>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            services.AddScoped<IVehiclePartItemRepository, VehiclePartItemRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IVehicleStageRepository, VehicleStageRepository>();
            services.AddScoped<IServiceCenterSlotRepository, ServiceCenterSlotRepository>();

            return services;
        }
    }
}
