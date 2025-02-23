using Microsoft.EntityFrameworkCore;
using PasswordStorageApi.Data;
using Microsoft.OpenApi.Models; // Add this using directive
using Microsoft.Extensions.DependencyInjection;
using PasswordStorageApi.Repository.Interface;
using PasswordStorageApi.Repository.Implementation;
using PasswordStorageApi.Service.Interface;
using PasswordStorageApi.Service.Implementaion; // Add this using directive

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
//add service
builder.Services.AddScoped<IPlatformService, PlatformService>();

//Add CORS - since i am getting an Httpconnectivity error
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Add Swagger services
builder.Services.AddSwaggerGen(c => // Change builder.AddSwaggerGen to builder.Services.AddSwaggerGen
{
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

app.Run();
