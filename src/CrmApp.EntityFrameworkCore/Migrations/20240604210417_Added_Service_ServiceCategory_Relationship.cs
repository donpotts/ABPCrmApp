using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmApp.Migrations
{
    /// <inheritdoc />
    public partial class Added_Service_ServiceCategory_Relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppServices_ServiceCategoryId",
                table: "AppServices",
                column: "ServiceCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppServices_AppServiceCategories_ServiceCategoryId",
                table: "AppServices",
                column: "ServiceCategoryId",
                principalTable: "AppServiceCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppServices_AppServiceCategories_ServiceCategoryId",
                table: "AppServices");

            migrationBuilder.DropIndex(
                name: "IX_AppServices_ServiceCategoryId",
                table: "AppServices");
        }
    }
}
