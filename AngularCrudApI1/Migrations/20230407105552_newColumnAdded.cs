using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularCrudApI1.Migrations
{
    /// <inheritdoc />
    public partial class newColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isEdit",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isEdit",
                table: "Students");
        }
    }
}
