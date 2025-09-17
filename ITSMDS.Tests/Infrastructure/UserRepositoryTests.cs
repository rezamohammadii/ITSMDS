

using ITSMDS.Domain.Entities;
using ITSMDS.Domain.Tools;
using ITSMDS.Infrastructure.Database;
using ITSMDS.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;


namespace ITSMDS.Tests.Infrastructure;



public class UserRepositoryTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
            .Options;
        return new ApplicationDbContext(options);
    }
    private User CreateTestUser(string username = "tester", int personalCode = 123)
    {
        return new User(
            firstName: "Test",
            lastName: "User",
            email: "test@test.com",
            personalCode: personalCode,
            phoneNumber: "09123456789",
            userName: username,
            password: HashGenerator.GenerateHashSHA512("password"),
            teamName: "TestTeam",
            ipAddress: "127.0.0.1",
            hashId : Guid.NewGuid().ToString()

        );
    }
  
    private Permission CreateTestPermission(string name = "TestPermission")
    {
        return new Permission(name, "test", true, false);
        
    }

    #region UserMethod Tests

    [Fact]
    public async Task AddUserAsync_ShouldAddUser()
    {
        var db = GetDbContext();
        var repo = new UserRepository(db);
        var user = CreateTestUser();

        await repo.AddUserAsync(user);
        await db.SaveChangesAsync();

        var added = await db.Users.FirstOrDefaultAsync(x => x.UserName == user.UserName);
        Assert.NotNull(added);
        Assert.Equal(user.UserName, added.UserName);
    }

    [Fact]
    public async Task CheckUserExsitAsync_ShouldReturnTrue_WhenUserNameExists()
    {
        var db = GetDbContext();
        db.Users.Add(CreateTestUser("existingUser"));
        await db.SaveChangesAsync();
        var repo = new UserRepository(db);

        bool exists = await repo.CheckUserExsitAsync(userName: "existingUser");

        Assert.True(exists);
    }

    [Fact]
    public async Task CheckUserExsitAsync_ShouldReturnFalse_WhenUserNameDoesNotExist()
    {
        var db = GetDbContext();
        var repo = new UserRepository(db);

        bool exists = await repo.CheckUserExsitAsync(userName: "unknown");

        Assert.False(exists);
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnAllActiveUsers()
    {
        var db = GetDbContext();
        db.Users.Add(CreateTestUser("user1"));
        db.Users.Add(CreateTestUser("user2"));
        await db.SaveChangesAsync();
        var repo = new UserRepository(db);

        var users = await repo.GetAllUsersAsync(CancellationToken.None);

        Assert.Equal(2, users.Count);
        Assert.Contains(users, x => x.UserName == "user1");
        Assert.Contains(users, x => x.UserName == "user2");
    }

    [Fact]
    public async Task GetUserByPersonalCodeAsync_ShouldReturnCorrectUser()
    {
        var db = GetDbContext();
        var user = CreateTestUser(personalCode: 555);
        db.Users.Add(user);
        await db.SaveChangesAsync();
        var repo = new UserRepository(db);

        var result = await repo.GetUserByPersonalCodeAsync(555);

        Assert.NotNull(result);
        Assert.Equal(555, result.PersonalCode);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldUpdateUserInfo()
    {
        var db = GetDbContext();
        var user = CreateTestUser(username: "toUpdate");
        db.Users.Add(user);
        await db.SaveChangesAsync();
        var repo = new UserRepository(db);

        user.FirstName = "UpdatedName";
        var updated = await repo.UpdateUserAsync(user);

        Assert.NotNull(updated);
        Assert.Equal("UpdatedName", updated.FirstName);
    }

    [Fact]
    public async Task GetUserByUsernameAsync_ShouldReturnUser()
    {
        var db = GetDbContext();
        var user = CreateTestUser("myUser");
        db.Users.Add(user);
        await db.SaveChangesAsync();
        var repo = new UserRepository(db);

        var result = await repo.GetUserByUsernameAsync("myUser");

        Assert.NotNull(result);
        Assert.Equal("myUser", result.UserName);
    }

    #endregion

    #region PermissionMethod Tests

    [Fact]
    public async Task GetPermissionListAsync_ShouldReturnActivePermissions()
    {
        var db = GetDbContext();
        db.Permissions.Add(CreateTestPermission("perm1"));
        db.Permissions.Add(CreateTestPermission("perm2"));
        await db.SaveChangesAsync();
        var repo = new UserRepository(db);

        var perms = await repo.GetPermissionListAsync();

        Assert.Equal(2, perms.Count);
        Assert.Contains(perms, x => x.Name == "perm1");
        Assert.Contains(perms, x => x.Name == "perm2");
    }

    [Fact]
    public async Task GetPermissionByNameAsync_ShouldReturnPermission()
    {
        var db = GetDbContext();
        db.Permissions.Add(CreateTestPermission("permX"));
        await db.SaveChangesAsync();
        var repo = new UserRepository(db);

        var perm = await repo.GetPermissionByNameAsync("permX");

        Assert.NotNull(perm);
        Assert.Equal("permX", perm.Name);
    }

    #endregion
}
