using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMS.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSettingsEntitiesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SettingId",
                schema: "SMS",
                table: "SchoolClasses",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolClasses_SettingId",
                schema: "SMS",
                table: "SchoolClasses",
                column: "SettingId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolClasses_ClassSettings_SettingId",
                schema: "SMS",
                table: "SchoolClasses",
                column: "SettingId",
                principalSchema: "SMS",
                principalTable: "ClassSettings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolClasses_ClassSettings_SettingId",
                schema: "SMS",
                table: "SchoolClasses");

            migrationBuilder.DropIndex(
                name: "IX_SchoolClasses_SettingId",
                schema: "SMS",
                table: "SchoolClasses");

            migrationBuilder.DropColumn(
                name: "SettingId",
                schema: "SMS",
                table: "SchoolClasses");
        }
    }
}
