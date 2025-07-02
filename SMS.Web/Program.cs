using SMS.Web;
using SMS.Web.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SMS.Web.Authentication;
using SMS.Web.Services;
using SMS.Web.Services.Toasts;
using SMS.Common.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();
builder.AddRedisOutputCache("cache");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


//builder.Services.AddHttpClient<SMSApiClient>(client =>
//{
//    client.BaseAddress = new(Environment.GetEnvironmentVariable("API_BASE_URL") ?? "https+http://localhost:7386");
//});

//builder.Services.AddHttpClient<SMSApiClient>(client =>
//{
//    client.BaseAddress = new(Environment.GetEnvironmentVariable("API_BASE_URL") ?? "https+http://apiservice");
//});


builder.Services.AddHttpClient<SMSApiClient>(client =>
{
    client.BaseAddress = new("https+http://apiservice");
});

builder.Services.AddFluentUIComponents();

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; });
builder.Services.AddScoped<IUserAuthState, UserAuthState>();

builder.Services.AddAuthenticationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddOutputCache();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<ISmsCoreToastService, SmsCoreToastService>();
builder.Services.AddSingleton<IPermissionService, PermissionService>();



// Add localization services
builder.Services.AddLocalization();
builder.Services.AddControllers();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();
app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();






