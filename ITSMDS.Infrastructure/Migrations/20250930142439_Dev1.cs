using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITSMDS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Dev1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PortServices");

            migrationBuilder.DropTable(
                name: "Ports");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "modidied_time",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 9, 30, 14, 24, 38, 663, DateTimeKind.Unspecified).AddTicks(2199), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 9, 30, 10, 47, 8, 73, DateTimeKind.Unspecified).AddTicks(4597), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<string>(
                name: "hash_Id",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "5e8eba28-e298-42f8-b9dd-fd9d057e84ae",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldDefaultValue: "614a7877-77ba-40a2-a155-a805605100e2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "create_date",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 9, 30, 14, 24, 38, 663, DateTimeKind.Unspecified).AddTicks(1711), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 9, 30, 10, 47, 8, 73, DateTimeKind.Unspecified).AddTicks(4165), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "ExcutionPath",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "modidied_time",
                table: "Roles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 30, 14, 24, 38, 663, DateTimeKind.Utc).AddTicks(4955),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 9, 30, 10, 47, 8, 73, DateTimeKind.Utc).AddTicks(7688));

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_date",
                table: "Roles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 30, 14, 24, 38, 663, DateTimeKind.Utc).AddTicks(4577),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 9, 30, 10, 47, 8, 73, DateTimeKind.Utc).AddTicks(7295));

            migrationBuilder.AlterColumn<DateTime>(
                name: "modidied_time",
                table: "Permissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 30, 14, 24, 38, 663, DateTimeKind.Utc).AddTicks(5952),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 9, 30, 10, 47, 8, 73, DateTimeKind.Utc).AddTicks(8623));

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_date",
                table: "Permissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 30, 14, 24, 38, 663, DateTimeKind.Utc).AddTicks(5567),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 9, 30, 10, 47, 8, 73, DateTimeKind.Utc).AddTicks(8268));

            migrationBuilder.AlterColumn<string>(
                name: "department_id",
                table: "Departments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                defaultValue: "a45a795c-2235-4fad-bcdc-11153e7f3eed",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldDefaultValue: "588a024b-fdac-453c-b714-90924326f9eb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExcutionPath",
                table: "Services");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "modidied_time",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 9, 30, 10, 47, 8, 73, DateTimeKind.Unspecified).AddTicks(4597), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 9, 30, 14, 24, 38, 663, DateTimeKind.Unspecified).AddTicks(2199), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<string>(
                name: "hash_Id",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "614a7877-77ba-40a2-a155-a805605100e2",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldDefaultValue: "5e8eba28-e298-42f8-b9dd-fd9d057e84ae");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "create_date",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 9, 30, 10, 47, 8, 73, DateTimeKind.Unspecified).AddTicks(4165), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 9, 30, 14, 24, 38, 663, DateTimeKind.Unspecified).AddTicks(1711), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTime>(
                name: "modidied_time",
                table: "Roles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 30, 10, 47, 8, 73, DateTimeKind.Utc).AddTicks(7688),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 9, 30, 14, 24, 38, 663, DateTimeKind.Utc).AddTicks(4955));

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_date",
                table: "Roles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 30, 10, 47, 8, 73, DateTimeKind.Utc).AddTicks(7295),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 9, 30, 14, 24, 38, 663, DateTimeKind.Utc).AddTicks(4577));

            migrationBuilder.AlterColumn<DateTime>(
                name: "modidied_time",
                table: "Permissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 30, 10, 47, 8, 73, DateTimeKind.Utc).AddTicks(8623),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 9, 30, 14, 24, 38, 663, DateTimeKind.Utc).AddTicks(5952));

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_date",
                table: "Permissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 30, 10, 47, 8, 73, DateTimeKind.Utc).AddTicks(8268),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 9, 30, 14, 24, 38, 663, DateTimeKind.Utc).AddTicks(5567));

            migrationBuilder.AlterColumn<string>(
                name: "department_id",
                table: "Departments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                defaultValue: "588a024b-fdac-453c-b714-90924326f9eb",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldDefaultValue: "a45a795c-2235-4fad-bcdc-11153e7f3eed");

            migrationBuilder.CreateTable(
                name: "Ports",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    port_number = table.Column<int>(type: "int", maxLength: 6, nullable: false),
                    protocol = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    risk_level = table.Column<int>(type: "int", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PortServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PortId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PortServices_Ports_PortId",
                        column: x => x.PortId,
                        principalTable: "Ports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PortServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PortServices_PortId",
                table: "PortServices",
                column: "PortId");

            migrationBuilder.CreateIndex(
                name: "IX_PortServices_ServiceId",
                table: "PortServices",
                column: "ServiceId");
        }
    }
}
