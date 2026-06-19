using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobJump.Migrations
{
    /// <inheritdoc />
    public partial class Addjon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_JobId",
                table: "JobApplications",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Jobs_JobId",
                table: "JobApplications",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Jobs_JobId",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_JobId",
                table: "JobApplications");
        }
    }
}
