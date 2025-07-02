using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using SMS.ApiService.BackgroundServices;
using SMS.ApiService.Extensions;
using SMS.ApiService.Repositories.Auths;
using SMS.ApiService.Repositories.ClassUnits;
using SMS.ApiService.Repositories.Roles;
using SMS.ApiService.Repositories.SchoolClasses;
using SMS.ApiService.Repositories.Schools;
using SMS.ApiService.Repositories.Settings;
using SMS.ApiService.Repositories.Users;
using SMS.ApiService.Repositories.Workflows;
using SMS.Common.Authorization;
using SMS.Common.Enums;
using System.Text;
try
{
    var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSMSDbContext(builder.Configuration, builder.Environment);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
            ValidateIssuerSigningKey = true
        };
    });
    //builder.Services.AddAuthorization();
    builder.Services.AddHostedService<SMSCoreStartupHostedService>();
    builder.Services.AddScoped<IAuthRepository, AuthRepository>();
    builder.Services.AddScoped<IRoleRepository, RoleRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddSingleton<IPermissionService, PermissionService>();
    builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
    builder.Services.AddScoped<IClassSettingsRepository, ClassSettingsRepository>();
    builder.Services.AddScoped<IFeesSettingRepository, FeesSettingRepository>();
    builder.Services.AddScoped<ISchoolSettingsRepository, SchoolSettingsRepository>();
    builder.Services.AddScoped<ISchoolRepository, SchoolRepository>();
    builder.Services.AddScoped<ISchoolClassRepository, SchoolClassRepository>();
    builder.Services.AddScoped<IClassUnitRepository, ClassUnitRepository>();
    builder.Services.AddScoped<IWorkflowRepository, WorkflowRepository>();



    builder.Services.AddAuthorization(options =>
    {
        foreach (var permission in Enum.GetValues<Permission>())
        {
            options.AddPolicy(permission.ToString(), policy =>
                policy.Requirements.Add(new PermissionRequirement(permission)));
        }
    });
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins("https://localhost:5238") // Use your Blazor client origin
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });
    var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
        app.UseDeveloperExceptionPage();
        app.MapOpenApi();
    //navigate to: https://localhost:7386/scalar/
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("SMS Core V1")
        .WithTheme(ScalarTheme.Mars)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.MapControllers();
app.MapDefaultEndpoints();

app.Run();

}
catch (Exception exception)
{
    // NLog: catch setup errors
    //logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    //LogManager.Shutdown();
}
 