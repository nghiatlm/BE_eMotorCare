using System.Text.Json;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;

namespace eMototCare.BLL.Services.FirebaseServices
{
    public class FirebaseService : IFirebaseService
    {
        private readonly FirebaseApp _firebaseApp;

        public FirebaseService(IConfiguration configuration)
        {
            // Nếu FirebaseApp đã tồn tại, lấy instance mặc định luôn
            if (FirebaseApp.DefaultInstance != null)
            {
                _firebaseApp = FirebaseApp.DefaultInstance;
                return;
            }

            // Nếu chưa tồn tại thì tạo mới
            var firebaseConfig = configuration.GetSection("Firebase").Get<Dictionary<string, string>>();
            string jsonString = JsonSerializer.Serialize(firebaseConfig);

            _firebaseApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromJson(jsonString)
            });
        }

        public async Task<FirebaseToken> VerifyIdTokenAsync(string idToken)
        {
            var auth = FirebaseAuth.GetAuth(_firebaseApp);
            return await auth.VerifyIdTokenAsync(idToken);
        }
    }
}
