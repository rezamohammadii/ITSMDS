

namespace ITSMDS.Domain.Enums;

public enum ErrorCode
{
    // General
    UnknownError = 1000,
    DatabaseError = 1001,
    NetworkError = 1002,
    ServiceUnavailable = 1003,
    OperationTimeout = 1004,
    RoleCreateSuccessfully = 1005,
    RoleAsignToUserSuccessfully = 1006,
    UpdateFailed,
    CreateFailed,
    DeleteFailed,

    // Authorization and access
    InvalidCredentials = 1101,
    Unauthorized = 1102,
    Forbidden = 1103,
    SessionExpired = 1104,
    AccountLocked = 1105,
    TooManyRequests = 1106,
    LoginSuccessfully = 1107,

    // Validation data
    ValidationError = 1201,
    InvalidFormat = 1202,
    RequiredFieldMissing = 1203,
    OutOfRange = 1204,
    DuplicateData = 1205,

    // Source
    NotFound = 1301,
    AlreadyExists = 1302,
    ResourceConflict = 1303,
    ResourceLimitExceeded = 1304,
    ServerError = 1305,

    // File and upload
    FileTooLarge = 1401,
    UnsupportedFileType = 1402,
    FileUploadFailed = 1403,

    // Payment and finance
    PaymentFailed = 1501,
    PaymentDeclined = 1502,
    InsufficientBalance = 1503,

    // Security
    SuspiciousActivity = 1601,
    IpBlocked = 1602,
    AccessRestricted = 1603,
    IpNotAllowed = 1604,
    IpNotRange = 1605,
    InvalidIpAddress = 1606,

    // System
    ConfigError = 1701,
    DependencyFailure = 1702,
    MaintenanceMode = 1703
}

