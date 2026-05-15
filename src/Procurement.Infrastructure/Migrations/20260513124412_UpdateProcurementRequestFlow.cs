using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Procurement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProcurementRequestFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminComment",
                table: "ProcurementRequests",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AdminReviewedByUserId",
                table: "ProcurementRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerComment",
                table: "ProcurementRequests",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ManagerReviewedByUserId",
                table: "ProcurementRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementRequests_AdminReviewedByUserId",
                table: "ProcurementRequests",
                column: "AdminReviewedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementRequests_ManagerReviewedByUserId",
                table: "ProcurementRequests",
                column: "ManagerReviewedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcurementRequests_Users_AdminReviewedByUserId",
                table: "ProcurementRequests",
                column: "AdminReviewedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcurementRequests_Users_ManagerReviewedByUserId",
                table: "ProcurementRequests",
                column: "ManagerReviewedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcurementRequests_Users_AdminReviewedByUserId",
                table: "ProcurementRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcurementRequests_Users_ManagerReviewedByUserId",
                table: "ProcurementRequests");

            migrationBuilder.DropIndex(
                name: "IX_ProcurementRequests_AdminReviewedByUserId",
                table: "ProcurementRequests");

            migrationBuilder.DropIndex(
                name: "IX_ProcurementRequests_ManagerReviewedByUserId",
                table: "ProcurementRequests");

            migrationBuilder.DropColumn(
                name: "AdminComment",
                table: "ProcurementRequests");

            migrationBuilder.DropColumn(
                name: "AdminReviewedByUserId",
                table: "ProcurementRequests");

            migrationBuilder.DropColumn(
                name: "ManagerComment",
                table: "ProcurementRequests");

            migrationBuilder.DropColumn(
                name: "ManagerReviewedByUserId",
                table: "ProcurementRequests");
        }
    }
}
