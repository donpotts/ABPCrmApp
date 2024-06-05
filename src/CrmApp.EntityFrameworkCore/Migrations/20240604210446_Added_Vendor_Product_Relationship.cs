using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmApp.Migrations
{
    /// <inheritdoc />
    public partial class Added_Vendor_Product_Relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppVendors_ProductId",
                table: "AppVendors",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppVendors_AppProducts_ProductId",
                table: "AppVendors",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppVendors_AppProducts_ProductId",
                table: "AppVendors");

            migrationBuilder.DropIndex(
                name: "IX_AppVendors_ProductId",
                table: "AppVendors");
        }
    }
}
