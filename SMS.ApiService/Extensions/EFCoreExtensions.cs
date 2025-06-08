using SMS.ApiService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace SMS.ApiService.Extensions
{
    public static class EFCoreExtensions
    {
        private const string AuthDb = "SMS";
        public static IServiceCollection AddSMSDbContext(this IServiceCollection services, IConfiguration config, IHostEnvironment environment)
        {
            var conn = config.GetConnectionString(AuthDb);
            return services.AddDbContext<AppDbContext>(
                options =>
                {
                    options.UseNpgsql(conn, optsBuilder => optsBuilder.MigrationsAssembly(ThisAssembly.Info.Title)
                            .MigrationsHistoryTable("__EFMigrationsHistory", schema: AppDbContext.Schema));
                    if (environment.IsDevelopment())
                    {
                        options.EnableDetailedErrors()
                            .EnableSensitiveDataLogging();
                    }
                });
        }
    }
}

