using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBeautyHubData.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionEntryFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Transaction_Status",
                table: "Transactions");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Transactions",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "pending",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Draft");

            migrationBuilder.AlterColumn<Guid>(
                name: "TransactionTypeId",
                table: "TransactionsDetails",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(name: "Code", table: "Transactions", type: "character varying(20)", maxLength: 20, nullable: true);
            migrationBuilder.AddColumn<string>(name: "Type", table: "Transactions", type: "character varying(20)", maxLength: 20, nullable: true);
            migrationBuilder.AddColumn<Guid>(name: "BranchId", table: "Transactions", type: "uuid", nullable: true);
            migrationBuilder.AddColumn<string>(name: "PaymentMode", table: "Transactions", type: "character varying(30)", maxLength: 30, nullable: true);
            migrationBuilder.AddColumn<string>(name: "CustomerName", table: "Transactions", type: "character varying(150)", maxLength: 150, nullable: true);
            migrationBuilder.AddColumn<string>(name: "CustomerMobile", table: "Transactions", type: "character varying(20)", maxLength: 20, nullable: true);
            migrationBuilder.AddColumn<string>(name: "Remark", table: "Transactions", type: "character varying(1000)", maxLength: 1000, nullable: true);
            migrationBuilder.AddColumn<Guid>(name: "StaffId", table: "Transactions", type: "uuid", nullable: true);
            migrationBuilder.AddColumn<string>(name: "CouponCode", table: "Transactions", type: "character varying(50)", maxLength: 50, nullable: true);
            migrationBuilder.AddColumn<string>(name: "CouponType", table: "Transactions", type: "character varying(20)", maxLength: 20, nullable: true);
            migrationBuilder.AddColumn<decimal>(name: "CouponValue", table: "Transactions", type: "numeric(18,2)", nullable: true);
            migrationBuilder.AddColumn<decimal>(name: "CouponDiscount", table: "Transactions", type: "numeric(18,2)", nullable: false, defaultValue: 0m);
            migrationBuilder.AddColumn<string>(name: "IdempotencyKey", table: "Transactions", type: "character varying(80)", maxLength: 80, nullable: true);
            migrationBuilder.AddColumn<int>(name: "EditCount", table: "Transactions", type: "integer", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<DateTime>(name: "EditableUntil", table: "Transactions", type: "timestamp with time zone", nullable: true);
            migrationBuilder.AddColumn<string>(name: "LastEditedBy", table: "Transactions", type: "character varying(150)", maxLength: 150, nullable: true);
            migrationBuilder.AddColumn<DateTime>(name: "LastEditedAt", table: "Transactions", type: "timestamp with time zone", nullable: true);
            migrationBuilder.AddColumn<DateTime>(name: "PaidAt", table: "Transactions", type: "timestamp with time zone", nullable: true);
            migrationBuilder.AddColumn<decimal>(name: "TaxAmount", table: "Transactions", type: "numeric(18,2)", nullable: false, defaultValue: 0m);
            migrationBuilder.AddColumn<decimal>(name: "TaxPercentage", table: "Transactions", type: "numeric(5,2)", nullable: false, defaultValue: 0m);

            migrationBuilder.AddColumn<int>(name: "Quantity", table: "TransactionsDetails", type: "integer", nullable: false, defaultValue: 1);
            migrationBuilder.AddColumn<Guid>(name: "StaffId", table: "TransactionsDetails", type: "uuid", nullable: true);
            migrationBuilder.AddColumn<string>(name: "Title", table: "TransactionsDetails", type: "character varying(200)", maxLength: 200, nullable: true);
            migrationBuilder.AddColumn<decimal>(name: "BaseAmount", table: "TransactionsDetails", type: "numeric(18,2)", nullable: false, defaultValue: 0m);
            migrationBuilder.AddColumn<decimal>(name: "TaxPercentage", table: "TransactionsDetails", type: "numeric(5,2)", nullable: false, defaultValue: 0m);
            migrationBuilder.AddColumn<decimal>(name: "TaxAmount", table: "TransactionsDetails", type: "numeric(18,2)", nullable: false, defaultValue: 0m);
            migrationBuilder.AddColumn<decimal>(name: "DiscountPercentage", table: "TransactionsDetails", type: "numeric(5,2)", nullable: false, defaultValue: 0m);
            migrationBuilder.AddColumn<decimal>(name: "DiscountAmount", table: "TransactionsDetails", type: "numeric(18,2)", nullable: false, defaultValue: 0m);
            migrationBuilder.AddColumn<decimal>(name: "GrossAmount", table: "TransactionsDetails", type: "numeric(18,2)", nullable: false, defaultValue: 0m);
            migrationBuilder.AddColumn<decimal>(name: "NetAmount", table: "TransactionsDetails", type: "numeric(18,2)", nullable: false, defaultValue: 0m);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Transaction_Status",
                table: "Transactions",
                sql: "\"Status\" IN ('Draft', 'Posted', 'Cancelled', 'paid', 'pending')");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId_Code",
                table: "Transactions",
                columns: new[] { "AccountId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId_IdempotencyKey",
                table: "Transactions",
                columns: new[] { "AccountId", "IdempotencyKey" });

            migrationBuilder.CreateIndex(name: "IX_Transactions_BranchId", table: "Transactions", column: "BranchId");
            migrationBuilder.CreateIndex(name: "IX_Transactions_StaffId", table: "Transactions", column: "StaffId");
            migrationBuilder.CreateIndex(name: "IX_TransactionsDetails_StaffId", table: "TransactionsDetails", column: "StaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Branch_BranchId",
                table: "Transactions",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Staff_StaffId",
                table: "Transactions",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "StaffId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionsDetails_Staff_StaffId",
                table: "TransactionsDetails",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "StaffId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Transactions_Branch_BranchId", table: "Transactions");
            migrationBuilder.DropForeignKey(name: "FK_Transactions_Staff_StaffId", table: "Transactions");
            migrationBuilder.DropForeignKey(name: "FK_TransactionsDetails_Staff_StaffId", table: "TransactionsDetails");
            migrationBuilder.DropIndex(name: "IX_Transactions_AccountId_Code", table: "Transactions");
            migrationBuilder.DropIndex(name: "IX_Transactions_AccountId_IdempotencyKey", table: "Transactions");
            migrationBuilder.DropIndex(name: "IX_Transactions_BranchId", table: "Transactions");
            migrationBuilder.DropIndex(name: "IX_Transactions_StaffId", table: "Transactions");
            migrationBuilder.DropIndex(name: "IX_TransactionsDetails_StaffId", table: "TransactionsDetails");
            migrationBuilder.DropCheckConstraint(name: "CK_Transaction_Status", table: "Transactions");

            migrationBuilder.DropColumn(name: "Code", table: "Transactions");
            migrationBuilder.DropColumn(name: "Type", table: "Transactions");
            migrationBuilder.DropColumn(name: "BranchId", table: "Transactions");
            migrationBuilder.DropColumn(name: "PaymentMode", table: "Transactions");
            migrationBuilder.DropColumn(name: "CustomerName", table: "Transactions");
            migrationBuilder.DropColumn(name: "CustomerMobile", table: "Transactions");
            migrationBuilder.DropColumn(name: "Remark", table: "Transactions");
            migrationBuilder.DropColumn(name: "StaffId", table: "Transactions");
            migrationBuilder.DropColumn(name: "CouponCode", table: "Transactions");
            migrationBuilder.DropColumn(name: "CouponType", table: "Transactions");
            migrationBuilder.DropColumn(name: "CouponValue", table: "Transactions");
            migrationBuilder.DropColumn(name: "CouponDiscount", table: "Transactions");
            migrationBuilder.DropColumn(name: "IdempotencyKey", table: "Transactions");
            migrationBuilder.DropColumn(name: "EditCount", table: "Transactions");
            migrationBuilder.DropColumn(name: "EditableUntil", table: "Transactions");
            migrationBuilder.DropColumn(name: "LastEditedBy", table: "Transactions");
            migrationBuilder.DropColumn(name: "LastEditedAt", table: "Transactions");
            migrationBuilder.DropColumn(name: "PaidAt", table: "Transactions");
            migrationBuilder.DropColumn(name: "TaxAmount", table: "Transactions");
            migrationBuilder.DropColumn(name: "TaxPercentage", table: "Transactions");

            migrationBuilder.DropColumn(name: "Quantity", table: "TransactionsDetails");
            migrationBuilder.DropColumn(name: "StaffId", table: "TransactionsDetails");
            migrationBuilder.DropColumn(name: "Title", table: "TransactionsDetails");
            migrationBuilder.DropColumn(name: "BaseAmount", table: "TransactionsDetails");
            migrationBuilder.DropColumn(name: "TaxPercentage", table: "TransactionsDetails");
            migrationBuilder.DropColumn(name: "TaxAmount", table: "TransactionsDetails");
            migrationBuilder.DropColumn(name: "DiscountPercentage", table: "TransactionsDetails");
            migrationBuilder.DropColumn(name: "DiscountAmount", table: "TransactionsDetails");
            migrationBuilder.DropColumn(name: "GrossAmount", table: "TransactionsDetails");
            migrationBuilder.DropColumn(name: "NetAmount", table: "TransactionsDetails");

            migrationBuilder.AlterColumn<Guid>(
                name: "TransactionTypeId",
                table: "TransactionsDetails",
                type: "uuid",
                nullable: false,
                defaultValue: Guid.Empty,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Transactions",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Draft",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldDefaultValue: "pending");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Transaction_Status",
                table: "Transactions",
                sql: "\"Status\" IN ('Draft', 'Posted', 'Cancelled')");
        }
    }
}
