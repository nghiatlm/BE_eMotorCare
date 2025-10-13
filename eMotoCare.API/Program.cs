using eMotoCare.API.Configurations;
using eMotoCare.Common.Models.Settings;
using eMotoCare.DAL.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddAppDI(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.MapperInjection();
builder.Services.AddSMSConfiguration(builder.Configuration);


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

builder.Services.AddDbContext<DBContextMotoCare>(options =>
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






var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();
