﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardReader.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviceHealthDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "DeviceHealths",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "DeviceHealths");
        }
    }
}
