
using FirebaseAdmin.Auth;

namespace eMototCare.BLL.Services.FirebaseServices
{
    public interface IFirebaseService
    {
        Task<FirebaseToken> VerifyIdTokenAsync(string idToken);
        Task<Dictionary<string, object>?> GetCustomerByCitizenIdAsync(string citizenId);
        bool IsFirestoreConfigured();
    }
}