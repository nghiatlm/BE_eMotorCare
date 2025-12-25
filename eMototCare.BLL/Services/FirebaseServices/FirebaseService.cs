using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Exceptions;
using eMotoCare.DAL;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace eMototCare.BLL.Services.FirebaseServices
{
    public class FirebaseService : IFirebaseService
    {
        private readonly FirebaseApp _firebaseApp;
        private readonly FirestoreDb? _firestoreDb;
        private readonly IUnitOfWork _unitOfWork;

        public FirebaseService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            var projectId =
                Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID")
                ?? configuration["Firebase:project_id"];
            var privateKeyId =
                Environment.GetEnvironmentVariable("FIREBASE_PRIVATE_KEY_ID")
                ?? configuration["Firebase:private_key_id"];
            var privateKey =
                Environment.GetEnvironmentVariable("FIREBASE_PRIVATE_KEY")
                ?? configuration["Firebase:private_key"];
            var clientEmail =
                Environment.GetEnvironmentVariable("FIREBASE_CLIENT_EMAIL")
                ?? configuration["Firebase:client_email"];
            var clientId =
                Environment.GetEnvironmentVariable("FIREBASE_CLIENT_ID")
                ?? configuration["Firebase:client_id"];
            var authUri =
                Environment.GetEnvironmentVariable("FIREBASE_AUTH_URI")
                ?? configuration["Firebase:auth_uri"];
            var tokenUri =
                Environment.GetEnvironmentVariable("FIREBASE_TOKEN_URI")
                ?? configuration["Firebase:token_uri"];
            var authProvider =
                Environment.GetEnvironmentVariable("FIREBASE_AUTH_PROVIDER")
                ?? configuration["Firebase:auth_provider_x509_cert_url"];
            var clientCert =
                Environment.GetEnvironmentVariable("FIREBASE_CLIENT")
                ?? configuration["Firebase:client_x509_cert_url"];
            var universe =
                Environment.GetEnvironmentVariable("FIREBASE_UNIVERSE")
                ?? configuration["Firebase:universe_domain"];

            if (
                string.IsNullOrEmpty(projectId)
                || string.IsNullOrEmpty(privateKey)
                || string.IsNullOrEmpty(clientEmail)
            )
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
                universe_domain = universe,
            };

            string jsonString = JsonSerializer.Serialize(firebaseJson);

            return new AppOptions { Credential = GoogleCredential.FromJson(jsonString) };
        }

        private FirestoreDb? InitializeFirestoreFromApp(
            FirebaseApp firebaseApp,
            IConfiguration configuration
        )
        {
            try
            {
                var projectId =
                    Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID")
                    ?? configuration["Firebase:project_id"];
                if (string.IsNullOrEmpty(projectId))
                    return null;

                var builder = new FirestoreDbBuilder
                {
                    ProjectId = projectId,
                    Credential = firebaseApp.Options.Credential,
                };

                // Support non-default database IDs (e.g. "emotocare2") via env or config
                var databaseId =
                    Environment.GetEnvironmentVariable("FIREBASE_DATABASE_ID")
                    ?? configuration["Firebase:database_id"];
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

        public async Task<bool> GetCustomerByCitizenIdAsync(string citizenId, Guid accountId)
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            try
            {
                var collection = _firestoreDb.Collection("customer");
                var query = collection.WhereEqualTo("citizenId", citizenId);
                var snapshot = await query.GetSnapshotAsync();

                if (snapshot.Count == 0)
                    return false;
                var doc = snapshot.Documents[0];
                var data = doc.ToDictionary();
                var customer = new Customer
                {
                    Id = Guid.Parse(doc.Id),
                    AccountId = accountId,
                    CitizenId = data.ContainsKey("citizenId")
                        ? data["citizenId"].ToString() ?? ""
                        : "",
                    FirstName = data.ContainsKey("firstName")
                        ? data["firstName"].ToString() ?? ""
                        : "",
                    LastName = data.ContainsKey("lastName")
                        ? data["lastName"].ToString() ?? ""
                        : "",
                    DateOfBirth =
                        data.ContainsKey("dateOfBirth")
                        && DateTime.TryParse(data["dateOfBirth"]?.ToString(), out var dob)
                            ? dob
                            : null,
                    CustomerCode = data.ContainsKey("customerCode")
                        ? data["customerCode"].ToString() ?? ""
                        : "",
                    Address = data.ContainsKey("address") ? data["address"].ToString() ?? "" : "",
                    Gender =
                        data.ContainsKey("gender")
                        && Enum.TryParse<GenderEnum>(data["gender"]?.ToString(), out var gender)
                            ? gender
                            : null,
                    AvatarUrl = data.ContainsKey("avatarUrl")
                        ? data["avatarUrl"].ToString() ?? ""
                        : "",
                };
                await _unitOfWork.Customers.CreateAsync(customer);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Grpc.Core.RpcException ex)
            {
                // có thể log để debug
                Console.WriteLine($"Firestore RPC Error: {ex.Message}");
                return false;
            }
        }

        public async Task<Dictionary<string, object>?> GetCustomerByIdAsync(string customerId)
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            try
            {
                var document = _firestoreDb.Collection("customer").Document(customerId);
                var snapshot = await document.GetSnapshotAsync();

                if (!snapshot.Exists)
                    return null;

                return snapshot.ToDictionary();
            }
            catch (Grpc.Core.RpcException ex)
            {
                // Log the error for debugging
                Console.WriteLine($"Firestore RPC Error: {ex.Message}");
                return null;
            }
        }

        public async Task<Dictionary<string, object>?> GetModelByIdAsync(string modelIdOrName)
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            try
            {
                DocumentSnapshot? snapshot;
                var docRef = _firestoreDb.Collection("model").Document(modelIdOrName);
                snapshot = await docRef.GetSnapshotAsync();

                if (!snapshot.Exists)
                {
                    var col = _firestoreDb.Collection("model");
                    var query = col.WhereEqualTo("name", modelIdOrName);
                    var snap = await query.GetSnapshotAsync();

                    if (snap.Count == 0)
                        return null;

                    snapshot = snap.Documents[0];
                }

                return snapshot.ToDictionary();
            }
            catch (Grpc.Core.RpcException ex)
            {
                Console.WriteLine($"Firestore RPC Error: {ex.Message}");
                return null;
            }
        }

        public async Task<(
            string Id,
            Dictionary<string, object> Data
        )?> GetVehicleByChassisNumberAsync(string chassisNumber)
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            // collection "vehicles", field "chassis_number"
            var col = _firestoreDb.Collection("vehicles");
            var query = col.WhereEqualTo("chassis_number", chassisNumber);
            var snapshot = await query.GetSnapshotAsync();

            if (snapshot.Count == 0)
                return null;

            var doc = snapshot.Documents[0];
            return (doc.Id, doc.ToDictionary());
        }
        public async Task<List<(string Id, Dictionary<string, object> Data)>> GetAllVehiclesAsync()
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            var col = _firestoreDb.Collection("vehicles");
            var snapshot = await col.GetSnapshotAsync();

            var result = new List<(string, Dictionary<string, object>)>();
            foreach (var doc in snapshot.Documents)
            {
                result.Add((doc.Id, doc.ToDictionary()));
            }

            return result;
        }

        public async Task<List<Dictionary<string, object>>> GetVehicleStagesByVehicleIdAsync(
    string vehicleId
)
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            var col = _firestoreDb.Collection("vehiclestage");
            var query = col.WhereEqualTo("vehicleId", vehicleId);
            var snapshot = await query.GetSnapshotAsync();

            Console.WriteLine(
                $"[Firestore] GetVehicleStagesByVehicleId vehicleId={vehicleId}, count={snapshot.Count}"
            );

            var list = new List<Dictionary<string, object>>();
            foreach (var doc in snapshot.Documents)
            {
                var data = doc.ToDictionary();

                // Sanitize the maintenanceStageName to remove newline characters
                if (data.ContainsKey("maintenanceStageName"))
                {
                    data["maintenanceStageName"] = data["maintenanceStageName"]?.ToString()?.Replace("\n", "").Trim();
                }

                list.Add(data);
            }
            return list;
        }


        public async Task<Dictionary<string, object>?> GetMaintenanceStageByIdAsync(
            string maintenanceStageId
        )
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            try
            {
                var docRef = _firestoreDb
                    .Collection("maintenancestage")
                    .Document(maintenanceStageId);
                var snapshot = await docRef.GetSnapshotAsync();

                if (!snapshot.Exists)
                    return null;

                return snapshot.ToDictionary();
            }
            catch (Grpc.Core.RpcException ex)
            {
                Console.WriteLine($"Firestore RPC Error: {ex.Message}");
                return null;
            }
        }

        public async Task<Dictionary<string, object>?> GetMaintenancePlanByIdAsync(
            string maintenancePlanId
        )
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            try
            {
                var docRef = _firestoreDb.Collection("maintenanceplan").Document(maintenancePlanId);
                var snapshot = await docRef.GetSnapshotAsync();

                if (!snapshot.Exists)
                    return null;

                return snapshot.ToDictionary();
            }
            catch (Grpc.Core.RpcException ex)
            {
                Console.WriteLine($"Firestore RPC Error: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Dictionary<string, object>>> GetVehiclePartItemsByVehicleIdAsync(
            string vehicleId
        )
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            var col = _firestoreDb.Collection("vehiclepartitem");
            var query = col.WhereEqualTo("vehicleId", vehicleId);
            var snapshot = await query.GetSnapshotAsync();

            Console.WriteLine(
                $"[Firestore] GetVehiclePartItemsByVehicleId vehicleId={vehicleId}, count={snapshot.Count}"
            );

            var list = new List<Dictionary<string, object>>();
            foreach (var doc in snapshot.Documents)
            {
                list.Add(doc.ToDictionary());
            }

            return list;
        }

        public async Task<bool> GetMaintenancePlanAsync()
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            try
            {
                var collectionRef = _firestoreDb.Collection("maintenanceplan");
                var snapshot = await collectionRef.GetSnapshotAsync();


                if (snapshot.Count == 0)
                    throw new AppException("Data nguồn đang trống hoặc không tìm thấy");
                var dbPlans = await _unitOfWork.MaintenancePlans.FindAllAsync();
                var dbIds = dbPlans.Select(x => x.Id.ToString()).ToHashSet();
                foreach (var doc in snapshot.Documents)
                {
                    string docId = doc.Id;

                    if (!dbIds.Contains(docId))
                    {
                        var data = doc.ToDictionary();
                        var unitString = data.ContainsKey("unit") ? data["unit"].ToString() : "";
                        Console.WriteLine("unitString: " + unitString);

                        var values = unitString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                               .Select(v => v.Trim())
                                               .ToList();
                        Console.WriteLine("After split: " + string.Join(" | ", values));

                        var enumValues = values
                            .Where(v => Enum.TryParse<MaintenanceUnit>(v, true, out _))
                            .Select(v => Enum.Parse<MaintenanceUnit>(v, true))
                            .ToArray();

                        
                        var newPlan = new MaintenancePlan
                        {
                            Id = Guid.Parse(docId),
                            Code = data.ContainsKey("code") ? data["code"].ToString() ?? "" : "",
                            Name = data.ContainsKey("name") ? data["name"].ToString() ?? "" : "",
                            Description = data.ContainsKey("description") ? data["description"].ToString() ?? "" : "",
                            Unit = enumValues,
                            TotalStages = data.ContainsKey("total_stages") ? Convert.ToInt32(data["total_stages"]) : 0,
                            EffectiveDate = data.ContainsKey("effective_date") ? Convert.ToDateTime(data["effective_date"]) : DateTime.UtcNow,
                            Status = data.ContainsKey("status") ? Enum.Parse<Status>(data["status"].ToString() ?? "ACTIVE") : Status.ACTIVE,
                            CreatedAt = data.ContainsKey("created_at") ? Convert.ToDateTime(data["created_at"]) : DateTime.UtcNow,
                            UpdatedAt = data.ContainsKey("updated_at") ? Convert.ToDateTime(data["updated_at"]) : DateTime.UtcNow,
                        };

                        await _unitOfWork.MaintenancePlans.CreateAsync(newPlan);

                    }
                }
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Grpc.Core.RpcException ex)
            {
                Console.WriteLine($"Firestore RPC Error: {ex.Message}");
                throw new AppException($"Firestore RPC Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }

        public async Task<bool> GetMaintenanceStageAsync()
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            try
            {
                var collectionRef = _firestoreDb.Collection("maintenancestage");
                var snapshot = await collectionRef.GetSnapshotAsync();


                if (snapshot.Count == 0)
                    throw new AppException("Data nguồn của Maintenance Stage đang trống hoặc không tìm thấy");
                var dbPlans = await _unitOfWork.MaintenanceStages.FindAllAsync();
                var dbIds = dbPlans.Select(x => x.Id.ToString()).ToHashSet();
                foreach (var doc in snapshot.Documents)
                {
                    string docId = doc.Id;

                    if (!dbIds.Contains(docId))
                    {
                        var data = doc.ToDictionary();
                        var maintenancePlanId = data.ContainsKey("maintenance_plan_id") ? Guid.Parse(data["maintenance_plan_id"].ToString() ?? throw new AppException("maintenance_plan_id trong firebase đang trống")) : throw new AppException("maintenance_plan_id không tồn tại trong Firebase");
                        var maintenancePlan = await _unitOfWork.MaintenancePlans.GetByIdAsync(maintenancePlanId);
                        if (maintenancePlan == null)
                            throw new AppException("Maintenance Plan chưa tồn tại trong hệ thống");
                        
                        var newStage = new MaintenanceStage
                        {
                            Id = Guid.Parse(docId),
                            MaintenancePlanId = data.ContainsKey("maintenance_plan_id") ? Guid.Parse(data["maintenance_plan_id"].ToString() ?? throw new AppException("maintenance_plan_id trong firebase đang trống")) : throw new AppException("maintenance_plan_id không tồn tại trong Firebase"),
                            Name = data.ContainsKey("name") ? data["name"].ToString() ?? "" : "",
                            Description = data.ContainsKey("description") ? data["description"].ToString() ?? "" : "",
                            Mileage = data.ContainsKey("mileage") ? Enum.Parse<Mileage>(data["mileage"].ToString() ?? "NONE") : throw new AppException("Mileage của Stage đang trống"),
                            DurationMonth = data.ContainsKey("duration_month") ? Enum.Parse<DurationMonth>(data["duration_month"].ToString() ?? "NONE") : throw new AppException("DurationMonth của Stage đang trống"),
                            EstimatedTime = data.ContainsKey("estimated_time") ? (int?)Convert.ToInt32(data["estimated_time"]) : null,
                            Status = data.ContainsKey("status") ? Enum.Parse<Status>(data["status"].ToString() ?? "ACTIVE") : Status.ACTIVE,
                            CreatedAt = data.ContainsKey("created_at") ? Convert.ToDateTime(data["created_at"]) : DateTime.UtcNow,
                            UpdatedAt = data.ContainsKey("updated_at") ? Convert.ToDateTime(data["updated_at"]) : DateTime.UtcNow,
                        };

                        await _unitOfWork.MaintenanceStages.CreateAsync(newStage);

                    }
                }
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Grpc.Core.RpcException ex)
            {
                Console.WriteLine($"Firestore RPC Error: {ex.Message}");
                throw new AppException($"Firestore RPC Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }
        public async Task<bool> GetMaintenanceStageDetailAsync()
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            try
            {
                var collectionRef = _firestoreDb.Collection("maintenancestagedetail");
                var snapshot = await collectionRef.GetSnapshotAsync();


                if (snapshot.Count == 0)    
                    throw new AppException("Data nguồn của Maintenance Stage Detail đang trống hoặc không tìm thấy");
                var dbPlans = await _unitOfWork.MaintenanceStageDetails.FindAllAsync();
                var dbIds = dbPlans.Select(x => x.Id.ToString()).ToHashSet();
                foreach (var doc in snapshot.Documents)
                {
                    string docId = doc.Id;

                    if (!dbIds.Contains(docId))
                    {
                        var data = doc.ToDictionary();
                        var maintenanceStageId = data.ContainsKey("maintenance_stage_id") ? Guid.Parse(data["maintenance_stage_id"].ToString() ?? throw new AppException("maintenance_stage_id trong firebase đang trống")) : throw new AppException("maintenance_stage_id không tồn tại trong Firebase");
                        var maintenanceStage = await _unitOfWork.MaintenanceStages.GetByIdAsync(maintenanceStageId);
                        if (maintenanceStage == null)
                            throw new AppException("Maintenance Stage chưa tồn tại trong database " + maintenanceStageId);

                        var partId = data.ContainsKey("part_id") ? Guid.Parse(data["part_id"].ToString() ?? throw new AppException("part_id trong firebase đang trống")) : throw new AppException("part_id không tồn tại trong Firebase");
                        var part = await _unitOfWork.Parts.GetByIdAsync(partId);
                        if (part == null)
                                throw new AppException("Part Id không tồn tại trong database: " + docId);
                        
                        var newStageDetail = new MaintenanceStageDetail
                        {
                            Id = Guid.Parse(docId),
                            MaintenanceStageId = data.ContainsKey("maintenance_stage_id") ? Guid.Parse(data["maintenance_stage_id"].ToString() ?? throw new AppException("maintenance_stage_id trong firebase đang trống")) : throw new AppException("maintenance_stage_id không tồn tại trong Firebase"),
                            PartId = data.ContainsKey("part_id") ? Guid.Parse(data["part_id"].ToString() ?? throw new AppException("part_id trong firebase đang trống")) : throw new AppException("part_id không tồn tại trong Firebase"),
                            ActionType = data.ContainsKey("action_type") ? ((string)data["action_type"]).Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                .Select(v => Enum.Parse<ActionType>(v.Trim(), true))
                                                .ToArray() : throw new AppException("ActionType của Stage Detail đang trống"),
                            Description = data.ContainsKey("description") ? data["description"].ToString() ?? "" : "",
                            Status = data.ContainsKey("status") ? Enum.Parse<Status>(data["status"].ToString() ?? "ACTIVE") : Status.ACTIVE,
                            CreatedAt = data.ContainsKey("created_at") ? Convert.ToDateTime(data["created_at"]) : DateTime.UtcNow,
                            UpdatedAt = data.ContainsKey("updated_at") ? Convert.ToDateTime(data["updated_at"]) : DateTime.UtcNow,
                        };

                        await _unitOfWork.MaintenanceStageDetails.CreateAsync(newStageDetail);
                        
                    }
                }
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Grpc.Core.RpcException ex)
            {
                Console.WriteLine($"Firestore RPC Error: {ex.Message}");
                throw new AppException($"Firestore RPC Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }

        public async Task<bool> GetProgramAsync()
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            try
            {
                var collectionRef = _firestoreDb.Collection("program");
                var snapshot = await collectionRef.GetSnapshotAsync();


                if (snapshot.Count == 0)
                    throw new AppException("Data nguồn của Program đang trống hoặc không tìm thấy");
                var dbPlans = await _unitOfWork.Programs.FindAllAsync();
                var dbIds = dbPlans.Select(x => x.Id.ToString()).ToHashSet();
                foreach (var doc in snapshot.Documents)
                {
                    string docId = doc.Id;
                    if (!dbIds.Contains(docId))
                    {
                        var data = doc.ToDictionary();

                        var program = new Program
                        {
                            Id = Guid.Parse(docId),
                            Type = data.ContainsKey("name") ? Enum.Parse<ProgramType>(data["name"].ToString() ?? throw new AppException("Type trong Firebase đang trống")) : throw new AppException("Type trong Firebase đang trống"),
                            Title = data.ContainsKey("title") ? data["title"].ToString() ?? "" : "",
                            StartDate = data.ContainsKey("start_date") ? Convert.ToDateTime(data["start_date"]) : DateTime.UtcNow,
                            EndDate = data.TryGetValue("end_date", out var hh)
                                                        && hh != null
                                                        && DateTime.TryParse(hh.ToString(), out var kk)
                                                            ? kk
                                                            : null,
                            AttachmentUrl = data.ContainsKey("attachment_url") ? data["attachment_url"].ToString() ?? "" : "",
                            CreatedBy = data.ContainsKey("created_by") ? (Guid?)Guid.Parse(data["created_by"].ToString() ?? null) : null,
                            UpdatedBy = data.ContainsKey("updated_by") ? (Guid?)Guid.Parse(data["updated_by"].ToString() ?? null) : null,
                            Description = data.ContainsKey("description") ? data["description"].ToString() ?? "" : "",
                            Status = data.ContainsKey("status") ? Enum.Parse<Status>(data["status"].ToString() ?? "ACTIVE") : Status.ACTIVE,
                            CreatedAt = data.ContainsKey("created_at") ? Convert.ToDateTime(data["created_at"]) : DateTime.UtcNow,
                            UpdatedAt = data.ContainsKey("updated_at") ? Convert.ToDateTime(data["updated_at"]) : DateTime.UtcNow,
                        };

                        await _unitOfWork.Programs.CreateAsync(program);

                    }
                }
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Grpc.Core.RpcException ex)
            {
                Console.WriteLine($"Firestore RPC Error: {ex.Message}");
                throw new AppException($"Firestore RPC Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }

        public async Task<bool> GetProgramDetailAsync()
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            try
            {
                var collectionRef = _firestoreDb.Collection("programdetail");
                var snapshot = await collectionRef.GetSnapshotAsync();


                if (snapshot.Count == 0)
                    throw new AppException("Data nguồn của Program đang trống hoặc không tìm thấy");
                var dbPlans = await _unitOfWork.ProgramDetails.FindAllAsync();
                var dbIds = dbPlans.Select(x => x.Id.ToString()).ToHashSet();
                foreach (var doc in snapshot.Documents)
                {
                    string docId = doc.Id;

                    if (!dbIds.Contains(docId))
                    {
                        var data = doc.ToDictionary();
                        var partId = data.ContainsKey("part_id") ? (Guid?)Guid.Parse(data["part_id"].ToString() ?? null) : null;
                        if (partId.HasValue)
                        {
                            var part = await _unitOfWork.Parts.GetByIdAsync(partId.Value);
                            if (part == null)
                                throw new AppException("Part Id không tồn tại trong database");
                        }
                        
                        
                        var programDetail = new ProgramDetail
                        {
                            Id = Guid.Parse(docId),
                            ProgramId = data.ContainsKey("program_id") ? Guid.Parse(data["program_id"].ToString() ?? throw new AppException("program_id trong firebase đang trống")) : throw new AppException("program_id không tồn tại trong Firebase"),
                            RecallPartId = data.ContainsKey("part_id") ? (Guid?)Guid.Parse(data["part_id"].ToString() ?? null) : null,
                            ServiceType = data.ContainsKey("service_type") ? data["service_type"].ToString() ?? "" : "",
                            DiscountPercent = data.ContainsKey("discount_percent") ? (int?)Convert.ToInt32(data["discount_percent"]) : null,
                            BonusAmount = data.ContainsKey("bonus_amount") ? (int?)Convert.ToInt32(data["bonus_amount"]) : null,
                            RecallAction = data.ContainsKey("recall_action") ? data["recall_action"].ToString() ?? "" : "",
                            CreatedAt = data.ContainsKey("created_at") ? Convert.ToDateTime(data["created_at"]) : DateTime.UtcNow,
                        };

                        await _unitOfWork.ProgramDetails.CreateAsync(programDetail);

                    }
                }
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Grpc.Core.RpcException ex)
            {
                Console.WriteLine($"Firestore RPC Error: {ex.Message}");
                throw new AppException($"Firestore RPC Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }
        public async Task<bool> GetVehiclePartitemAsync()
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            try
            {
                var collectionRef = _firestoreDb.Collection("vehiclepartitem");
                var snapshot = await collectionRef.GetSnapshotAsync();


                if (snapshot.Count == 0)
                    throw new AppException("Data nguồn của vehiclepartitem đang trống hoặc không tìm thấy");
                var dbPlans = await _unitOfWork.VehiclePartItems.FindAllAsync();
                var dbIds = dbPlans.Select(x => x.Id.ToString()).ToHashSet();
                foreach (var doc in snapshot.Documents)
                {
                    string docId = doc.Id;

                    if (!dbIds.Contains(docId))
                    {
                        var data = doc.ToDictionary();

                        var vehiclePartItem = new VehiclePartItem
                        {
                            Id = Guid.Parse(docId),
                            InstallDate = data.ContainsKey("installDate") ? Convert.ToDateTime(data["installDate"]) : throw new AppException("Install Date đang trống"),
                            VehicleId = data.ContainsKey("vehicleId") ? Guid.Parse(data["vehicleId"].ToString() ?? throw new AppException("vehicle_id trong firebase đang trống")) : throw new AppException("vehicle_id không tồn tại trong Firebase"),
                            PartItemId = data.ContainsKey("partItemId") ? Guid.Parse(data["partItemId"].ToString() ?? throw new AppException("part_item_id trong firebase đang trống")) : throw new AppException("part_item_id không tồn tại trong Firebase"),
                            ReplaceForId = data.ContainsKey("replaceForId") ? (Guid?)Guid.Parse(data["replaceForId"].ToString() ?? null) : null,
                            CreatedAt = data.ContainsKey("createdAt") ? Convert.ToDateTime(data["createdAt"]) : DateTime.UtcNow,
                            UpdatedAt = data.ContainsKey("updatedAt") ? Convert.ToDateTime(data["updatedAt"]) : DateTime.UtcNow,
                        };
                        var vehicleId = Guid.Parse(data["vehicleId"].ToString()!);
                        var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(vehicleId);
                        if (vehicle == null) continue; 

                        await _unitOfWork.VehiclePartItems.CreateAsync(vehiclePartItem);

                    }
                }
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Grpc.Core.RpcException ex)
            {
                Console.WriteLine($"Firestore RPC Error: {ex.Message}");
                throw new AppException($"Firestore RPC Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }
        public async Task<bool> GetModelAsync()
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            try
            {
                var collectionRef = _firestoreDb.Collection("model");
                var snapshot = await collectionRef.GetSnapshotAsync();


                if (snapshot.Count == 0)
                    throw new AppException("Data nguồn của model đang trống hoặc không tìm thấy");
                var dbPlans = await _unitOfWork.Models.FindAllAsync();
                var dbIds = dbPlans.Select(x => x.Id.ToString()).ToHashSet();
                foreach (var doc in snapshot.Documents)
                {
                    string docId = doc.Id;

                    if (!dbIds.Contains(docId))
                    {
                        var data = doc.ToDictionary();

                        var maintenancePlanIdStr = data.ContainsKey("maintenancePlanId")
                            ? data["maintenancePlanId"]?.ToString()
                            : null;

                        if (!Guid.TryParse(maintenancePlanIdStr, out var maintenancePlanId))
                            throw new AppException("maintenancePlanId từ Firebase không hợp lệ");

                        var plan = await _unitOfWork.MaintenancePlans.GetByIdAsync(maintenancePlanId);
                        if (plan == null)
                        {
                            var planDoc = await _firestoreDb
                                .Collection("maintenanceplan")
                                .Document(maintenancePlanIdStr!)
                                .GetSnapshotAsync();

                            if (!planDoc.Exists)
                                throw new AppException("Không tìm thấy MaintenancePlan trên Firebase");

                            var planData = planDoc.ToDictionary();
                            var unitStr = planData.ContainsKey("unit")
                                ? planData["unit"]?.ToString()?.Trim()
                                : null;

                            if (string.IsNullOrWhiteSpace(unitStr))
                                unitStr = "KILOMETER"; 

                            if (!Enum.TryParse<MaintenanceUnit>(unitStr, true, out var unitEnum))
                                unitEnum = MaintenanceUnit.KILOMETER;
                            int totalStages = 0;
                            if (planData.ContainsKey("total_stages"))
                            {
                                var tsStr = planData["total_stages"]?.ToString();
                                if (!string.IsNullOrWhiteSpace(tsStr))
                                    int.TryParse(tsStr, out totalStages);
                            }
                            DateTime effectiveDate = DateTime.UtcNow;
                            if (planData.ContainsKey("effectiveDate") &&
                                DateTime.TryParse(planData["effectiveDate"]?.ToString(), out var eff))
                            {
                                effectiveDate = eff;
                            }
                            plan = new MaintenancePlan
                            {
                                Id = maintenancePlanId,
                                Code = planData["code"]?.ToString(),
                                Name = planData["name"]?.ToString(),
                                Description = planData["description"]?.ToString(),
                                Status = Status.ACTIVE,
                                Unit = new[] { unitEnum },         
                                TotalStages = totalStages,
                                EffectiveDate = effectiveDate,
                            };

                            await _unitOfWork.MaintenancePlans.CreateAsync(plan);
                        }

                        var model = new Model
                        {
                            Id = Guid.Parse(docId),
                            Code = data["code"]?.ToString() ?? throw new AppException("Code đang trống"),
                            Name = data["name"]?.ToString() ?? throw new AppException("Name đang trống"),
                            Manufacturer = data["manufacturer"]?.ToString() ?? throw new AppException("Manufacturer đang trống"),
                            MaintenancePlanId = maintenancePlanId,
                            Status = data.ContainsKey("status")
                                ? Enum.Parse<Status>(data["status"]?.ToString() ?? "ACTIVE")
                                : Status.ACTIVE,
                        };

                        await _unitOfWork.Models.CreateAsync(model);
                    }
                }
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Grpc.Core.RpcException ex)
            {
                Console.WriteLine($"Firestore RPC Error: {ex.Message}");
                throw new AppException($"Firestore RPC Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }

        public async Task<bool> GetVehicleByCustomerId(Guid customerId)
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            try
            {
                var collection = _firestoreDb.Collection("vehicles");
                var query = collection.WhereEqualTo("customerId", customerId.ToString());
                var snapshot = await query.GetSnapshotAsync();

                if (snapshot.Count == 0)
                    return false;
                var dbPlans = await _unitOfWork.Vehicles.FindAllAsync();
                var dbIds = dbPlans.Select(x => x.Id.ToString()).ToHashSet();
                foreach (var doc in snapshot.Documents)
                {
                    var docId = doc.Id;

                    if (!dbIds.Contains(docId))
                    {
                        var data = doc.ToDictionary();
                        var vehicle = new Vehicle
                        {
                            Id = Guid.Parse(docId),
                            Image = data.ContainsKey("image") ? (data["image"].ToString() ?? throw new AppException("image trong firebase đang trống")) : throw new AppException("image không tồn tại trong Firebase"),
                            Color = data.ContainsKey("color") ? (data["color"].ToString() ?? throw new AppException("color trong firebase đang trống")) : throw new AppException("color không tồn tại trong Firebase"),
                            ChassisNumber = data.ContainsKey("chassis_number") ? (data["chassis_number"].ToString() ?? throw new AppException("chassis_number trong firebase đang trống")) : throw new AppException("chassis_number không tồn tại trong Firebase"),
                            EngineNumber = data.ContainsKey("engine_number") ? (data["engine_number"].ToString() ?? throw new AppException("engine_number trong firebase đang trống")) : throw new AppException("engine_number không tồn tại trong Firebase"),
                            Status = data.ContainsKey("status") ? Enum.Parse<StatusEnum>(data["status"].ToString() ?? "ACTIVE") : StatusEnum.ACTIVE,
                            ManufactureDate = data.ContainsKey("manufacture_date") ? Convert.ToDateTime(data["manufacture_date"]) : throw new AppException("manufacture_date không tồn tại trong Firebase"),
                            PurchaseDate = data.ContainsKey("purchase_date") ? Convert.ToDateTime(data["purchase_date"]) : throw new AppException("purchase_date không tồn tại trong Firebase"),
                            WarrantyExpiry = data.ContainsKey("warranty_expiry") ? Convert.ToDateTime(data["warranty_expiry"]) : throw new AppException("warranty_expiry không tồn tại trong Firebase"),
                            ModelId = data.ContainsKey("modelId") ? Guid.Parse(data["modelId"].ToString() ?? throw new AppException("modelId trong firebase đang trống")) : throw new AppException("modelId không tồn tại trong Firebase"),
                            CustomerId = customerId,
                            IsPrimary = data.ContainsKey("is_primary")
                                        && bool.TryParse(data["is_primary"]?.ToString(), out var v)
                                        ? v
                                        : false,
                        };
                        await _unitOfWork.Vehicles.CreateAsync(vehicle);
                    }
                    
                }

                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Grpc.Core.RpcException ex)
            {
                // có thể log để debug
                Console.WriteLine($"Firestore RPC Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> GetPartItemAsync()
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            try
            {
                var collectionRef = _firestoreDb.Collection("partitem");
                var snapshot = await collectionRef.GetSnapshotAsync();


                if (snapshot.Count == 0)
                    throw new AppException("Data nguồn của partitem đang trống hoặc không tìm thấy");
                var dbPlans = await _unitOfWork.PartItems.FindAllAsync();
                var dbIds = dbPlans.Select(x => x.Id.ToString()).ToHashSet();
                foreach (var doc in snapshot.Documents)
                {
                    string docId = doc.Id;

                    if (!dbIds.Contains(docId))
                    {
                        var data = doc.ToDictionary();

                        var partItem = new PartItem
                        {
                            Id = Guid.Parse(docId),
                            Quantity = data.ContainsKey("quantity") ? int.Parse(data["quantity"].ToString() ?? throw new AppException("Quantity đang null")) : throw new AppException("Quantity đang null"),
                            PartId = data.ContainsKey("part_id") ? Guid.Parse(data["part_id"].ToString() ?? throw new AppException("part_id trong firebase đang trống")) : throw new AppException("part_id không tồn tại trong Firebase"),
                            SerialNumber = data.ContainsKey("serial_number") ? (data["serial_number"].ToString() ?? throw new AppException("serial_number trong firebase đang trống")) : throw new AppException("serial_number không tồn tại trong Firebase"),
                            Price = data.ContainsKey("price") ? Decimal.Parse(data["price"].ToString() ?? "0") : 0,
                            Status = data.ContainsKey("status") ? Enum.Parse<PartItemStatus>(data["status"].ToString() ?? "ACTIVE") : PartItemStatus.INSTALLED,
                            WarrantyPeriod = data.ContainsKey("warranty_period") ? int.Parse(data["warranty_period"].ToString() ?? null) : null,
                            WarantyStartDate = data.TryGetValue("waranty_start_date", out var hh)
                                                        && hh != null
                                                        && DateTime.TryParse(hh.ToString(), out var kk)
                                                            ? kk
                                                            : null,
                            WarantyEndDate = data.TryGetValue("waranty_end_date", out var we)
                                                        && we != null
                                                        && DateTime.TryParse(we.ToString(), out var wa)
                                                            ? wa
                                                            : null,
                            ServiceCenterInventoryId = data.ContainsKey("service_center_inventory_id") ? Guid.Parse(data["service_center_inventory_id"].ToString() ?? throw new AppException("service_center_inventory_id trong firebase đang trống")) : throw new AppException("service_center_inventory_id không tồn tại trong Firebase"),
                            IsManufacturerWarranty = data.TryGetValue("is_manufacturer_warranty", out var v)
                                                    && v?.ToString() switch
                                                    {
                                                        "1" => true,
                                                        "0" => false,
                                                        var s when bool.TryParse(s, out var b) => b,
                                                        _ => false
                                                    },
                        };

                        await _unitOfWork.PartItems.CreateAsync(partItem);

                    }
                }
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Grpc.Core.RpcException ex)
            {
                Console.WriteLine($"Firestore RPC Error: {ex.Message}");
                throw new AppException($"Firestore RPC Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }

        public async Task<bool> GetPartAsync()
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            try
            {
                var collectionRef = _firestoreDb.Collection("part");
                var snapshot = await collectionRef.GetSnapshotAsync();


                if (snapshot.Count == 0)
                    throw new AppException("Data nguồn của part đang trống hoặc không tìm thấy");
                var dbPlans = await _unitOfWork.Parts.FindAllAsync();
                var dbIds = dbPlans.Select(x => x.Id.ToString()).ToHashSet();
                foreach (var doc in snapshot.Documents)
                {
                    string docId = doc.Id;

                    if (!dbIds.Contains(docId))
                    {
                        var data = doc.ToDictionary();
                        var partTypeId = data.ContainsKey("partTypeId") ? Guid.Parse(data["partTypeId"].ToString() ?? throw new AppException("part_type_id trong firebase đang trống")) : throw new AppException("part_type_id không tồn tại trong Firebase");
                        var partType =  await _unitOfWork.PartTypes.GetByIdAsync(partTypeId);
                        if (partType == null)
                            throw new AppException("Part Type chưa tồn tại trong hệ thống");
                        var part = new Part
                        {
                            Id = Guid.Parse(docId),
                            Quantity = data.ContainsKey("quantity") ? int.Parse(data["quantity"].ToString() ?? throw new AppException("Quantity đang null")) : throw new AppException("Quantity đang null"),
                            Code = data.ContainsKey("code") ? (data["code"].ToString() ?? throw new AppException("code trong firebase đang trống")) : throw new AppException("code không tồn tại trong Firebase"),
                            Description = data.ContainsKey("description") ? (data["description"].ToString() ?? "") : "",
                            Name = data.ContainsKey("name") ? (data["name"].ToString() ?? throw new AppException("name trong firebase đang trống")) : throw new AppException("name không tồn tại trong Firebase"),
                            CreatedAt = data.ContainsKey("createdAt") ? Convert.ToDateTime(data["created_at"]) : DateTime.UtcNow,
                            UpdatedAt = data.ContainsKey("updatedAt") ? Convert.ToDateTime(data["updated_at"]) : DateTime.UtcNow,
                            PartTypeId = data.ContainsKey("partTypeId") ? Guid.Parse(data["partTypeId"].ToString() ?? throw new AppException("part_type_id trong firebase đang trống")) : throw new AppException("part_type_id không tồn tại trong Firebase"),
                            Status = data.ContainsKey("status") ? Enum.Parse<Status>(data["status"].ToString() ?? "ACTIVE") : Status.ACTIVE,
                        };

                        await _unitOfWork.Parts.CreateAsync(part);

                    }
                }
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Grpc.Core.RpcException ex)
            {
                Console.WriteLine($"Firestore RPC Error: {ex.Message}");
                throw new AppException($"Firestore RPC Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }

        public async Task<bool> GetVehicleStageAsync()
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            try
            {
                var collectionRef = _firestoreDb.Collection("vehiclestage");
                var snapshot = await collectionRef.GetSnapshotAsync();


                if (snapshot.Count == 0)
                    throw new AppException("Data nguồn của vehiclestage đang trống hoặc không tìm thấy");
                var dbPlans = await _unitOfWork.VehicleStages.FindAllAsync();
                var dbIds = dbPlans.Select(x => x.Id.ToString()).ToHashSet();
                foreach (var doc in snapshot.Documents)
                {
                    string docId = doc.Id;

                    if (!dbIds.Contains(docId))
                    {
                        var data = doc.ToDictionary();

                        var vehicleStage = new VehicleStage
                        {
                            Id = Guid.Parse(docId),
                            ActualMaintenanceMileage = data.ContainsKey("actualMaintenanceMileage") ? int.Parse(data["actualMaintenanceMileage"].ToString() ?? "0") : 0,
                            MaintenanceStageId = data.ContainsKey("maintenancestageId") ? Guid.Parse(data["maintenancestageId"].ToString() ?? throw new AppException("maintenance_stage_id trong firebase đang trống")) : throw new AppException("maintenance_stage_id không tồn tại trong Firebase"),
                            ActualMaintenanceUnit = data.ContainsKey("actualMaintenanceUnit") ? Enum.Parse<MaintenanceUnit>(data["actualMaintenanceUnit"].ToString() ?? throw new AppException("actual_maintenance_unit trong firebase đang trống")) : throw new AppException("actual_maintenance_unit không tồn tại trong Firebase"),
                            Status = data.ContainsKey("status") ? Enum.Parse<VehicleStageStatus>(data["status"].ToString() ?? "NO_START") : VehicleStageStatus.NO_START,
                            VehicleId = data.ContainsKey("vehicleId") ? Guid.Parse(data["vehicleId"].ToString() ?? throw new AppException("vehicle_id trong firebase đang trống")) : throw new AppException("vehicle_id không tồn tại trong Firebase"),
                            UpdatedAt = data.ContainsKey("updatedAt") ? Convert.ToDateTime(data["updatedAt"]) : DateTime.UtcNow,
                            ActualImplementationDate = data.TryGetValue("actualImplementationDate", out var raw)
                                                        && raw != null
                                                        && DateTime.TryParse(raw.ToString(), out var dt)
                                                            ? dt
                                                            : null,
                            CreatedAt = data.ContainsKey("createdAt") ? Convert.ToDateTime(data["createdAt"]) : DateTime.UtcNow,
                            ExpectedEndDate = data.TryGetValue("expectedEndDate", out var expectedEndDate)
                                                        && expectedEndDate != null
                                                        && DateTime.TryParse(expectedEndDate.ToString(), out var eedt)
                                                            ? eedt
                                                            : null,
                            ExpectedImplementationDate = data.TryGetValue("expectedImplementationDate", out var ab)
                                                        && ab != null
                                                        && DateTime.TryParse(ab.ToString(), out var aa)
                                                            ? aa
                                                            : null,
                            ExpectedStartDate = data.TryGetValue("expectedStartDate", out var bb)
                                                        && bb != null
                                                        && DateTime.TryParse(bb.ToString(), out var bc)
                                                            ? bc
                                                            : null,
                        };

                        await _unitOfWork.VehicleStages.CreateAsync(vehicleStage);

                    }
                }
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Grpc.Core.RpcException ex)
            {
                Console.WriteLine($"Firestore RPC Error: {ex.Message}");
                throw new AppException($"Firestore RPC Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }

        public async Task<bool> CreateVehiclePartItemsByVehicleIdAsync(
            string vehicleId
        )
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            try
            {
                var collection = _firestoreDb.Collection("vehiclepartitem");
                var query = collection.WhereEqualTo("vehicleId", vehicleId.ToString());
                var snapshot = await query.GetSnapshotAsync();

                if (snapshot.Count == 0)
                    return false;
                var dbPlans = await _unitOfWork.VehiclePartItems.FindAllAsync();
                var dbIds = dbPlans.Select(x => x.Id.ToString()).ToHashSet();
                foreach (var doc in snapshot.Documents)
                {
                    var docId = doc.Id;

                    if (!dbIds.Contains(docId))
                    {
                        var data = doc.ToDictionary();
                        var vehiclePartItem = new VehiclePartItem
                        {
                            Id = Guid.Parse(docId),
                            InstallDate = data.ContainsKey("installDate") ? Convert.ToDateTime(data["installDate"]) : throw new AppException("Install Date đang trống"),
                            VehicleId = data.ContainsKey("vehicleId") ? Guid.Parse(data["vehicleId"].ToString() ?? throw new AppException("vehicle_id trong firebase đang trống")) : throw new AppException("vehicle_id không tồn tại trong Firebase"),
                            PartItemId = data.ContainsKey("partItemId") ? Guid.Parse(data["partItemId"].ToString() ?? throw new AppException("part_item_id trong firebase đang trống")) : throw new AppException("part_item_id không tồn tại trong Firebase"),
                            ReplaceForId = data.ContainsKey("replaceForId") ? (Guid?)Guid.Parse(data["replaceForId"].ToString() ?? null) : null,
                            CreatedAt = data.ContainsKey("createdAt") ? Convert.ToDateTime(data["createdAt"]) : DateTime.UtcNow,
                            UpdatedAt = data.ContainsKey("updatedAt") ? Convert.ToDateTime(data["updatedAt"]) : DateTime.UtcNow,
                        };
                        await _unitOfWork.VehiclePartItems.CreateAsync(vehiclePartItem);
                    }

                }

                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Grpc.Core.RpcException ex)
            {
                // có thể log để debug
                Console.WriteLine($"Firestore RPC Error: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> CreateVehicleStageByVehicleId(
            string vehicleId
        )
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            try
            {
                var collection = _firestoreDb.Collection("vehiclestage");
                var query = collection.WhereEqualTo("vehicleId", vehicleId.ToString());
                var snapshot = await query.GetSnapshotAsync();

                if (snapshot.Count == 0)
                    return false;
                var dbPlans = await _unitOfWork.VehicleStages.FindAllAsync();
                var dbIds = dbPlans.Select(x => x.Id.ToString()).ToHashSet();
                foreach (var doc in snapshot.Documents)
                {
                    var docId = doc.Id;

                    if (!dbIds.Contains(docId))
                    {
                        var data = doc.ToDictionary();
                        var vehicleStage = new VehicleStage
                        {
                            Id = Guid.Parse(docId),
                            ActualMaintenanceMileage = data.ContainsKey("actualMaintenanceMileage") ? int.Parse(data["actualMaintenanceMileage"].ToString() ?? "0") : 0,
                            MaintenanceStageId = data.ContainsKey("maintenancestageId") ? Guid.Parse(data["maintenancestageId"].ToString() ?? throw new AppException("maintenance_stage_id trong firebase đang trống")) : throw new AppException("maintenance_stage_id không tồn tại trong Firebase"),
                            ActualMaintenanceUnit = data.ContainsKey("actualMaintenanceUnit") ? Enum.Parse<MaintenanceUnit>(data["actualMaintenanceUnit"].ToString() ?? throw new AppException("actual_maintenance_unit trong firebase đang trống")) : throw new AppException("actual_maintenance_unit không tồn tại trong Firebase"),
                            Status = data.ContainsKey("status") ? Enum.Parse<VehicleStageStatus>(data["status"].ToString() ?? "NO_START") : VehicleStageStatus.NO_START,
                            VehicleId = data.ContainsKey("vehicleId") ? Guid.Parse(data["vehicleId"].ToString() ?? throw new AppException("vehicle_id trong firebase đang trống")) : throw new AppException("vehicle_id không tồn tại trong Firebase"),
                            UpdatedAt = data.ContainsKey("updatedAt") ? Convert.ToDateTime(data["updatedAt"]) : DateTime.UtcNow,
                            ActualImplementationDate = data.TryGetValue("actualImplementationDate", out var cc)
                                                        && cc != null
                                                        && DateTime.TryParse(cc.ToString(), out var ac)
                                                            ? ac
                                                            : null,
                            CreatedAt = data.ContainsKey("createdAt") ? Convert.ToDateTime(data["createdAt"]) : DateTime.UtcNow,
                            ExpectedEndDate = data.TryGetValue("expectedEndDate", out var ss)
                                                        && ss != null
                                                        && DateTime.TryParse(ss.ToString(), out var dd)
                                                            ? dd
                                                            : null,
                            ExpectedImplementationDate = data.TryGetValue("expectedImplementationDate", out var hh)
                                                        && hh != null
                                                        && DateTime.TryParse(hh.ToString(), out var kk)
                                                            ? kk
                                                            : null,
                            ExpectedStartDate = data.TryGetValue("expectedStartDate", out var qe)
                                                        && qe != null
                                                        && DateTime.TryParse(qe.ToString(), out var qs)
                                                            ? qs
                                                            : null,
                        };
                        await _unitOfWork.VehicleStages.CreateAsync(vehicleStage);
                    }

                }

                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Grpc.Core.RpcException ex)
            {
                // có thể log để debug
                Console.WriteLine($"Firestore RPC Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CreateVehicleByChassis(string chassisNumber)
        {
            if (_firestoreDb == null)
                throw new AppException("Firestore chưa được cấu hình");

            try
            {
                var collection = _firestoreDb.Collection("vehicles");
                var query = collection.WhereEqualTo("chassis_number", chassisNumber.ToString());
                var snapshot = await query.GetSnapshotAsync();

                if (snapshot.Count == 0)
                    return false;
                var dbPlans = await _unitOfWork.Vehicles.FindAllAsync();
                var dbIds = dbPlans.Select(x => x.Id.ToString()).ToHashSet();
                foreach (var doc in snapshot.Documents)
                {
                    var docId = doc.Id;

                    if (!dbIds.Contains(docId))
                    {
                        var data = doc.ToDictionary();
                        var vehicle = new Vehicle
                        {
                            Id = Guid.Parse(docId),
                            Image = data.ContainsKey("image") ? (data["image"].ToString() ?? throw new AppException("image trong firebase đang trống")) : throw new AppException("image không tồn tại trong Firebase"),
                            Color = data.ContainsKey("color") ? (data["color"].ToString() ?? throw new AppException("color trong firebase đang trống")) : throw new AppException("color không tồn tại trong Firebase"),
                            ChassisNumber = data.ContainsKey("chassis_number") ? (data["chassis_number"].ToString() ?? throw new AppException("chassis_number trong firebase đang trống")) : throw new AppException("chassis_number không tồn tại trong Firebase"),
                            EngineNumber = data.ContainsKey("engine_number") ? (data["engine_number"].ToString() ?? throw new AppException("engine_number trong firebase đang trống")) : throw new AppException("engine_number không tồn tại trong Firebase"),
                            Status = data.ContainsKey("status") ? Enum.Parse<StatusEnum>(data["status"].ToString() ?? "ACTIVE") : StatusEnum.ACTIVE,
                            ManufactureDate = data.ContainsKey("manufacture_date") ? Convert.ToDateTime(data["manufacture_date"]) : throw new AppException("manufacture_date không tồn tại trong Firebase"),
                            PurchaseDate = data.ContainsKey("purchase_date") ? Convert.ToDateTime(data["purchase_date"]) : throw new AppException("purchase_date không tồn tại trong Firebase"),
                            WarrantyExpiry = data.ContainsKey("warranty_expiry") ? Convert.ToDateTime(data["warranty_expiry"]) : throw new AppException("warranty_expiry không tồn tại trong Firebase"),
                            ModelId = data.ContainsKey("modelId") ? Guid.Parse(data["modelId"].ToString() ?? throw new AppException("modelId trong firebase đang trống")) : throw new AppException("modelId không tồn tại trong Firebase"),
                            CustomerId = data.ContainsKey("customerId") ? Guid.Parse(data["customerId"].ToString() ?? throw new AppException("customerId trong firebase đang trống")) : throw new AppException("customerId không tồn tại trong Firebase"),
                            IsPrimary = data.ContainsKey("is_primary")
                                        && bool.TryParse(data["is_primary"]?.ToString(), out var v)
                                        ? v
                                        : false,
                        };
                        await _unitOfWork.Vehicles.CreateAsync(vehicle);
                    }

                }

                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Grpc.Core.RpcException ex)
            {
                // có thể log để debug
                Console.WriteLine($"Firestore RPC Error: {ex.Message}");
                return false;
            }
        }



    }
}
