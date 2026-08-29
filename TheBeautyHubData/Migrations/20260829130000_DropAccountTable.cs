using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TheBeautyHubData.Context;

#nullable disable

namespace TheBeautyHubData.Migrations
{
    [DbContext(typeof(BeautyHubDbContext))]
    [Migration("20260829130000_DropAccountTable")]
    public partial class DropAccountTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS ""Account"";");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
