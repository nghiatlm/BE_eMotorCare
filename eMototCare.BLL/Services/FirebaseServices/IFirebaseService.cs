using FirebaseAdmin.Auth;

namespace eMototCare.BLL.Services.FirebaseServices
{
    public interface IFirebaseService
    {
        Task<FirebaseToken> VerifyIdTokenAsync(string idToken);
        Task<bool> GetCustomerByCitizenIdAsync(string citizenId, Guid accountId);
        bool IsFirestoreConfigured();
        Task<(string Id, Dictionary<string, object> Data)?> GetVehicleByChassisNumberAsync(
            string chassisNumber
        );
        Task<Dictionary<string, object>?> GetModelByIdAsync(string modelIdOrName);
        Task<Dictionary<string, object>?> GetCustomerByIdAsync(string customerId);
        Task<List<Dictionary<string, object>>> GetVehicleStagesByVehicleIdAsync(string vehicleId);
        Task<Dictionary<string, object>?> GetMaintenanceStageByIdAsync(string maintenanceStageId);
        Task<Dictionary<string, object>?> GetMaintenancePlanByIdAsync(string maintenancePlanId);
        Task<List<Dictionary<string, object>>> GetVehiclePartItemsByVehicleIdAsync(
            string vehicleId
        );
        Task<List<(string Id, Dictionary<string, object> Data)>> GetAllVehiclesAsync();
        Task<bool> GetMaintenancePlanAsync();
        Task<bool> GetMaintenanceStageAsync();
        Task<bool> GetMaintenanceStageDetailAsync();
        Task<bool> GetProgramAsync();
        Task<bool> GetProgramDetailAsync();
        Task<bool> GetVehiclePartitemAsync();
        Task<bool> GetModelAsync();
        Task<bool> GetVehicleByCustomerId(Guid customerId);
        Task<bool> GetPartItemAsync();
        Task<bool> GetVehicleStageAsync();
        Task<bool> CreateVehiclePartItemsByVehicleIdAsync(string vehicleId);
        Task<bool> CreateVehicleByChassis(string chassisNumber);
        Task<bool> CreateVehicleStageByVehicleId(string vehicleId);
        Task<bool> GetPartAsync();
    }
}
