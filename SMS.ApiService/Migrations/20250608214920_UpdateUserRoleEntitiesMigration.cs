using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMS.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserRoleEntitiesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserProfiles_UserProfileId",
                schema: "SMS",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserProfileId",
                schema: "SMS",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                schema: "SMS",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserProfileId",
                schema: "SMS",
                table: "Users",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserProfileId",
                schema: "SMS",
                table: "Users",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserProfiles_UserProfileId",
                schema: "SMS",
                table: "Users",
                column: "UserProfileId",
                principalSchema: "SMS",
                principalTable: "UserProfiles",
                principalColumn: "Id");
        }
    }
}
