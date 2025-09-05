
using System.ComponentModel.DataAnnotations;
using ITSMDS.Domain.Common;

namespace ITSMDS.Domain.Entities;

public class ServiceEntity : Entity<long>
{
    public string Name { get; private set; }
    public string? Version { get; private set; }
    public int CriticalityScore { get; private set; }
    public long ServerId { get; private set; }

    // Navigation properties
    public virtual ServerEntity Server { get; private set; }
    public virtual IReadOnlyCollection<PortService> PortServices => _portServices.AsReadOnly();
    private readonly List<PortService> _portServices = new();

    // Private constructor for EF Core
    private ServiceEntity() { }

    public ServiceEntity(string name, string? version, int criticalityScore, long serverId)
    {
        Validate(name, criticalityScore, serverId);

        Name = name;
        Version = version;
        CriticalityScore = criticalityScore;
        ServerId = serverId;
    }

    private static void Validate(string name, int criticalityScore, long serverId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidOperationException("Service name cannot be empty");

        if (criticalityScore < 0 || criticalityScore > 100)
            throw new InvalidOperationException("Criticality score must be between 0 and 100");

        if (serverId <= 0)
            throw new InvalidOperationException("Server ID must be a positive number");
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

    public void UpdateCriticalityScore(int score)
    {
        if (score < 0 || score > 100)
            throw new InvalidOperationException("Criticality score must be between 0 and 100");

        CriticalityScore = score;
    }

    public void ChangeServer(int serverId)
    {
        if (serverId <= 0)
            throw new InvalidOperationException("Server ID must be a positive number");

        ServerId = serverId;
    }

    public void AddPortService(PortService portService)
    {
        if (portService == null)
            throw new InvalidOperationException("Port service cannot be null");

        _portServices.Add(portService);
    }

    public void RemovePortService(PortService portService)
    {
        if (portService == null)
            throw new InvalidOperationException("Port service cannot be null");

        _portServices.Remove(portService);
    }

    public void ClearPortServices()
    {
        _portServices.Clear();
    }
}
