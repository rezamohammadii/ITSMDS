using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITSMDS.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using ITSMDS.Core.Domain.Common;

public class User : Entity<long>, IAggregateRoot
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public int LoginAttempt { get; private set; }
    public int PersonalCode { get; private set; }
    public int PhoneNumber { get; private set; }
    public DateTimeOffset CreateDate { get; private set; }
    public DateTimeOffset ModifiedTime { get; private set; }
    public string UserName { get; private set; }
    public string Password { get; private set; }
    public string TeamName { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    private readonly List<UserRole> _userRoles = new();
    public virtual IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    private User() { }

    public User(
        string? firstName,
        string? lastName,
        string? email,
        int personalCode,
        int? phoneNumber,
        string userName,
        string password,
        string? teamName)
    {
        Validate(firstName, lastName, email, personalCode, phoneNumber, userName, password);

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PersonalCode = personalCode;
        PhoneNumber = phoneNumber.Value;
        UserName = userName;
        Password = password; // Note: In real application, password should be hashed
        TeamName = teamName;
        CreateDate = DateTimeOffset.UtcNow;
        ModifiedTime = DateTimeOffset.UtcNow;
        IsActive = true;
        IsDeleted = false;
        LoginAttempt = 0;
    }

    private static void Validate(string? firstName, string? lastName, string? email, int personalCode,
        int? phoneNumber, string userName, string password)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name cannot be empty");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name cannot be empty");

        if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            throw new DomainException("Valid email is required");

        if (personalCode <= 0)
            throw new DomainException("Personal code must be valid");

        if (phoneNumber <= 0)
            throw new DomainException("Phone number must be valid");

        if (string.IsNullOrWhiteSpace(userName))
            throw new DomainException("Username cannot be empty");

        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            throw new DomainException("Password must be at least 6 characters long");
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var mailAddress = new System.Net.Mail.MailAddress(email);
            return mailAddress.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public void UpdatePersonalInfo(string firstName, string lastName, string email, int phoneNumber, string teamName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name cannot be empty");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name cannot be empty");

        if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            throw new DomainException("Valid email is required");

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        TeamName = teamName;
        ModifiedTime = DateTimeOffset.UtcNow;
    }

    public void UpdateUserName(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new DomainException("Username cannot be empty");

        UserName = userName;
        ModifiedTime = DateTimeOffset.UtcNow;
    }

    public void ChangePassword(string newPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
            throw new DomainException("Password must be at least 6 characters long");

        Password = newPassword; // Note: Hash the password in real application
        ModifiedTime = DateTimeOffset.UtcNow;
    }

    public void IncrementLoginAttempt()
    {
        LoginAttempt++;
        ModifiedTime = DateTimeOffset.UtcNow;
    }

    public void ResetLoginAttempt()
    {
        LoginAttempt = 0;
        ModifiedTime = DateTimeOffset.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        ModifiedTime = DateTimeOffset.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        ModifiedTime = DateTimeOffset.UtcNow;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
        IsActive = false;
        ModifiedTime = DateTimeOffset.UtcNow;
    }

    public void Restore()
    {
        IsDeleted = false;
        ModifiedTime = DateTimeOffset.UtcNow;
    }

    public void AssignRole(Role role)
    {
        if (role == null)
            throw new DomainException("Role cannot be null");

        if (!_userRoles.Any(ur => ur.RoleId == role.Id))
        {
            _userRoles.Add(new UserRole(this, role));
        }
    }

    public void RemoveRole(Role role)
    {
        if (role == null)
            throw new DomainException("Role cannot be null");

        var userRole = _userRoles.FirstOrDefault(ur => ur.RoleId == role.Id);
        if (userRole != null)
        {
            _userRoles.Remove(userRole);
        }
    }

    public bool HasRole(string roleName)
    {
        return _userRoles.Any(ur => ur.Role.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
    }

    public void UpdatePersonalCode(int personalCode)
    {
        if (personalCode <= 0)
            throw new DomainException("Personal code must be valid");

        PersonalCode = personalCode;
        ModifiedTime = DateTimeOffset.UtcNow;
    }
}
