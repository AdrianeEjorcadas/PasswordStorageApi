using Microsoft.EntityFrameworkCore;
using PasswordStorageApi.Data;
using Microsoft.OpenApi.Models; // Add this using directive
using Microsoft.Extensions.DependencyInjection; // Add this using directive

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
