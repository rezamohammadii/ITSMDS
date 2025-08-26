using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITSMDS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addIpToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "create_date",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 8, 25, 5, 12, 16, 10, DateTimeKind.Unspecified).AddTicks(4179), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 8, 23, 7, 40, 22, 176, DateTimeKind.Unspecified).AddTicks(8195), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "hash_Id",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "8f7e67bc-76a6-4a65-b2f4-111d068202ca");

            migrationBuilder.AlterColumn<string>(
                name: "local_location",
                table: "Departments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2)",
                oldMaxLength: 2,
                oldNullable: true);

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
                oldDefaultValue: "5c8fc74c-d720-48f3-9f6f-c28d020273db");

            migrationBuilder.CreateIndex(
                name: "IX_User_ActiveDeleted",
                table: "Users",
                columns: new[] { "is_active", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "IX_User_HashId",
                table: "Users",
                column: "hash_Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_ActiveDeleted",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_User_HashId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "hash_Id",
                table: "Users");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "create_date",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 8, 23, 7, 40, 22, 176, DateTimeKind.Unspecified).AddTicks(8195), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 8, 25, 5, 12, 16, 10, DateTimeKind.Unspecified).AddTicks(4179), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<string>(
                name: "local_location",
                table: "Departments",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "department_id",
                table: "Departments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                defaultValue: "5c8fc74c-d720-48f3-9f6f-c28d020273db",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldDefaultValue: "c1696936-fb94-40ea-a057-ca2ee7463db4");
        }
    }
}
