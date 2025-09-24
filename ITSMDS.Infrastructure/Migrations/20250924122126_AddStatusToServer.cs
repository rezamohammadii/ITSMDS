using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITSMDS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_User_HashId",
            //    table: "Users");

            migrationBuilder.DropColumn(
                name: "is_enable",
                table: "Servers");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "modidied_time",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 9, 24, 12, 21, 25, 152, DateTimeKind.Unspecified).AddTicks(3435), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 8, 31, 5, 16, 23, 976, DateTimeKind.Unspecified).AddTicks(3307), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<string>(
                name: "hash_Id",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "046f2b54-80bd-4249-a533-f6a2ebaf0fbb",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldDefaultValue: "4c4597db-d606-4eef-b97e-91e1270a5cb4");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "create_date",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 9, 24, 12, 21, 25, 152, DateTimeKind.Unspecified).AddTicks(2987), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 8, 31, 5, 16, 23, 976, DateTimeKind.Unspecified).AddTicks(1903), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "Servers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "modidied_time",
                table: "Roles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 24, 12, 21, 25, 152, DateTimeKind.Utc).AddTicks(6695),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 8, 31, 5, 16, 23, 977, DateTimeKind.Utc).AddTicks(3816));

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_date",
                table: "Roles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 24, 12, 21, 25, 152, DateTimeKind.Utc).AddTicks(6257),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 8, 31, 5, 16, 23, 977, DateTimeKind.Utc).AddTicks(2924));

            migrationBuilder.AlterColumn<DateTime>(
                name: "modidied_time",
                table: "Permissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 24, 12, 21, 25, 152, DateTimeKind.Utc).AddTicks(7653),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 8, 31, 5, 16, 23, 978, DateTimeKind.Utc).AddTicks(854));

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_date",
                table: "Permissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 24, 12, 21, 25, 152, DateTimeKind.Utc).AddTicks(7312),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 8, 31, 5, 16, 23, 977, DateTimeKind.Utc).AddTicks(5378));

            migrationBuilder.AlterColumn<string>(
                name: "department_id",
                table: "Departments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                defaultValue: "6e7e6582-970f-40be-9e68-89c1627a42c4",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldDefaultValue: "298c3838-31bb-4220-9d34-bd1591299457");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "Servers");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "modidied_time",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 8, 31, 5, 16, 23, 976, DateTimeKind.Unspecified).AddTicks(3307), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 9, 24, 12, 21, 25, 152, DateTimeKind.Unspecified).AddTicks(3435), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<string>(
                name: "hash_Id",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "4c4597db-d606-4eef-b97e-91e1270a5cb4",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldDefaultValue: "046f2b54-80bd-4249-a533-f6a2ebaf0fbb");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "create_date",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 8, 31, 5, 16, 23, 976, DateTimeKind.Unspecified).AddTicks(1903), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 9, 24, 12, 21, 25, 152, DateTimeKind.Unspecified).AddTicks(2987), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<bool>(
                name: "is_enable",
                table: "Servers",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "modidied_time",
                table: "Roles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 8, 31, 5, 16, 23, 977, DateTimeKind.Utc).AddTicks(3816),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 9, 24, 12, 21, 25, 152, DateTimeKind.Utc).AddTicks(6695));

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_date",
                table: "Roles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 8, 31, 5, 16, 23, 977, DateTimeKind.Utc).AddTicks(2924),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 9, 24, 12, 21, 25, 152, DateTimeKind.Utc).AddTicks(6257));

            migrationBuilder.AlterColumn<DateTime>(
                name: "modidied_time",
                table: "Permissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 8, 31, 5, 16, 23, 978, DateTimeKind.Utc).AddTicks(854),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 9, 24, 12, 21, 25, 152, DateTimeKind.Utc).AddTicks(7653));

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_date",
                table: "Permissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 8, 31, 5, 16, 23, 977, DateTimeKind.Utc).AddTicks(5378),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 9, 24, 12, 21, 25, 152, DateTimeKind.Utc).AddTicks(7312));

            migrationBuilder.AlterColumn<string>(
                name: "department_id",
                table: "Departments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                defaultValue: "298c3838-31bb-4220-9d34-bd1591299457",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldDefaultValue: "6e7e6582-970f-40be-9e68-89c1627a42c4");

            //migrationBuilder.CreateIndex(
            //    name: "IX_User_HashId",
            //    table: "Users",
            //    column: "hash_Id",
            //    unique: true);
        }
    }
}
