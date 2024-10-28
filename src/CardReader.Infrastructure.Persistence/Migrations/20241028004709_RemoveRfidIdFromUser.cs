using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardReader.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRfidIdFromUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RfidId",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RfidId",
                table: "Users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
