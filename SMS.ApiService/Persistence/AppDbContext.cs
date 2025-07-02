using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SMS.ApiService.Converters;
using SMS.ApiService.Entities.ClassUnits;
using SMS.ApiService.Entities.Departments;
using SMS.ApiService.Entities.Roles;
using SMS.ApiService.Entities.SchoolClasses;
using SMS.ApiService.Entities.Schools;
using SMS.ApiService.Entities.Settings;
using SMS.ApiService.Entities.Users;
using SMS.ApiService.Entities.Workflows;

namespace SMS.ApiService.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public const string Schema = "SMS";
        public DbSet<User> Users { get; set; }
        public DbSet<PermissionConfig> PermissionConfigs { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<SchoolSetting> SchoolSettings { get; set; }
        public DbSet<ClassSetting> ClassSettings { get; set; }
        public DbSet<FeesSetting> FeesSettings { get; set; }
        public DbSet<FeesSettingBreakdown> FeesSettingBreakdowns { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<SchoolClass> SchoolClasses { get; set; }
        public DbSet<ClassUnit> ClassUnits { get; set; }
        public DbSet<Workflow> Workflows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(Schema);

            modelBuilder.Entity<User>()
                        .HasOne(u => u.Role)
                        .WithMany(r => r.Users)
                        .HasForeignKey(u => u.RoleId)
                        .IsRequired(false);

            modelBuilder.Entity<User>()
                        .HasOne(u => u.Department)
                        .WithMany(d => d.Users)
                        .HasForeignKey(u => u.DepartmentId)
                        .IsRequired(false);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTimeOffset))
                    {
                        property.SetValueConverter(new ValueConverter<DateTimeOffset, DateTimeOffset>(
                            v => v.ToUniversalTime(),      // Store as UTC
                            v => DateTime.SpecifyKind(v.UtcDateTime, DateTimeKind.Utc))); // Read as UTC
                    }

                    if (property.ClrType == typeof(DateTimeOffset?))
                    {
                        property.SetValueConverter(new ValueConverter<DateTimeOffset?, DateTimeOffset?>(
                            v => v.HasValue ? v.Value.ToUniversalTime() : null,
                            v => v.HasValue ? DateTime.SpecifyKind(v.Value.UtcDateTime, DateTimeKind.Utc) : (DateTimeOffset?)null));
                    }
                }
            }
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
        .Properties<Ulid>()
        .HaveConversion<UlidToStringConverter>();

            configurationBuilder
                .Properties<Ulid?>()
                .HaveConversion<NullableUlidToStringConverter>();
        }
    }
}
