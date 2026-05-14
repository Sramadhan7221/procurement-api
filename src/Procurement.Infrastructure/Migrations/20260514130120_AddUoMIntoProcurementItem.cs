using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Procurement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUoMIntoProcurementItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UoM",
                table: "ProcurementItems",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UoM",
                table: "ProcurementItems");
        }
    }
}
