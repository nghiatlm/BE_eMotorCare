using eMotoCare.BO.Entities;
using eMotoCare.DAL.Repositories.AccountRepository;
using eMotoCare.DAL.Repositories.AppointmentRepository;
using eMotoCare.DAL.Repositories.BatteryCheckRepository;
using eMotoCare.DAL.Repositories.CampaignDetailRepository;
using eMotoCare.DAL.Repositories.CampaignRepository;
using eMotoCare.DAL.Repositories.CustomerRepository;
using eMotoCare.DAL.Repositories.EVCheckDetailRepository;
using eMotoCare.DAL.Repositories.EVCheckRepository;
using eMotoCare.DAL.Repositories.ExportNoteDetailRepository;
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
using eMotoCare.DAL.Repositories.RMADetailRepository;
using eMotoCare.DAL.Repositories.RMARepository;
using eMotoCare.DAL.Repositories.ServiceCenterInventoryRepository;
using eMotoCare.DAL.Repositories.ServiceCenterRepository;
using eMotoCare.DAL.Repositories.ServiceCenterSlotRepository;
using eMotoCare.DAL.Repositories.StaffRepository;
using eMotoCare.DAL.Repositories.VehiclePartItemRepository;
using eMotoCare.DAL.Repositories.VehicleRepository;
using eMotoCare.DAL.Repositories.VehicleStageRepository;

namespace eMotoCare.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Accounts { get; }
        IAppointmentRepository Appointments { get; }
        IBatteryCheckRepository BatteryChecks { get; }
        ICampaignDetailRepository CampaignDetails { get; }
        ICampaignRepository Campaigns { get; }
        ICustomerRepository Customers { get; }
        IEVCheckDetailRepository EVCheckDetails { get; }
        IEVCheckRepository EVChecks { get; }
        IExportNoteRepository ExportNotes { get; }
        IImportNoteRepository ImportNotes { get; }
        IMaintenancePlanRepository MaintenancePlans { get; }
        IMaintenanceStageDetailRepository MaintenanceStageDetails { get; }
        IMaintenanceStageRepository MaintenanceStages { get; }
        IModelPartTypeRepository ModelPartTypes { get; }
        IModelRepository Models { get; }
        IPartItemRepository PartItems { get; }
        IPartRepository Parts { get; }
        IPartTypeRepository PartTypes { get; }
        IPaymentRepository Payments { get; }
        IPriceServiceRepository PriceServices { get; }
        IRMADetailRepository RMADetails { get; }
        IRMARepository RMAs { get; }
        IServiceCenterInventoryRepository ServiceCenterInventories { get; }
        IServiceCenterRepository ServiceCenters { get; }
        IStaffRepository Staffs { get; }
        IVehiclePartItemRepository VehiclePartItems { get; }
        IVehicleRepository Vehicles { get; }
        IVehicleStageRepository VehicleStages { get; }
        IServiceCenterSlotRepository ServiceCenterSlot { get; }
        IExportNoteDetailRepository ExportNoteDetails { get; }

        void RemoveRange(List<EVCheckDetail> olds);
        Task<int> SaveAsync();
    }
}
