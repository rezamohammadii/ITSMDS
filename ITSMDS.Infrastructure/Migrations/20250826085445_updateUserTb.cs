using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITSMDS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateUserTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "Users",
                type: "nvarchar(max)",
                maxLength: -1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

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
                oldDefaultValue: "8f7e67bc-76a6-4a65-b2f4-111d068202ca");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "create_date",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 8, 26, 8, 54, 43, 210, DateTimeKind.Unspecified).AddTicks(4763), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 8, 25, 5, 12, 16, 10, DateTimeKind.Unspecified).AddTicks(4179), new TimeSpan(0, 0, 0, 0, 0)));

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
                oldDefaultValue: "c1696936-fb94-40ea-a057-ca2ee7463db4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: -1);

            migrationBuilder.AlterColumn<string>(
                name: "hash_Id",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "8f7e67bc-76a6-4a65-b2f4-111d068202ca",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldDefaultValue: "dd1a02e6-5ba4-4a8b-a1b5-ff518c8e5bab");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "create_date",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 8, 25, 5, 12, 16, 10, DateTimeKind.Unspecified).AddTicks(4179), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 8, 26, 8, 54, 43, 210, DateTimeKind.Unspecified).AddTicks(4763), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<string>(
                name: "department_id",
                table: "Departments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                defaultValue: "c1696936-fb94-40ea-a057-ca2ee7463db4",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldDefaultValue: "1673baee-a586-4a35-9434-e3b90932be3a");
        }
    }
}
