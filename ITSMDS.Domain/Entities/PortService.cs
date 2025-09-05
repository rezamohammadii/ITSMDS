
using ITSMDS.Domain.Common;

namespace ITSMDS.Domain.Entities;

public class PortService : Entity
{
    public long PortId { get; private set; }
    public virtual Port Port { get; private set; }

    public long ServiceId { get; private set; }
    public virtual ServiceEntity Service { get; private set; }

    private PortService() { }

    public PortService(Port port, ServiceEntity service)
    {
        Port = port ?? throw new DomainException("Port cannot be null");
        Service = service ?? throw new DomainException("Service cannot be null");
        PortId = port.Id;
        ServiceId = service.Id;
    }
}