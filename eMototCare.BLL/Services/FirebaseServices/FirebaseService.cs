using System.Text.Json;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using eMotoCare.BO.Exceptions;

namespace eMototCare.BLL.Services.FirebaseServices
{

    public class FirebaseService : IFirebaseService
    {
        private readonly FirebaseApp _firebaseApp;
        private readonly FirestoreDb? _firestoreDb;

        public FirebaseService(IConfiguration configuration)
        {
            if (FirebaseApp.DefaultInstance != null)
            {
                _firebaseApp = FirebaseApp.DefaultInstance;
                _firestoreDb = InitializeFirestoreFromApp(_firebaseApp, configuration);
                return;
            }
            var firebaseAppOptions = BuildAppOptions(configuration);
            _firebaseApp = FirebaseApp.Create(firebaseAppOptions);
            _firestoreDb = InitializeFirestoreFromApp(_firebaseApp, configuration);
        }


        private AppOptions BuildAppOptions(IConfiguration configuration)
        {
            var projectId = Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID") ?? configuration["Firebase:project_id"];
            var privateKeyId = Environment.GetEnvironmentVariable("FIREBASE_PRIVATE_KEY_ID") ?? configuration["Firebase:private_key_id"];
            var privateKey = Environment.GetEnvironmentVariable("FIREBASE_PRIVATE_KEY") ?? configuration["Firebase:private_key"];
            var clientEmail = Environment.GetEnvironmentVariable("FIREBASE_CLIENT_EMAIL") ?? configuration["Firebase:client_email"];
            var clientId = Environment.GetEnvironmentVariable("FIREBASE_CLIENT_ID") ?? configuration["Firebase:client_id"];
            var authUri = Environment.GetEnvironmentVariable("FIREBASE_AUTH_URI") ?? configuration["Firebase:auth_uri"];
            var tokenUri = Environment.GetEnvironmentVariable("FIREBASE_TOKEN_URI") ?? configuration["Firebase:token_uri"];
            var authProvider = Environment.GetEnvironmentVariable("FIREBASE_AUTH_PROVIDER") ?? configuration["Firebase:auth_provider_x509_cert_url"];
            var clientCert = Environment.GetEnvironmentVariable("FIREBASE_CLIENT") ?? configuration["Firebase:client_x509_cert_url"];
            var universe = Environment.GetEnvironmentVariable("FIREBASE_UNIVERSE") ?? configuration["Firebase:universe_domain"];

            if (string.IsNullOrEmpty(projectId) || string.IsNullOrEmpty(privateKey) || string.IsNullOrEmpty(clientEmail))
            {
                throw new AppException("Firebase environment variables or config missing");
            }

            var firebaseJson = new
            {
                type = "service_account",
                project_id = projectId,
                private_key_id = privateKeyId,
                private_key = privateKey.Replace("\\n", "\n"),
                client_email = clientEmail,
                client_id = clientId,
                auth_uri = authUri,
                token_uri = tokenUri,
                auth_provider_x509_cert_url = authProvider,
                client_x509_cert_url = clientCert,
                universe_domain = universe
            };

            string jsonString = JsonSerializer.Serialize(firebaseJson);

            return new AppOptions
            {
                Credential = GoogleCredential.FromJson(jsonString)
            };
        }

        private FirestoreDb? InitializeFirestoreFromApp(FirebaseApp firebaseApp, IConfiguration configuration)
        {
            try
            {
                var projectId = Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID") ?? configuration["Firebase:project_id"];
                if (string.IsNullOrEmpty(projectId))
                    return null;

                var builder = new FirestoreDbBuilder
                {
                    ProjectId = projectId,
                    Credential = firebaseApp.Options.Credential
                };

                // Support non-default database IDs (e.g. "emotocare2") via env or config
                var databaseId = Environment.GetEnvironmentVariable("FIREBASE_DATABASE_ID") ?? configuration["Firebase:database_id"];
                if (!string.IsNullOrEmpty(databaseId))
                {
                    builder.DatabaseId = databaseId;
                }

                return builder.Build();
            }
            catch
            {
                return null;
            }
        }



        public bool IsFirestoreConfigured() => _firestoreDb != null;

        public async Task<FirebaseToken> VerifyIdTokenAsync(string idToken)
        {
            if (_firebaseApp == null)
                throw new AppException("FirebaseApp chưa được khởi tạo");

            var auth = FirebaseAuth.GetAuth(_firebaseApp);
            return await auth.VerifyIdTokenAsync(idToken);
        }

        public async Task<Dictionary<string, object>?> GetCustomerByCitizenIdAsync(string citizenId)
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            try
            {
                var collection = _firestoreDb.Collection("customer");
                var query = collection.WhereEqualTo("citizenId", citizenId);
                var snapshot = await query.GetSnapshotAsync();

                if (snapshot.Count == 0)
                    return null;

                // Lấy document đầu tiên
                return snapshot.Documents[0].ToDictionary();
            }
            catch (Grpc.Core.RpcException ex)
            {
                // có thể log để debug
                Console.WriteLine($"Firestore RPC Error: {ex.Message}");
                return null;
            }
        }
    }
}