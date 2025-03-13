using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CardReader.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAccessCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_AccessCards_AccessCardId",
                table: "Memberships");

            migrationBuilder.DropTable(
                name: "AccessCards");

            migrationBuilder.DropIndex(
                name: "IX_Memberships_AccessCardId",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "AccessCardId",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Memberships");

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "Memberships",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "Memberships");

            migrationBuilder.AddColumn<int>(
                name: "AccessCardId",
                table: "Memberships",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Memberships",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AccessCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CardNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessCards", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_AccessCardId",
                table: "Memberships",
                column: "AccessCardId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessCards_CardNumber",
                table: "AccessCards",
                column: "CardNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_AccessCards_AccessCardId",
                table: "Memberships",
                column: "AccessCardId",
                principalTable: "AccessCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
