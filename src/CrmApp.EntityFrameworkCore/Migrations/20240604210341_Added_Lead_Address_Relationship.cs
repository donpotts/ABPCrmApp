using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmApp.Migrations
{
    /// <inheritdoc />
    public partial class Added_Lead_Address_Relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppLeads_AddressId",
                table: "AppLeads",
                column: "AddressId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppLeads_AppAddresses_AddressId",
                table: "AppLeads",
                column: "AddressId",
                principalTable: "AppAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppLeads_AppAddresses_AddressId",
                table: "AppLeads");

            migrationBuilder.DropIndex(
                name: "IX_AppLeads_AddressId",
                table: "AppLeads");
        }
    }
}
