
using ITSMDS.Core.Domain.Common;
using ITSMDS.Domain.Enums;

namespace ITSMDS.Domain.Entities;

public class ServerEntity : Entity<long>, IAggregateRoot
{
    public string ServerName { get; private set; }
    public int RAM { get; private set; }
    public string CPU { get; private set; }
    public string MainBoardModel { get; private set; }
    public int StorageSize { get; private set; }
    public StorageType StorageType { get; private set; }
    public string OS { get; private set; }
    public DateTimeOffset StartDate { get; private set; }
    public string IpAddress { get; private set; }
    public string Location { get; private set; }
    public bool IsDeleted { get; private set; }
    public bool IsEnable { get; private set; }

    public long? DepartmentId { get; private set; }
    public virtual Department? Department { get; private set; }

    private readonly List<ServiceEntity> _services = new();
    public virtual IReadOnlyCollection<ServiceEntity> Services => _services.AsReadOnly();

    private ServerEntity() { }

    public ServerEntity(
        string serverName,
        int ram,
        string cpu,
        string mainBoardModel,
        int storageSize,
        StorageType storageType,
        string os,
        DateTimeOffset startDate,
        string ipAddress,
        string location,
        bool isEnable = true)
    {
        Validate(serverName, ram, storageSize, ipAddress, location);

        ServerName = serverName;
        RAM = ram;
        CPU = cpu;
        MainBoardModel = mainBoardModel;
        StorageSize = storageSize;
        StorageType = storageType;
        OS = os;
        StartDate = startDate;
        IpAddress = ipAddress;
        Location = location;
        IsEnable = isEnable;
        IsDeleted = false;
    }

    private static void Validate(string serverName, int ram, int storageSize, string ipAddress, string location)
    {
        if (string.IsNullOrWhiteSpace(serverName))
            throw new DomainException("Server name cannot be empty");

        if (ram <= 0)
            throw new DomainException("RAM must be greater than 0");

        if (storageSize <= 0)
            throw new DomainException("Storage size must be greater than 0");

        if (string.IsNullOrWhiteSpace(ipAddress))
            throw new DomainException("IP address cannot be empty");

        if (string.IsNullOrWhiteSpace(location))
            throw new DomainException("Location cannot be empty");
    }

    public void UpdateServerName(string serverName)
    {
        if (string.IsNullOrWhiteSpace(serverName))
            throw new DomainException("Server name cannot be empty");

        ServerName = serverName;
    }

    public void UpdateHardwareSpecs(int ram, string cpu, string mainBoardModel, int storageSize, StorageType storageType)
    {
        if (ram <= 0)
            throw new DomainException("RAM must be greater than 0");

        if (storageSize <= 0)
            throw new DomainException("Storage size must be greater than 0");

        RAM = ram;
        CPU = cpu;
        MainBoardModel = mainBoardModel;
        StorageSize = storageSize;
        StorageType = storageType;
    }

    public void UpdateSoftware(string os)
    {
        OS = os;
    }

    public void UpdateNetworkInfo(string ipAddress, string location)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
            throw new DomainException("IP address cannot be empty");

        if (string.IsNullOrWhiteSpace(location))
            throw new DomainException("Location cannot be empty");

        IpAddress = ipAddress;
        Location = location;
    }

    public void Enable()
    {
        IsEnable = true;
    }

    public void Disable()
    {
        IsEnable = false;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
        IsEnable = false;
    }

    public void Restore()
    {
        IsDeleted = false;
    }

    public void AssignToDepartment(Department department)
    {
        Department = department ?? throw new DomainException("Department cannot be null");
        DepartmentId = department.Id;
    }

    public void RemoveFromDepartment()
    {
        Department = null;
        DepartmentId = null;
    }

    public void AddService(ServiceEntity service)
    {
        if (service == null)
            throw new DomainException("Service cannot be null");

        if (!_services.Any(s => s.Id == service.Id))
        {
            _services.Add(service);
        }
    }

    public void RemoveService(ServiceEntity service)
    {
        if (service == null)
            throw new DomainException("Service cannot be null");

        var existingService = _services.FirstOrDefault(s => s.Id == service.Id);
        if (existingService != null)
        {
            _services.Remove(existingService);
        }
    }

    public void UpdateStartDate(DateTimeOffset startDate)
    {
        StartDate = startDate;
    }
}
