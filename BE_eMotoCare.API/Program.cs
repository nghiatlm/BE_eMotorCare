using System.Text.Json.Serialization;
using BE_eMotoCare.API.Configuration;
using BE_eMotoCare.API.Middlewares;
using BE_eMotoCare.API.Realtime.Hubs;
using BE_eMotoCare.API.Realtime.Services;
using eMotoCare.BO.Common;
using eMotoCare.DAL.context;
using eMototCare.BLL.Services.BackgroundServices;
using Microsoft.EntityFrameworkCore;
using Net.payOS;

var builder = WebApplication.CreateBuilder(args);

// Add SignalR
builder.Services.AddSignalR();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

var mailSection = builder.Configuration.GetSection("MailSettings");
var fromEmail = mailSection["FromEmail"];
var fromName = mailSection["FromName"];
var mailPassword = mailSection["Password"];
var mailHost = mailSection["Host"];
var mailPort = int.TryParse(mailSection["Port"], out var parsedPort) ? parsedPort : 587;

var dbSection = builder.Configuration.GetSection("Database");
var server = dbSection["Server"];
var port = dbSection["Port"];
var database = dbSection["DataName"];
var user = dbSection["UserId"];
var password = dbSection["Password"];

var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? server;
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? port;
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? database;
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? user;
var dbPass = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? password;

var connectionString =
    $"Server={dbHost};Port={dbPort};Database={dbName};User Id={dbUser};Password={dbPass};SslMode=Required;";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 31)),
        mySqlOptions =>
            mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null
            )
    );
});

var PayOS = builder.Configuration.GetSection("PAYOS");
var ClientId = PayOS["CLIENT_ID"];
var APILEY = PayOS["API_KEY"];
var CHECKSUMKEY = PayOS["CHECKSUM_KEY"];
PayOS payOS = new PayOS(ClientId, APILEY, CHECKSUMKEY);
builder.Services.AddSingleton(payOS);

//DI
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddHttpContextAccessor();
builder.Services.AddAppDI();
builder.Services.MapperInjection();

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowExpoApp",
        policy =>
            policy
                //.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowed(_ => true)
    );
});

var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();

// }

app.UseHttpsRedirection();

app.UseAppExceptionHandler();
app.UseCors("AllowExpoApp");
app.MapHub<NotificationHub>("/hubs/notify");
app.MapHub<NotificationCampaignHub>("/hubs/notifycampaign");
app.MapHub<NotificationAppointmentHub>("/hubs/notifyappointment");
app.MapHub<NotificationExportNoteHub>("/hubs/notifyexportnote");
app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
