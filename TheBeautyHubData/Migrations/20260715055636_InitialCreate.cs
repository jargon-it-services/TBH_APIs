using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBeautyHubData.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tables already exist from EnsureCreatedAsync - this is a baseline migration
            // No changes needed
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No down migration for baseline
        }
    }
}
