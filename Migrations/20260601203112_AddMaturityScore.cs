using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gov_API.Migrations
{
    /// <inheritdoc />
    public partial class AddMaturityScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MaturityScore",
                table: "GovernmentEntities",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaturityScore",
                table: "GovernmentEntities");
        }
    }
}
