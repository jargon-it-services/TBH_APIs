using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TheBeautyHubData.Context;

#nullable disable

namespace TheBeautyHubData.Migrations
{
    [DbContext(typeof(BeautyHubDbContext))]
    [Migration("20260829140000_DropUserAndUserSession")]
    public partial class DropUserAndUserSession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            DropFk(migrationBuilder, "Staff", "FK_Staff_User_UserId");
            DropFk(migrationBuilder, "BranchEmployee", "FK_BranchEmployee_User_UserId");
            DropFk(migrationBuilder, "ExceptionLogs", "FK_ExceptionLogs_User_UserId");
            DropFk(migrationBuilder, "FirmDetails", "FK_FirmDetails_User_UserId");
            DropFk(migrationBuilder, "UserSessions", "FK_UserSessions_User_UserId");
            DropFk(migrationBuilder, "User", "FK_User_User_ManagerId");

            migrationBuilder.Sql(@"DROP TABLE IF EXISTS ""UserSessions"";");
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS ""User"";");
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
