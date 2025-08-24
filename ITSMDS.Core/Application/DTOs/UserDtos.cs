

namespace ITSMDS.Core.Application.DTOs;

public record CreateUserRequest(string password, string? userName = null, int? personalCode = null);
public record UserResponse(long id, string email, string fName, string lName, DateTimeOffset createDate, int? phoneNumber,  int? personalCode = null);