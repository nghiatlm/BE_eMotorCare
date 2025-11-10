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
            services.AddAutoMapper(typeof(AppointmentMapper));
            services.AddAutoMapper(typeof(CustomerMapper));
            services.AddAutoMapper(typeof(MaintenancePlanMapper));
            services.AddAutoMapper(typeof(MaintenanceStageMapper));
            services.AddAutoMapper(typeof(MaintenanceStageDetailMapper));
            services.AddAutoMapper(typeof(PartMapper));
            services.AddAutoMapper(typeof(PartTypeMapper));
            services.AddAutoMapper(typeof(VehicleMapper));
            services.AddAutoMapper(typeof(VehiclePartItemMapper));
            services.AddAutoMapper(typeof(CampaignDetailMapper));
            services.AddAutoMapper(typeof(CampaignMapper));
            services.AddAutoMapper(typeof(PartItemMapper));
            services.AddAutoMapper(typeof(ExportNoteMapper));
            services.AddAutoMapper(typeof(ImportNoteMapper));
            services.AddAutoMapper(typeof(EVCheckDetailMapper));

            services.AddAutoMapper(typeof(ServiceCenterSlotMapper));
            services.AddAutoMapper(typeof(EVCheckMapper));
            services.AddAutoMapper(typeof(VehicleStageMapper));
            services.AddAutoMapper(typeof(PriceServiceMapper));
            services.AddAutoMapper(typeof(PaymentMapper));
            services.AddAutoMapper(typeof(RMAMapper));
            return services;
        }
    }
}
