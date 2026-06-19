using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobJump.Migrations
{
    /// <inheritdoc />
    public partial class UpdateJobApplicationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "JobApplications",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "JobApplications",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_UserId",
                table: "JobApplications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_AspNetUsers_UserId",
                table: "JobApplications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_AspNetUsers_UserId",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_UserId",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "JobApplications");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "JobApplications",
                newName: "EmployeeId");
        }
    }
}
