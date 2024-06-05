using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmApp.Migrations
{
    /// <inheritdoc />
    public partial class Added_Product_ProductCategory_Relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppProducts_ProductCategoryId",
                table: "AppProducts",
                column: "ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppProducts_AppProductCategories_ProductCategoryId",
                table: "AppProducts",
                column: "ProductCategoryId",
                principalTable: "AppProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppProducts_AppProductCategories_ProductCategoryId",
                table: "AppProducts");

            migrationBuilder.DropIndex(
                name: "IX_AppProducts_ProductCategoryId",
                table: "AppProducts");
        }
    }
}
