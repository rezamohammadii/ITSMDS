using System.ComponentModel.DataAnnotations;
using ITSMDS.Domain.Common;
using ITSMDS.Domain.Enums;

namespace ITSMDS.Domain.Entities;

public class ServiceEntity : Entity<long>
{
    public string Name { get; private set; }
    public string? Version { get; private set; }
    public string? Description { get; private set; }
    public string? DocumentFilePath { get; private set; }
    public string? ExcutionPath { get; private set; }
    public ServiceEnum.CriticalityScore CriticalityScore { get; private set; }
    public int Port { get; private set; } 
    public long ServerId { get; private set; }
    public DateTimeOffset CreateTime { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }

    // Navigation properties
    public virtual ServerEntity Server { get; private set; }

    // Private constructor for EF Core
    private ServiceEntity() { }

    public ServiceEntity(string name, string? version, ServiceEnum.CriticalityScore criticalityScore, long serverId, int port)
    {
        Validate(name, serverId, port);

        Name = name;
        Version = version;
        CriticalityScore = criticalityScore;
        ServerId = serverId;
        Port = port;
        CreateTime = DateTimeOffset.UtcNow;
        IsActive = true;
        IsDeleted = false;
    }

    private static void Validate(string name, long serverId, int port)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidOperationException("Service name cannot be empty");

        if (serverId <= 0)
            throw new InvalidOperationException("Server ID must be a positive number");

        if (port < 1 || port > 65535)
            throw new InvalidOperationException("Port must be between 1 and 65535");
    }

    // Domain methods
    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidOperationException("Service name cannot be empty");

        Name = name;
    }

    public void UpdateVersion(string? version)
    {
        Version = version;
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
    }

    public void UpdateDocumentFilePath(string? documentFilePath)
    {
        DocumentFilePath = documentFilePath;
    }

    public void UpdateCriticalityScore(ServiceEnum.CriticalityScore criticalityScore)
    {
        if (!Enum.IsDefined(typeof(ServiceEnum.CriticalityScore), criticalityScore))
            throw new InvalidOperationException("Invalid criticality score value");

        CriticalityScore = criticalityScore;
    }

    public void UpdatePort(int port)
    {
        if (port < 1 || port > 65535)
            throw new InvalidOperationException("Port must be between 1 and 65535");

        Port = port;
    }

    public void ChangeServer(long serverId)
    {
        if (serverId <= 0)
            throw new InvalidOperationException("Server ID must be a positive number");

        ServerId = serverId;
    }

    // Soft delete method
    public void Delete()
    {
        IsDeleted = true;
        IsActive = false;
    }

    // Activate/Deactivate methods
    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}