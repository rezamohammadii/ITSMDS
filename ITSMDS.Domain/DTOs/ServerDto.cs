

using ITSMDS.Domain.Enums;

namespace ITSMDS.Domain.DTOs;

public class CreateServerRequest
{
    public string ServerName { get; set; }
    public int RAM { get; set; }
    public string CPU { get; set; }
    public string MainBoardModel { get; set; }
    public int StorageSize { get; set; }
    public StorageType StorageType { get; set; }
    public ServerStatus Status { get; set; }
    public string OS { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public string IpAddress { get; set; }
    public string Location { get; set; }
    public bool IsEnable { get; set; } = true;
    public long? DepartmentId { get; set; }
}

public class ServerDto
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
}

public class UpdateServerRequest
{
    public string ServerName { get; set; }
    public int RAM { get; set; }
    public string CPU { get; set; }
    public string MainBoardModel { get; set; }
    public int StorageSize { get; set; }
    public StorageType StorageType { get; set; }
    public ServerStatus Status { get; set; }
    public string OS { get; set; }
    public string IpAddress { get; set; }
    public string Location { get; set; }
    public bool IsEnable { get; set; }
    public long? DepartmentId { get; set; }
}
