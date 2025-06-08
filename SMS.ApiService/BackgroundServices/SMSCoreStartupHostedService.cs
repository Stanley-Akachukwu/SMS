
using Microsoft.EntityFrameworkCore;
using SMS.ApiService.Entities.Departments;
using SMS.ApiService.Entities.Roles;
using SMS.ApiService.Persistence;
using SMS.ApiService.Persistence.DataSeed;
using SMS.Common.Enums;
using System.Text.RegularExpressions;
namespace SMS.ApiService.BackgroundServices
{
    public sealed class SMSCoreStartupHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SMSCoreStartupHostedService> _logger;

        public SMSCoreStartupHostedService(
            IServiceScopeFactory scopeFactory,
            ILogger<SMSCoreStartupHostedService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return SeedSMSCoreData(stoppingToken,false);
        }

        private async Task SeedSMSCoreData(CancellationToken cancellationToken, bool shouldRun)
        {
            if (shouldRun)
            {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await dbContext.Database.MigrateAsync(cancellationToken);

                // await dbContext.Database.ExecuteSqlRawAsync(@"TRUNCATE TABLE ""SMS"".""AppRoles""",cancellationToken);

                //await TruncateSchemaAsync(dbContext, "SMS", cancellationToken);

                var tables = new List<string>();

                var permissionConfigsSeeded = await SeedPermissionConfigsAsync(dbContext, cancellationToken);
                if (permissionConfigsSeeded) tables.Add("PermissionConfigs");

                var rolePermissionsSeeded = await SeedRolePermissionsAsync(dbContext, cancellationToken);
                if (rolePermissionsSeeded) tables.Add("RolePermissions");

                var deptsSeeded = await SeedDepartmentsAsync(dbContext, cancellationToken);
                if (deptsSeeded) tables.Add("Departments");

                await dbContext.SaveChangesAsync(cancellationToken);
                _logger.LogSeededTables([.. tables]);
            }

        }
        public async Task TruncateSchemaAsync(DbContext dbContext, string schemaName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(schemaName) || schemaName.All(char.IsDigit))
                throw new ArgumentException("Invalid schema name provided.", nameof(schemaName));

            var sql = $@"
        DO $$
        DECLARE
            r RECORD;
        BEGIN
            FOR r IN (
                SELECT tablename
                FROM pg_tables
                WHERE schemaname = '{schemaName}'
            )
            LOOP
                EXECUTE format('TRUNCATE TABLE ""{schemaName}"".""%s"" RESTART IDENTITY CASCADE', r.tablename);
            END LOOP;
        END$$;";

            await dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);
        }


        private async Task<bool> SeedPermissionConfigsAsync(AppDbContext dbContext, CancellationToken cancellationToken)
        {
            var configs = await PermissionSeed.GetPermissionConfigsAsync(cancellationToken);
            var newPermissions = new List<PermissionConfig>();

            foreach (var p in configs)
            {
                if (!await dbContext.PermissionConfigs.AnyAsync(i => i.Id == p.Id, cancellationToken))
                {
                    newPermissions.Add(p);
                }
            }

            if (newPermissions.Any())
            {
                await dbContext.PermissionConfigs.AddRangeAsync(newPermissions, cancellationToken);
                return true;
            }

            return false;
        }

        private async Task<bool> SeedRolePermissionsAsync(AppDbContext dbContext, CancellationToken cancellationToken)
        {
            var configs = await PermissionSeed.GetPermissionConfigsAsync(cancellationToken);
            var newRolePermissions = new List<RolePermission>();

            var sysId = "01JX3BYWMF1AWD900K93VEFC6A";
            var systemRole = new Role();

            if (!await dbContext.Roles.AnyAsync(i => i.Id == sysId, cancellationToken))
            {
                 systemRole = new Role
                {
                    Id = sysId,
                    Name = "System",
                    Description = "System Role with System permissions",
                    IsActive = true,
                    CreatedByUserId = sysId,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    UpdatedByUserId = sysId,
                };
                await dbContext.Roles.AddAsync(systemRole, cancellationToken);
            }


            foreach (var p in configs)
            {
                if (!await dbContext.RolePermissions.AnyAsync(i => i.PermissionId == p.Id, cancellationToken))
                {
                    newRolePermissions.Add(new RolePermission
                    {
                        IsActive = true,
                        CreatedByUserId = sysId,
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow,
                        Description = p.Description,
                        Id = Ulid.NewUlid().ToString(),
                        ParentId = p.ParentId.Value,
                        PermissionId = p.Id,
                        PermissionName = p.Name,
                        RoleId = systemRole.Id,
                        UpdatedByUserId = sysId,
                    });
                }
            }

            if (newRolePermissions.Any())
            {
                await dbContext.RolePermissions.AddRangeAsync(newRolePermissions, cancellationToken);
                return true;
            }

            return false;
        }

        private async Task<bool> SeedDepartmentsAsync(AppDbContext dbContext, CancellationToken cancellationToken)
        {
            var sysId = Ulid.NewUlid().ToString();
            var dpetMenuSections = Enum.GetValues(typeof(MenuSection))
                            .Cast<MenuSection>()
                            .Select((section, index) => new Department
                            {
                                Id = index + 1,
                                Name = GetValidDescription(section.ToString()),
                                MenuSectionMap = section,
                                IsActive = true,
                                CreatedByUserId = sysId,
                                DateCreated = DateTime.UtcNow,
                                DateUpdated = DateTime.UtcNow,
                                Description = GetValidDescription(section.ToString()),
                                UpdatedByUserId = sysId,
                            })
                            .ToList();

            var newDepartments = new List<Department>();

            foreach (var p in dpetMenuSections)
            {
                if (!await dbContext.Departments.AnyAsync(i => i.Id == p.Id, cancellationToken))
                {
                    if (p.Name.Equals("None", StringComparison.OrdinalIgnoreCase))
                        continue;
                    newDepartments.Add(p);
                }
            }

            if (newDepartments.Any())
            {
                await dbContext.Departments.AddRangeAsync(newDepartments, cancellationToken);
                return true;
            }

            return false;
        }

        private string GetValidDescription(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return Regex.Replace(input, "(?<!^)([A-Z])", " $1");
        }
    }

    public static partial class SMSCoreStartupHostedServiceLog
    {
        [LoggerMessage(
            EventId = 1,
            Level = LogLevel.Information,
            Message = "Seed SMSCore data")]
        public static partial void LogStarter(this ILogger logger);

        [LoggerMessage(
            EventId = 2,
            Level = LogLevel.Information,
            Message = "The following tables were seeded: {tables}")]
        public static partial void LogSeededTables(this ILogger logger,string[] tables);
    }
}
