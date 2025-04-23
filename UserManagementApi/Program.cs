using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UserManagementApi.Data;
using UserManagementApi.Filters;
using UserManagementApi.Helpers;
using UserManagementApi.Repositories;
using UserManagementApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Add DBContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Add Redis for Caching
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

//add services
builder.Services.AddScoped<IUserService, UserService>();
//add repo
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
//add email service
builder.Services.AddTransient<SmtpEmailHelper>();
builder.Services.AddTransient<SibEmailHelper>();
// add http context
builder.Services.AddHttpContextAccessor();

// add global custom filters
builder.Services.AddControllers(options => {
    options.Filters.Add<ValidateModelStateAttribute>(); // check model state
});

//add local scope filter
builder.Services.AddScoped<ValidateTokenFilter>();
builder.Services.AddScoped<ValidateAuthorizationFilter>();

//builder.Services.AddEndpointsApiExplorer(); // for minimal api

//Add Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName);
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "User Management API",
        Version = "v1",
        Description = "API for managing users, roles, and permissions."
    });
});

//Add CORS - since i am getting an Httpconnectivity error
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAllOrigins",
//        builder => builder.AllowAnyOrigin()
//                          .AllowAnyMethod()
//                          .AllowAnyHeader());
//});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Management API v1");
        c.RoutePrefix = string.Empty; // Serve at root URL
    });
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
