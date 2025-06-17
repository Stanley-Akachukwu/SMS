
using Microsoft.EntityFrameworkCore;
using SMS.ApiService.Entities.Departments;
using SMS.ApiService.Entities.Roles;
using SMS.ApiService.Entities.Settings;
using SMS.ApiService.Persistence;
using SMS.ApiService.Persistence.DataSeed;
using SMS.Common.Dtos.Fees;
using SMS.Common.Dtos.Schools;
using SMS.Common.Enums;
using System.Text.RegularExpressions;
using static System.Collections.Specialized.BitVector32;
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

                var schSettingsSeeded = await SeedSchoolSettingsAsync(dbContext, cancellationToken);
                if (schSettingsSeeded) tables.Add("SchoolSettings");

                var classSettingsSeeded = await SeedClassSettingsAsync(dbContext, cancellationToken);
                if (classSettingsSeeded) tables.Add("ClassSettings");

                var feesSettingsSeeded = await FeeSettingsAsync(dbContext, cancellationToken);
                if (feesSettingsSeeded)
                {
                    tables.Add("FeesSettings"); 
                    tables.Add("FeesBreakdownSettings");
                }

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
        private async Task<bool> SeedSchoolSettingsAsync(AppDbContext dbContext, CancellationToken cancellationToken)
        {
            var sysId = Ulid.NewUlid().ToString();

            var settings = new List<SchoolSetting>
                        {
                            new SchoolSetting
                            {
                                Id = Ulid.NewUlid().ToString(),
                                Name = "2024/2025 Morning Schedule",
                                Description = "Morning schedule for junior schools.",
                                ClassStartTime = new TimeSpan(7, 30, 0),     // 7:30 AM
                                BreakStartTime = new TimeSpan(9, 45, 0),
                                BreakEndTime = new TimeSpan(10, 15, 0),
                                DismissalTime = new TimeSpan(13, 30, 0),
                                IsActive = true,
                                CreatedByUserId = sysId,
                                DateCreated = DateTime.UtcNow,
                                DateUpdated = DateTime.UtcNow,
                                UpdatedByUserId = sysId,
                            },
                            new SchoolSetting
                            {
                                Id = Ulid.NewUlid().ToString(),
                                Name = "2024/2025 Full-Day Program",
                                Description = "Standard full-day schedule for all students.",
                                ClassStartTime = new TimeSpan(8, 0, 0),      // 8:00 AM
                                BreakStartTime = new TimeSpan(11, 0, 0),
                                BreakEndTime = new TimeSpan(11, 30, 0),
                                DismissalTime = new TimeSpan(15, 0, 0),
                                 IsActive = true,
                                CreatedByUserId = sysId,
                                DateCreated = DateTime.UtcNow,
                                DateUpdated = DateTime.UtcNow,
                                UpdatedByUserId = sysId,
                            },
                            new SchoolSetting
                            {
                                Id = Ulid.NewUlid().ToString(),
                                Name = "Saturday Classes",
                                Description = "Modified schedule for weekend lessons.",
                                ClassStartTime = new TimeSpan(8, 30, 0),     // 8:30 AM
                                BreakStartTime = new TimeSpan(10, 0, 0),
                                BreakEndTime = new TimeSpan(10, 20, 0),
                                DismissalTime = new TimeSpan(12, 30, 0),
                                 IsActive = true,
                                CreatedByUserId = sysId,
                                DateCreated = DateTime.UtcNow,
                                DateUpdated = DateTime.UtcNow,
                                UpdatedByUserId = sysId,
                            },
                            new SchoolSetting
                            {
                                Id = Ulid.NewUlid().ToString(),
                                Name = "Summer Term Schedule",
                                Description = "Flexible summer term hours with extended breaks.",
                                ClassStartTime = new TimeSpan(9, 0, 0),      // 9:00 AM
                                BreakStartTime = new TimeSpan(10, 30, 0),
                                BreakEndTime = new TimeSpan(11, 15, 0),
                                DismissalTime = new TimeSpan(14, 0, 0),
                                 IsActive = true,
                                CreatedByUserId = sysId,
                                DateCreated = DateTime.UtcNow,
                                DateUpdated = DateTime.UtcNow,
                                UpdatedByUserId = sysId,
                            },
                            new SchoolSetting
                            {
                                Id = Ulid.NewUlid().ToString(),
                                Name = "Half-Day Program",
                                Description = "Shortened schedule for special events and assessments.",
                                ClassStartTime = new TimeSpan(8, 0, 0),      // 8:00 AM
                                BreakStartTime = new TimeSpan(9, 30, 0),
                                BreakEndTime = new TimeSpan(10, 0, 0),
                                DismissalTime = new TimeSpan(12, 0, 0),
                                 IsActive = true,
                                CreatedByUserId = sysId,
                                DateCreated = DateTime.UtcNow,
                                DateUpdated = DateTime.UtcNow,
                                UpdatedByUserId = sysId,
                            }
                        };
             

            if (!await dbContext.SchoolSettings.AnyAsync(cancellationToken))
            {
                await dbContext.SchoolSettings.AddRangeAsync(settings, cancellationToken);
                return true;
            }

            return false;
        }
        private async Task<bool> SeedClassSettingsAsync(AppDbContext dbContext, CancellationToken cancellationToken)
        {
            var sysId = Ulid.NewUlid().ToString();
            var classSettings = new List<ClassSetting>
            {
            new ClassSetting
            {
                Id = Ulid.NewUlid().ToString(),
                Name = "Primary 1",
                Description = "Entry-level class for ages 5–6",
                MaxPopulationSize = 30,
                MinNumberOfTeachers = 2,
                IsActive = true,
                CreatedByUserId = sysId,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                UpdatedByUserId = sysId,
            },
            new ClassSetting
            {
                Id = Ulid.NewUlid().ToString(),
                Name = "Primary 2",
                Description = "Second-level class for basic education",
                MaxPopulationSize = 35,
                MinNumberOfTeachers = 2,
                IsActive = true,
                CreatedByUserId = sysId,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                UpdatedByUserId = sysId,
            },
            new ClassSetting
            {
                Id = Ulid.NewUlid().ToString(),
                Name = "Junior Secondary 1",
                Description = "First year of junior high school",
                MaxPopulationSize = 40,
                MinNumberOfTeachers = 3,
                IsActive = true,
                CreatedByUserId = sysId,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                UpdatedByUserId = sysId,
            },
            new ClassSetting
            {
                Id = Ulid.NewUlid().ToString(),
                Name = "Senior Secondary 3",
                Description = "Final year before university entrance",
                MaxPopulationSize = 50,
                MinNumberOfTeachers = 4,
                IsActive = true,
                CreatedByUserId = sysId,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                UpdatedByUserId = sysId,
            },
            new ClassSetting
            {
                Id = Ulid.NewUlid().ToString(),
                Name = "Kindergarten",
                Description = "Introductory class for young learners",
                MaxPopulationSize = 20,
                MinNumberOfTeachers = 2,
                IsActive = true,
                CreatedByUserId = sysId,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                UpdatedByUserId = sysId,
            }
            };
            

            if (!await dbContext.ClassSettings.AnyAsync(cancellationToken))
            {
                await dbContext.ClassSettings.AddRangeAsync(classSettings, cancellationToken);
                return true;
            }

            return false;
        }
        private async Task<bool> FeeSettingsAsync(AppDbContext dbContext, CancellationToken cancellationToken)
        {
            var sysId = Ulid.NewUlid().ToString();
            var feeList = MockDataGenerator.GenerateFeesDtoList(5);
            var feesBreakdowns = feeList.SelectMany(c=>c.FeesBreakDown).ToList();

            if (!await dbContext.FeesSettingBreakdowns.AnyAsync(cancellationToken))
            {
                await dbContext.FeesSettingBreakdowns.AddRangeAsync(feesBreakdowns, cancellationToken);
                return true;
            }
            if (!await dbContext.FeesSettings.AnyAsync(cancellationToken))
            {
                await dbContext.FeesSettings.AddRangeAsync(feeList, cancellationToken);
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

    public class MockDataGenerator
    {
        private static readonly Random random = new Random();
        private static readonly string[] feeTypes = { "Tuition", "Registration", "Technology", "Activity", "Library", "Sports", "Lab", "Transportation" };
        private static readonly string[] descriptions = {
        "Mandatory fee for all students",
        "Optional fee for selected activities",
        "One-time payment for the semester",
        "Annual fee for facility maintenance",
        "Fee for specialized equipment usage"
    };

        public static FeesSetting GenerateFeesDto()
        {
            var sysId = Ulid.NewUlid().ToString();
            var feesDto = new FeesSetting
            {
                Id = Ulid.NewUlid().ToString(),
                Name = "Academic Fees " + (random.Next(2020, 2024)),
                Description = "Comprehensive fee structure for the academic year",
                TotalAmount = 0, // Will be calculated from breakdown
                IsActive = true,
                CreatedByUserId = sysId,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                UpdatedByUserId = sysId,
            };

            int breakdownItemsCount = random.Next(3, 8);
            for (int i = 0; i < breakdownItemsCount; i++)
            {
                var breakdownItem = GenerateFeesBreakdownDto();
                feesDto.FeesBreakDown.Add(breakdownItem);
                feesDto.TotalAmount += breakdownItem.Amount;
            }

            return feesDto;
        }

        public static FeesSettingBreakdown GenerateFeesBreakdownDto()
        {
            var sysId = Ulid.NewUlid().ToString();
            return new FeesSettingBreakdown
            {
                Id = Ulid.NewUlid().ToString(),
                Name = feeTypes[random.Next(feeTypes.Length)] + " Fee",
                Description = descriptions[random.Next(descriptions.Length)],
                Amount = Math.Round((decimal)(random.NextDouble() * 1000 + 50), 2), // Random amount between 50 and 1050
                IsActive = true,
                CreatedByUserId = sysId,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                UpdatedByUserId = sysId,
            };
        }

        public static List<FeesSetting> GenerateFeesDtoList(int count)
        {
            var list = new List<FeesSetting>();
            for (int i = 0; i < count; i++)
            {
                list.Add(GenerateFeesDto());
            }
            return list;
        }
    }
}
