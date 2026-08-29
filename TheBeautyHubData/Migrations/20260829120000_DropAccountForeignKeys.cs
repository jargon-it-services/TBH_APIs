using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TheBeautyHubData.Context;

#nullable disable

namespace TheBeautyHubData.Migrations
{
    [DbContext(typeof(BeautyHubDbContext))]
    [Migration("20260829120000_DropAccountForeignKeys")]
    public partial class DropAccountForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            DropFk(migrationBuilder, "Branch", "FK_Branch_Account_AccountId");
            DropFk(migrationBuilder, "ExpensesType", "FK_ExpensesType_Account_AccountId");
            DropFk(migrationBuilder, "Firm", "FK_Firm_Account_AccountId");
            DropFk(migrationBuilder, "FirmDetails", "FK_FirmDetails_Account_AccountId");
            DropFk(migrationBuilder, "Partner", "FK_Partner_Account_AccountId");
            DropFk(migrationBuilder, "ReportsForAccount", "FK_ReportsForAccount_Account_AccountId");
            DropFk(migrationBuilder, "SalaryRule", "FK_SalaryRule_Account_AccountId");
            DropFk(migrationBuilder, "Services", "FK_Services_Account_AccountId");
            DropFk(migrationBuilder, "Staff", "FK_Staff_Account_AccountId");
            DropFk(migrationBuilder, "Subscription", "FK_Subscription_Account_AccountId");
            DropFk(migrationBuilder, "TransactionRules", "FK_TransactionRules_Account_AccountId");
            DropFk(migrationBuilder, "Transactions", "FK_Transactions_Account_AccountId");
            DropFk(migrationBuilder, "TransactionsDetails", "FK_TransactionsDetails_Account_AccountId");
            DropFk(migrationBuilder, "User", "FK_User_Account_AccountId");
            DropFk(migrationBuilder, "Wallet", "FK_Wallet_Account_AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }

        private static void DropFk(MigrationBuilder migrationBuilder, string table, string constraint)
        {
            migrationBuilder.Sql($@"
DO $$ BEGIN
  IF EXISTS (
    SELECT 1 FROM information_schema.tables
    WHERE table_schema = current_schema() AND table_name = '{table}') THEN
    EXECUTE 'ALTER TABLE ""{table}"" DROP CONSTRAINT IF EXISTS ""{constraint}""';
  END IF;
END $$;");
        }
    }
}
