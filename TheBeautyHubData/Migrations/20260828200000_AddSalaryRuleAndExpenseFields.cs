using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TheBeautyHubData.Context;

#nullable disable

namespace TheBeautyHubData.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(BeautyHubDbContext))]
    [Migration("20260828200000_AddSalaryRuleAndExpenseFields")]
    public partial class AddSalaryRuleAndExpenseFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowAdvanceRecovery",
                table: "SalaryRule",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SalaryRule",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "FixedSalary",
                table: "SalaryRule",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxRecoveryPerMonth",
                table: "SalaryRule",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyTarget",
                table: "SalaryRule",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalaryType",
                table: "SalaryRule",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "fixed");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "SalaryRule",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "active");

            migrationBuilder.AddColumn<decimal>(
                name: "TargetBonus",
                table: "SalaryRule",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllBranches",
                table: "ExpensesType",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ExpensesType",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ExpensesType",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "active");

            migrationBuilder.CreateTable(
                name: "ExpensesTypeBranch",
                columns: table => new
                {
                    ExpensesTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensesTypeBranch", x => new { x.ExpensesTypeId, x.BranchId });
                    table.ForeignKey(
                        name: "FK_ExpensesTypeBranch_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpensesTypeBranch_ExpensesType_ExpensesTypeId",
                        column: x => x.ExpensesTypeId,
                        principalTable: "ExpensesType",
                        principalColumn: "ExpensesTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpensesTypeBranch_BranchId",
                table: "ExpensesTypeBranch",
                column: "BranchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ExpensesTypeBranch");

            migrationBuilder.DropColumn(name: "AllowAdvanceRecovery", table: "SalaryRule");
            migrationBuilder.DropColumn(name: "Description", table: "SalaryRule");
            migrationBuilder.DropColumn(name: "FixedSalary", table: "SalaryRule");
            migrationBuilder.DropColumn(name: "MaxRecoveryPerMonth", table: "SalaryRule");
            migrationBuilder.DropColumn(name: "MonthlyTarget", table: "SalaryRule");
            migrationBuilder.DropColumn(name: "SalaryType", table: "SalaryRule");
            migrationBuilder.DropColumn(name: "Status", table: "SalaryRule");
            migrationBuilder.DropColumn(name: "TargetBonus", table: "SalaryRule");

            migrationBuilder.DropColumn(name: "AllBranches", table: "ExpensesType");
            migrationBuilder.DropColumn(name: "Description", table: "ExpensesType");
            migrationBuilder.DropColumn(name: "Status", table: "ExpensesType");
        }
    }
}
