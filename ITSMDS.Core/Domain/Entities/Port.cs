
using ITSMDS.Core.Domain.Common;
using ITSMDS.Domain.Enums;

namespace ITSMDS.Domain.Entities;

public class Port : Entity<long>, IAggregateRoot
{
    public int PortNumber { get; private set; }
    public RiskLevel RiskLevel { get; private set; }
    public string? Description { get; private set; }
    public string? Protocol { get; private set; }

    private readonly List<PortService> _portServices = new();
    public virtual IReadOnlyCollection<PortService> PortServices => _portServices.AsReadOnly();

    // Private constructor for EF Core
    private Port() { }

    public Port(int portNumber, string? description, string? protocol, RiskLevel riskLevel)
    {
        Validate(portNumber, protocol);

        PortNumber = portNumber;
        Description = description;
        Protocol = protocol;
        RiskLevel = riskLevel;
    }

    public void UpdatePortInfo(int portNumber, string? description, string? protocol, RiskLevel riskLevel)
    {
        Validate(portNumber, protocol);

        PortNumber = portNumber;
        Description = description;
        Protocol = protocol;
        RiskLevel = riskLevel;
    }

    public void UpdateRiskLevel(RiskLevel riskLevel)
    {
        RiskLevel = riskLevel;
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
    }

    public void UpdateProtocol(string? protocol)
    {
        if (string.IsNullOrWhiteSpace(protocol))
            throw new DomainException("Protocol cannot be empty");

        Protocol = protocol;
    }

    public void AddService(ServiceEntity service)
    {
        if (service == null)
            throw new DomainException("Service cannot be null");

        if (!_portServices.Any(ps => ps.ServiceId == service.Id))
        {
            _portServices.Add(new PortService(this, service));
        }
    }

    public void RemoveService(ServiceEntity service)
    {
        if (service == null)
            throw new DomainException("Service cannot be null");

        var portService = _portServices.FirstOrDefault(ps => ps.ServiceId == service.Id);
        if (portService != null)
        {
            _portServices.Remove(portService);
        }
    }

    public bool IsWellKnownPort() => PortNumber >= 0 && PortNumber <= 1023;
    public bool IsRegisteredPort() => PortNumber >= 1024 && PortNumber <= 49151;
    public bool IsDynamicPort() => PortNumber >= 49152 && PortNumber <= 65535;

    private static void Validate(int portNumber, string? protocol)
    {
        if (portNumber < 0 || portNumber > 65535)
            throw new DomainException("Port number must be between 0 and 65535");

        if (string.IsNullOrWhiteSpace(protocol))
            throw new DomainException("Protocol cannot be empty");

        if (!protocol.Equals("tcp", StringComparison.OrdinalIgnoreCase) &&
            !protocol.Equals("udp", StringComparison.OrdinalIgnoreCase))
            throw new DomainException("Protocol must be either TCP or UDP");
    }
}