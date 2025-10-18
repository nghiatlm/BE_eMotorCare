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

            string? firebaseJsonEnv = Environment.GetEnvironmentVariable("FIREBASE_CONFIG");

            GoogleCredential credential;

            if (!string.IsNullOrEmpty(firebaseJsonEnv))
            {
                credential = GoogleCredential.FromJson(firebaseJsonEnv);
            }
            else
            {
                var firebaseConfig = configuration.GetSection("Firebase").Get<Dictionary<string, string>>();
                string jsonString = JsonSerializer.Serialize(firebaseConfig);
                credential = GoogleCredential.FromJson(jsonString);
            }

            _firebaseApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = credential
            });
        }

        public async Task<FirebaseToken> VerifyIdTokenAsync(string idToken)
        {
            var auth = FirebaseAuth.GetAuth(_firebaseApp);
            return await auth.VerifyIdTokenAsync(idToken);
        }
    }
}
