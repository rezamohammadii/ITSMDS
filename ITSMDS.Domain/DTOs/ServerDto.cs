

using ITSMDS.Domain.Enums;

namespace ITSMDS.Domain.DTOs;

public class CreateServerRequest
{
    public string ServerName { get; set; }
    public int RAM { get; set; }
    public string CPU { get; set; }
    public string MainBoardModel { get; set; }
    public int StorageSize { get; set; }
    public ServerEnum.StorageType StorageType { get; set; }
    public ServerEnum.ServerStatus Status { get; set; }
    public ServerEnum.ServerUseageType ServerUseage { get; set; }
    public string OS { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public string IpAddress { get; set; }
    public string? Description { get; set; }
    public string? ServerManager { get; set; }
    public string Location { get; set; }
    public bool IsEnable { get; set; } = true;
    public long? DepartmentId { get; set; }
}

public class ServerDto
{
    public string ServerName { get;  set; }
    public int RAM { get;  set; }
    public string CPU { get;  set; }
    public string MainBoardModel { get;  set; }
    public int StorageSize { get;  set; }
    public ServerEnum.StorageType StorageType { get;  set; }
    public ServerEnum.ServerStatus Status { get; set; }
    public string OS { get;  set; }
    public string CreateDate { get;  set; }
    public string IpAddress { get;  set; }
    public string Location { get;  set; }
    public int Id { get;  set; }
    public bool IsEnable { get;  set; }
    public List<ServiceDto> Services { get; set; }
}

public class UpdateServerRequest
{
    public string ServerName { get; set; }
    public int RAM { get; set; }
    public string CPU { get; set; }
    public string MainBoardModel { get; set; }
    public int StorageSize { get; set; }
    public ServerEnum.StorageType StorageType { get; set; }
    public ServerEnum.ServerStatus Status { get; set; }
    public string OS { get; set; }
    public string IpAddress { get; set; }
    public string Location { get; set; }
    public bool IsEnable { get; set; }
    public long? DepartmentId { get; set; }
}

public class ServerWidget
{
    public int ServerActiveCount { get; set; }
    public int ServerUpPercentSalary { get; set; }
    public int ServerOperationalCount { get; set; }
    public int ServerOperationalPercentSalary { get; set; }
    public int ServerTestCount { get; set; }
    public int ServerTestPercentSalary { get; set; }
    public int ServerDevelopCount { get; set; }
    public int ServerDevelopPercentSalary { get; set; }
}
