using eMotoCare.BO.Entities;
using eMotoCare.DAL.context;
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

namespace eMotoCare.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _unitOfWorkContext;

        public UnitOfWork(ApplicationDbContext unitOfWorkContext)
        {
            _unitOfWorkContext = unitOfWorkContext;
        }

        private IAccountRepository? _accountRepository;
        private IAppointmentRepository? _appointmentRepository;
        private IBatteryCheckRepository? _batteryCheckRepository;
        private IProgramDetailRepository? _programDetailRepository;
        private IProgramRepository? _programRepository;
        private IProgramModelRepository? _programModelRepository;
        private ICustomerRepository? _customerRepository;
        private IEVCheckDetailRepository? _evCheckDetailRepository;
        private IEVCheckRepository? _evCheckRepository;
        private IExportNoteRepository? _exportNoteRepository;
        private IImportNoteRepository? _importNoteRepository;
        private IMaintenancePlanRepository? _maintenancePlanRepository;
        private IMaintenanceStageDetailRepository? _maintenanceStageDetailRepository;
        private IMaintenanceStageRepository? _maintenanceStageRepository;
        private IModelPartTypeRepository? _modelPartTypeRepository;
        private IModelRepository? _modelRepository;
        private IPartItemRepository? _partItemRepository;
        private IPartRepository? _partRepository;
        private IPartTypeRepository? _partTypeRepository;
        private IPaymentRepository? _paymentRepository;
        private IPriceServiceRepository? _priceServiceRepository;
        private IRMADetailRepository? _rmaDetailRepository;
        private IRMARepository? _rmaRepository;
        private IServiceCenterInventoryRepository? _serviceCenterInventoryRepository;
        private IServiceCenterRepository? _serviceCenterRepository;
        private IStaffRepository? _staffRepository;
        private IVehiclePartItemRepository? _vehiclePartItemRepository;
        private IVehicleRepository? _vehicleRepository;
        private IVehicleStageRepository? _vehicleStageRepository;
        private IServiceCenterSlotRepository _serviceCenterSlotRepository;
        public IAccountRepository Accounts =>
            _accountRepository ??= new AccountRepository(_unitOfWorkContext);

        public IAppointmentRepository Appointments =>
            _appointmentRepository ??= new AppointmentRepository(_unitOfWorkContext);

        public IBatteryCheckRepository BatteryChecks =>
            _batteryCheckRepository ??= new BatteryCheckRepository(_unitOfWorkContext);

        public ICustomerRepository Customers =>
            _customerRepository ??= new CustomerRepository(_unitOfWorkContext);

        public IEVCheckDetailRepository EVCheckDetails =>
            _evCheckDetailRepository ??= new EVCheckDetailRepository(_unitOfWorkContext);

        public IEVCheckRepository EVChecks =>
            _evCheckRepository ??= new EVCheckRepository(_unitOfWorkContext);

        public IExportNoteRepository ExportNotes =>
            _exportNoteRepository ??= new ExportNoteRepository(_unitOfWorkContext);

        public IImportNoteRepository ImportNotes =>
            _importNoteRepository ??= new ImportNoteRepository(_unitOfWorkContext);

        public IMaintenancePlanRepository MaintenancePlans =>
            _maintenancePlanRepository ??= new MaintenancePlanRepository(_unitOfWorkContext);

        public IMaintenanceStageDetailRepository MaintenanceStageDetails =>
            _maintenanceStageDetailRepository ??= new MaintenanceStageDetailRepository(
                _unitOfWorkContext
            );

        public IMaintenanceStageRepository MaintenanceStages =>
            _maintenanceStageRepository ??= new MaintenanceStageRepository(_unitOfWorkContext);

        public IModelPartTypeRepository ModelPartTypes =>
            _modelPartTypeRepository ??= new ModelPartTypeRepository(_unitOfWorkContext);

        public IModelRepository Models =>
            _modelRepository ??= new ModelRepository(_unitOfWorkContext);

        public IPartItemRepository PartItems =>
            _partItemRepository ??= new PartItemRepository(_unitOfWorkContext);

        public IPartRepository Parts => _partRepository ??= new PartRepository(_unitOfWorkContext);

        public IPartTypeRepository PartTypes =>
            _partTypeRepository ??= new PartTypeRepository(_unitOfWorkContext);

        public IPaymentRepository Payments =>
            _paymentRepository ??= new PaymentRepository(_unitOfWorkContext);

        public IPriceServiceRepository PriceServices =>
            _priceServiceRepository ??= new PriceServiceRepository(_unitOfWorkContext);

        public IRMADetailRepository RMADetails =>
            _rmaDetailRepository ??= new RMADetailRepository(_unitOfWorkContext);

        public IRMARepository RMAs => _rmaRepository ??= new RMARepository(_unitOfWorkContext);

        public IServiceCenterInventoryRepository ServiceCenterInventories =>
            _serviceCenterInventoryRepository ??= new ServiceCenterInventoryRepository(
                _unitOfWorkContext
            );

        public IServiceCenterRepository ServiceCenters =>
            _serviceCenterRepository ??= new ServiceCenterRepository(_unitOfWorkContext);

        public IStaffRepository Staffs =>
            _staffRepository ??= new StaffRepository(_unitOfWorkContext);

        public IVehiclePartItemRepository VehiclePartItems =>
            _vehiclePartItemRepository ??= new VehiclePartItemRepository(_unitOfWorkContext);

        public IVehicleRepository Vehicles =>
            _vehicleRepository ??= new VehicleRepository(_unitOfWorkContext);

        public IVehicleStageRepository VehicleStages =>
            _vehicleStageRepository ??= new VehicleStageRepository(_unitOfWorkContext);
        public IServiceCenterSlotRepository ServiceCenterSlot =>
            _serviceCenterSlotRepository ??= new ServiceCenterSlotRepository(_unitOfWorkContext);

        public IProgramDetailRepository ProgramDetails => _programDetailRepository ??= new ProgramDetailRepository(_unitOfWorkContext);

        public IProgramRepository Programs => _programRepository ??= new ProgramRepository(_unitOfWorkContext);

        public IProgramModelRepository ProgramModels => _programModelRepository ??= new ProgramModelRepository(_unitOfWorkContext);

        public void RemoveRange(List<EVCheckDetail> olds)
        {
            _unitOfWorkContext.EVCheckDetails.RemoveRange(olds);
        }

        public void Dispose() => _unitOfWorkContext.Dispose();

        public async Task<int> SaveAsync() => await _unitOfWorkContext.SaveChangesAsync();
    }
}
