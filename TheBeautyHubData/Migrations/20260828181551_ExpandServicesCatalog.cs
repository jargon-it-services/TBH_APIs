using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBeautyHubData.Migrations
{
    /// <inheritdoc />
    public partial class ExpandServicesCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllBranches",
                table: "Services",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicableGender",
                table: "Services",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "unisex");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Services",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CommissionType",
                table: "Services",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "flat");

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionValue",
                table: "Services",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Services",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtraChargePerKm",
                table: "Services",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HomeServiceAvailable",
                table: "Services",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "HomeVisitCharges",
                table: "Services",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaterialCost",
                table: "Services",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OtherCost",
                table: "Services",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Services",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceRadiusKm",
                table: "Services",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Services",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "active");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Services",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "in_salon");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllBranches",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ApplicableGender",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CommissionType",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CommissionValue",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ExtraChargePerKm",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "HomeServiceAvailable",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "HomeVisitCharges",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "MaterialCost",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "OtherCost",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ServiceRadiusKm",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Services");
        }
    }
}
