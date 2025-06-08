using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SMS.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRoleEntitiesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                schema: "SMS",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Role",
                schema: "SMS",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Username",
                schema: "SMS",
                table: "Users",
                newName: "Email");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                schema: "SMS",
                table: "Users",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "SMS",
                table: "Users",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                schema: "SMS",
                table: "Users",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                schema: "SMS",
                table: "Users",
                type: "integer",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "SMS",
                table: "Users",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                schema: "SMS",
                table: "Users",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                schema: "SMS",
                table: "Users",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SurName",
                schema: "SMS",
                table: "Users",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserProfileId",
                schema: "SMS",
                table: "Users",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Departments",
                schema: "SMS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    MenuSectionMap = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DateUpdated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionConfigs",
                schema: "SMS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    StaffAssign = table.Column<bool>(type: "boolean", nullable: true),
                    ParentAssign = table.Column<bool>(type: "boolean", nullable: true),
                    AdminAssign = table.Column<bool>(type: "boolean", nullable: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: true),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DateUpdated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "SMS",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    RoleType = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DateUpdated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                schema: "SMS",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    FirstName = table.Column<string>(type: "character varying(65)", maxLength: 65, nullable: true),
                    Surname = table.Column<string>(type: "character varying(65)", maxLength: 65, nullable: true),
                    Gender = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DateUpdated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                schema: "SMS",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    PermissionId = table.Column<int>(type: "integer", maxLength: 128, nullable: false),
                    PermissionName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    ParentId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DateUpdated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "SMS",
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                schema: "SMS",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                schema: "SMS",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserProfileId",
                schema: "SMS",
                table: "Users",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId",
                schema: "SMS",
                table: "RolePermissions",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Departments_DepartmentId",
                schema: "SMS",
                table: "Users",
                column: "DepartmentId",
                principalSchema: "SMS",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                schema: "SMS",
                table: "Users",
                column: "RoleId",
                principalSchema: "SMS",
                principalTable: "Roles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserProfiles_UserProfileId",
                schema: "SMS",
                table: "Users",
                column: "UserProfileId",
                principalSchema: "SMS",
                principalTable: "UserProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Departments_DepartmentId",
                schema: "SMS",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                schema: "SMS",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserProfiles_UserProfileId",
                schema: "SMS",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Departments",
                schema: "SMS");

            migrationBuilder.DropTable(
                name: "PermissionConfigs",
                schema: "SMS");

            migrationBuilder.DropTable(
                name: "RolePermissions",
                schema: "SMS");

            migrationBuilder.DropTable(
                name: "UserProfiles",
                schema: "SMS");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "SMS");

            migrationBuilder.DropIndex(
                name: "IX_Users_DepartmentId",
                schema: "SMS",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                schema: "SMS",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserProfileId",
                schema: "SMS",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                schema: "SMS",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "SMS",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                schema: "SMS",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleId",
                schema: "SMS",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SurName",
                schema: "SMS",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                schema: "SMS",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Email",
                schema: "SMS",
                table: "Users",
                newName: "Username");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                schema: "SMS",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "SMS",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                schema: "SMS",
                table: "Users",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                schema: "SMS",
                table: "Users",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                schema: "SMS",
                table: "Users",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }
    }
}
