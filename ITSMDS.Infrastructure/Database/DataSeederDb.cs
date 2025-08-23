
using System.Reflection;
using ITSMDS.Domain.Entities;
using ITSMDS.Domain.Enums;
using ITSMDS.Infrastructure.Constants;
using ITSMDS.Infrastructure.CustomAttribute;
using ITSMDS.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ITSMDS.Infrastructure.Database;

public static class DataSeederDb
{
    public static void SeedData(this ApplicationDbContext dbContext, ILogger logger = null)
    {
        var _logger = logger ?? NullLogger.Instance;

        try
        {
            // Seed Roles
            if (!dbContext.Roles.Any())
            {
                var newRole = new Role("SUPER_ADMIN", "this role has high access");
                dbContext.Roles.Add(newRole);
                dbContext.SaveChanges();
                _logger.LogInformation("Role {RoleName} created successfully.", newRole.Name);
            }

            // Seed Users
            if (!dbContext.Users.Any())
            {
                var getRoleAdmin = dbContext.Roles.First(x => x.Name == "SUPER_ADMIN");

                var newUser = new User(
                    firstName: "admin",
                    lastName: "system",
                    email: "super-admin@domain.com",
                    personalCode: 88378,
                    phoneNumber: 833254300,
                    userName: "super-admin",
                    password: "22eswsssr42@!#3", // ⚠️ Hash it in real projects!
                    teamName: "IT Management"
                );

                newUser.AssignRole(getRoleAdmin);
                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();
                _logger.LogInformation("User {UserName} with role {RoleName} created successfully.", newUser.UserName, getRoleAdmin.Name);
            }

            // Seed Departments
            if (!dbContext.Departments.Any())
            {
                var newDepartment = new Department(
                    name: "Central Information Technology Building",
                    ownerName: "admin",
                    localLocation: "ABC"
                );

                dbContext.Departments.Add(newDepartment);
                dbContext.SaveChanges();
                _logger.LogInformation("Department {DeptName} created successfully.", newDepartment.Name);
            }

            // Seed Servers
            if (!dbContext.Servers.Any())
            {
                var getDepartment = dbContext.Departments.First(x => x.OwnerName == "admin");

                var newServer = new ServerEntity(
                    serverName: "SRV-development",
                    ram: 256,
                    cpu: "Intel Xeon Platinum 8280 Processor",
                    mainBoardModel: "Asus",
                    storageSize: 1000,
                    storageType: Domain.Enums.StorageType.SSD,
                    os: "linux ubuntu 22.04",
                    startDate: DateTimeOffset.Parse("2025/01/01"),
                    ipAddress: "127.0.0.1",
                    location: "Tehran"
                );

                newServer.AssignToDepartment(getDepartment);
                dbContext.Servers.Add(newServer);
                dbContext.SaveChanges();
                _logger.LogInformation("Server {ServerName} created successfully.", newServer.ServerName);
            }

            // Seed Ports
            if (!dbContext.Ports.Any())
            {
                var listPort = new List<Port>
            {
                new Port(80, "Http", "tcp", RiskLevel.Medium),
                new Port(443, "Https", "tcp", RiskLevel.Low)
            };

                dbContext.Ports.AddRange(listPort);
                dbContext.SaveChanges();
                _logger.LogInformation("Ports {Ports} created successfully.", string.Join(",", listPort.Select(p => p.PortNumber)));
            }

            // Seed Services
            if (!dbContext.Services.Any())
            {
                var getServer = dbContext.Servers.First(x => x.ServerName == "SRV-development");
                var getPorts = dbContext.Ports.Where(x => x.Protocol == "tcp").ToList();

                var newService = new ServiceEntity(
                    name: "Service nginx",
                    version: "1.3.4",
                    criticalityScore: 5,
                    serverId: getServer.Id
                );

                foreach (var port in getPorts)
                {
                    newService.AddPortService(new PortService(port, newService));
                }

                dbContext.Services.Add(newService);
                dbContext.SaveChanges();
                _logger.LogInformation("Service {ServiceName} created successfully on server {ServerName}.", newService.Name, getServer.ServerName);
            }

            // Seed Permissions
            if (!dbContext.Permissions.Any())
            {
                var info = typeof(PermissionName);
                foreach (var field in info.GetFields())
                {
                    if (field.FieldType != typeof(string))
                        continue;

                    var permissionName = field.GetRawConstantValue() as string;
                    if (string.IsNullOrEmpty(permissionName))
                        continue;

                    string? permissionComment = null;
                    if (field.GetCustomAttribute(typeof(PermissionCommentAttribute)) is PermissionCommentAttribute attribute)
                    {
                        permissionComment = attribute.Comment;
                    }

                    var newPermission = new Permission(permissionName, permissionComment);
                    dbContext.Permissions.Add(newPermission);
                    _logger.LogInformation("Permission {PermissionName} created successfully.", permissionName);
                }
                dbContext.SaveChanges();
            }

            _logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

}
