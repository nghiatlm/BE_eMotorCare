using FirebaseAdmin.Auth;

namespace eMototCare.BLL.Services.FirebaseServices
{
    public interface IFirebaseService
    {
        Task<FirebaseToken> VerifyIdTokenAsync(string idToken);
        Task<Dictionary<string, object>?> GetCustomerByCitizenIdAsync(string citizenId);
        bool IsFirestoreConfigured();
        Task<(string Id, Dictionary<string, object> Data)?> GetVehicleByChassisNumberAsync(
            string chassisNumber
        );
        Task<Dictionary<string, object>?> GetModelByIdAsync(string modelId);
        Task<Dictionary<string, object>?> GetCustomerByIdAsync(string customerId);
        Task<List<Dictionary<string, object>>> GetVehicleStagesByVehicleIdAsync(string vehicleId);
        Task<Dictionary<string, object>?> GetMaintenanceStageByIdAsync(string maintenanceStageId);
    }
}
