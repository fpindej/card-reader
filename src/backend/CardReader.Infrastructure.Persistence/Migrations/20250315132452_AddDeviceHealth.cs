using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CardReader.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviceHealth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceHealths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeviceId = table.Column<int>(type: "integer", nullable: false),
                    MaxAllocHeap = table.Column<int>(type: "integer", nullable: false),
                    MinFreeHeap = table.Column<int>(type: "integer", nullable: false),
                    FreeHeap = table.Column<int>(type: "integer", nullable: false),
                    Uptime = table.Column<int>(type: "integer", nullable: false),
                    FreeSketchSpace = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceHealths", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceHealths");
        }
    }
}
