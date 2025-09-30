using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITSMDS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    service_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    owner_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    local_location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    department_id = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, defaultValue: "588a024b-fdac-453c-b714-90924326f9eb")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    permission_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripption = table.Column<string>(type: "nvarchar(max)", maxLength: -1, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2025, 9, 30, 10, 47, 8, 73, DateTimeKind.Utc).AddTicks(8268)),
                    modidied_time = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2025, 9, 30, 10, 47, 8, 73, DateTimeKind.Utc).AddTicks(8623)),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ports",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    port_number = table.Column<int>(type: "int", maxLength: 6, nullable: false),
                    risk_level = table.Column<int>(type: "int", maxLength: 1, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    protocol = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripption = table.Column<string>(type: "nvarchar(max)", maxLength: -1, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2025, 9, 30, 10, 47, 8, 73, DateTimeKind.Utc).AddTicks(7295)),
                    modidied_time = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2025, 9, 30, 10, 47, 8, 73, DateTimeKind.Utc).AddTicks(7688)),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    hash_Id = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, defaultValue: "614a7877-77ba-40a2-a155-a805605100e2"),
                    first_name = table.Column<string>(type: "nvarchar(155)", maxLength: 155, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(155)", maxLength: 155, nullable: false),
                    email_address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LoginAttempt = table.Column<int>(type: "int", nullable: false),
                    personali_code = table.Column<int>(type: "int", nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2025, 9, 30, 10, 47, 8, 73, DateTimeKind.Unspecified).AddTicks(4165), new TimeSpan(0, 0, 0, 0, 0))),
                    modidied_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2025, 9, 30, 10, 47, 8, 73, DateTimeKind.Unspecified).AddTicks(4597), new TimeSpan(0, 0, 0, 0, 0))),
                    user_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", maxLength: -1, nullable: false),
                    team_name = table.Column<string>(type: "nvarchar(155)", maxLength: 155, nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    server_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ram_size = table.Column<int>(type: "int", nullable: false, comment: "base on GB"),
                    cpu_size = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "base on GB"),
                    mainboard_model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    storage_size = table.Column<int>(type: "int", nullable: false, comment: "base on GB"),
                    storage_type = table.Column<int>(type: "int", nullable: false),
                    os_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    start_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ip_address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    physical_location = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ServerManager = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_deeted = table.Column<bool>(name: "is_de;eted", type: "bit", nullable: false, defaultValue: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    UseageType = table.Column<int>(type: "int", nullable: false),
                    department_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Servers_Departments_department_id",
                        column: x => x.department_id,
                        principalTable: "Departments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    PermissionId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    service_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    version = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    criticality_score = table.Column<int>(type: "int", maxLength: 2, nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false),
                    ServerId = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Servers_department_id",
                table: "Servers",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServerId",
                table: "Services",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_User_ActiveDeleted",
                table: "Users",
                columns: new[] { "is_active", "is_deleted" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PortServices");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Ports");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Servers");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
