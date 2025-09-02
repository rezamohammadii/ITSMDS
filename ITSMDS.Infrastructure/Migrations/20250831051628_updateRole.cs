using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITSMDS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedTime",
                table: "Users",
                newName: "modidied_time");

            migrationBuilder.AlterColumn<string>(
                name: "phone_number",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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
                oldDefaultValue: "dd1a02e6-5ba4-4a8b-a1b5-ff518c8e5bab");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "create_date",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 8, 31, 5, 16, 23, 976, DateTimeKind.Unspecified).AddTicks(1903), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 8, 26, 8, 54, 43, 210, DateTimeKind.Unspecified).AddTicks(4763), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "modidied_time",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 8, 31, 5, 16, 23, 976, DateTimeKind.Unspecified).AddTicks(3307), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "create_date",
                table: "Roles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 8, 31, 5, 16, 23, 977, DateTimeKind.Utc).AddTicks(2924));

            migrationBuilder.AddColumn<DateTime>(
                name: "modidied_time",
                table: "Roles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 8, 31, 5, 16, 23, 977, DateTimeKind.Utc).AddTicks(3816));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "create_date",
                table: "Permissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 8, 31, 5, 16, 23, 977, DateTimeKind.Utc).AddTicks(5378));

            migrationBuilder.AddColumn<DateTime>(
                name: "modidied_time",
                table: "Permissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 8, 31, 5, 16, 23, 978, DateTimeKind.Utc).AddTicks(854));

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
                oldDefaultValue: "1673baee-a586-4a35-9434-e3b90932be3a");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "create_date",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "modidied_time",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "create_date",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "modidied_time",
                table: "Permissions");

            migrationBuilder.RenameColumn(
                name: "modidied_time",
                table: "Users",
                newName: "ModifiedTime");

            migrationBuilder.AlterColumn<int>(
                name: "phone_number",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "hash_Id",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "dd1a02e6-5ba4-4a8b-a1b5-ff518c8e5bab",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldDefaultValue: "4c4597db-d606-4eef-b97e-91e1270a5cb4");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "create_date",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 8, 26, 8, 54, 43, 210, DateTimeKind.Unspecified).AddTicks(4763), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 8, 31, 5, 16, 23, 976, DateTimeKind.Unspecified).AddTicks(1903), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedTime",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 8, 31, 5, 16, 23, 976, DateTimeKind.Unspecified).AddTicks(3307), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<string>(
                name: "department_id",
                table: "Departments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                defaultValue: "1673baee-a586-4a35-9434-e3b90932be3a",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldDefaultValue: "298c3838-31bb-4220-9d34-bd1591299457");
        }
    }
}
