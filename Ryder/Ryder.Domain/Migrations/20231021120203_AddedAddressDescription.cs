using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ryder.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddedAddressDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressDescription",
                table: "Address",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressDescription",
                table: "Address");
        }
    }
}
