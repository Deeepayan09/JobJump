using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobJump.Migrations
{
    /// <inheritdoc />
    public partial class SavedJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SavedJobs_UserId",
                table: "SavedJobs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedJobs_AspNetUsers_UserId",
                table: "SavedJobs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SavedJobs_AspNetUsers_UserId",
                table: "SavedJobs");

            migrationBuilder.DropIndex(
                name: "IX_SavedJobs_UserId",
                table: "SavedJobs");
        }
    }
}
