using Microsoft.EntityFrameworkCore;
using PasswordStorageApi.Data;
using Microsoft.OpenApi.Models; // Add this using directive
using Microsoft.Extensions.DependencyInjection;
using PasswordStorageApi.Repository.Interface;
using PasswordStorageApi.Repository.Implementation;
using PasswordStorageApi.Service.Interface;
using PasswordStorageApi.Service.Implementaion;
using PasswordStorageApi.Configuration;
using PasswordStorageApi.Helpers;
using PasswordStorageApi.Loggers; // Add this using directive

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//add db service
builder.Services.AddDbContext<PasswordStorageDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//add repository
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordRepository, PasswordRepository>();
//add service
builder.Services.AddScoped<IPlatformService, PlatformService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
// logger
//builder.Services.AddSingleton<ILogger, CustomLogger>(); 

//Add CORS - since i am getting an Httpconnectivity error
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

//Add config reader
//builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
//builder.Services.AddSingleton<EncryptionHelper>();

// Add Swagger services
builder.Services.AddSwaggerGen(c => // Change builder.AddSwaggerGen to builder.Services.AddSwaggerGen
{
    c.CustomSchemaIds(type => type.FullName);
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Password Storage API",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Password Storage API v1");
        c.RoutePrefix = string.Empty; // Optional: Serve Swagger UI at the root
    });
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Load the encryption key from the configuration
//LoadEncryptionKey(builder.Configuration);

// Resolve the EncryptionHelper to use in your application
//var encryptionHelper = app.Services.GetRequiredService<EncryptionHelper>();
//// Use the encryption key
//encryptionHelper.

// --- Register the Custom Logger Provider ---
// Define the file path where logs will be stored.
string logFilePath = "Logs/logs.txt";

// Retrieve the IServiceScopeFactory from the app's services.
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

// Add our custom logger provider to the logging factory.
// You can choose the minimum log level you want (e.g., LogLevel.Information).
var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
loggerFactory.AddProvider(new CustomLoggerProvider(logFilePath, LogLevel.Information, scopeFactory));

app.Run();

// Method to load the encryption key
//static void LoadEncryptionKey(IConfiguration configuration)
//{
//    var encryptionKey = configuration.GetSection("AppSettings")["EncryptionKey"];
//    EncryptionHelper.SetEncryptionKey(encryptionKey);
//}