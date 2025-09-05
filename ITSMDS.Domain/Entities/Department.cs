
using System.ComponentModel.DataAnnotations;
using ITSMDS.Domain.Common;

namespace ITSMDS.Domain.Entities;

public class Department : Entity<long>, IAggregateRoot
{
    public string Name { get; private set; }
    public string? OwnerName { get; private set; }
    public string? LocalLocation { get; private set; }
    public string? DepartmentIdentifire { get; private set; }

    private readonly List<ServerEntity> _servers = new();
    public virtual IReadOnlyCollection<ServerEntity> Servers => _servers.AsReadOnly();

    private Department() { }

    public Department(string name, string? ownerName = null, string? localLocation = null, string? departmentId = null)
    {
        ValidateName(name);

        Name = name;
        OwnerName = ownerName;
        LocalLocation = localLocation;
        DepartmentIdentifire = departmentId;
    }

    public void UpdateName(string name)
    {
        ValidateName(name);
        Name = name;
    }

    public void UpdateOwner(string? ownerName)
    {
        OwnerName = ownerName;
    }

    public void UpdateLocation(string? localLocation)
    {
        LocalLocation = localLocation;
    }

    public void UpdateDepartmentId(string? departmentId)
    {
        DepartmentIdentifire = departmentId;
    }

    public void AddServer(ServerEntity server)
    {
        if (server == null)
            throw new DomainException("Server cannot be null");

        if (!_servers.Any(s => s.Id == server.Id))
        {
            _servers.Add(server);
            server.AssignToDepartment(this);
        }
    }

    public void RemoveServer(ServerEntity server)
    {
        if (server == null)
            throw new DomainException("Server cannot be null");

        var existingServer = _servers.FirstOrDefault(s => s.Id == server.Id);
        if (existingServer != null)
        {
            _servers.Remove(existingServer);
            server.RemoveFromDepartment();
        }
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Department name cannot be empty");
    }
}