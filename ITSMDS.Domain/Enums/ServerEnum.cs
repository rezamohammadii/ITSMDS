

namespace ITSMDS.Domain.Enums;

public class ServerEnum
{
    public enum ServerStatus
    {
        Unknown = 0,
        Updating,
        Active,
        DeActive
    }
    public enum StorageType
    {
        HDD = 1,
        SSD,
        NVME
    }
    public enum ServerUseageType
    {
        Operational = 1,
        UAT = 2,
        Develop = 3,
    }
}
