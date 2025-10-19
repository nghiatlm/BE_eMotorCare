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
            if (FirebaseApp.DefaultInstance != null)
            {
                _firebaseApp = FirebaseApp.DefaultInstance;
                return;
            }

            // Ưu tiên lấy từ biến môi trường
            var projectId = Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID");
            var privateKeyId = Environment.GetEnvironmentVariable("FIREBASE_PRIVATE_KEY_ID");
            var privateKey = Environment.GetEnvironmentVariable("FIREBASE_PRIVATE_KEY");
            var clientEmail = Environment.GetEnvironmentVariable("FIREBASE_CLIENT_EMAIL");
            var clientId = Environment.GetEnvironmentVariable("FIREBASE_CLIENT_EMAIL_ID");
            var authURI = Environment.GetEnvironmentVariable("FIREBASE_AUTH_URI");
            var tokenURI = Environment.GetEnvironmentVariable("FIREBASE_TOKEN_URI");
            var authProvider = Environment.GetEnvironmentVariable("FIREBASE_AUTH_PROVIDER");
            var client = Environment.GetEnvironmentVariable("FIREBASE_CLIENT");
            var universe = Environment.GetEnvironmentVariable("FIREBASE_UNIVERSE");

            if (!string.IsNullOrEmpty(projectId) && !string.IsNullOrEmpty(privateKey) && !string.IsNullOrEmpty(clientEmail))
            {
                // Build JSON config theo format của Firebase
                var firebaseJson = new
                {
                    type = "service_account",
                    project_id = projectId,
                    private_key_id = privateKeyId,
                    private_key = privateKey.Replace("\\n", "\n"), // quan trọng
                    client_email = clientEmail,
                    client_id = clientId,
                    auth_uri = authURI,
                    token_uri = tokenURI,
                    auth_provider_x509_cert_url = authProvider,
                    client_x509_cert_url = client,
                    universe_domain = universe
                };

                string jsonString = JsonSerializer.Serialize(firebaseJson);
                _firebaseApp = FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromJson(jsonString)
                });
            }
            else
            {
                // Fallback nếu không có ENV, lấy từ appsettings.json
                var firebaseConfig = configuration.GetSection("Firebase").Get<Dictionary<string, string>>();
                string jsonString = JsonSerializer.Serialize(firebaseConfig);

                _firebaseApp = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(jsonString)
                });
            }
        }

        public async Task<FirebaseToken> VerifyIdTokenAsync(string idToken)
        {
            var auth = FirebaseAuth.GetAuth(_firebaseApp);
            return await auth.VerifyIdTokenAsync(idToken);
        }
    }
}
