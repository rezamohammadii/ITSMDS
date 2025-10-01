

using ITSMDS.Domain.Enums;

namespace ITSMDS.Domain.DTOs;

public class ServiceDto
{
    public string ServiceName { get;  set; }
    public string? Version { get;  set; }
    public string? Description { get;  set; }
    public string? DocumentFilePath { get;  set; }
    public string? ExcutionPath { get;  set; }
    public ServiceEnum.CriticalityScore CriticalityScore { get;  set; }
    public int Port { get;  set; }
    public string ServerName { get;  set; }
    public string? CreateTime { get; set; }
    public bool IsActive { get; set; }
}

public class ServiceWidget
{
    public int ServiceActiveCount { get; set; }
    public int ServiceHasDocumentCount { get; set; }
    public int ServiceAllCount { get; set; }
    public int ServiceDeActiveCount { get; set; }

}

