

namespace ITSMDS.Core.Application.DTOs;

public record CreateUserRequest(string email, string firstName,
              string lastName, string password, string? personalCode,
                 string? phoneNumber, string userName, string ipAddress);
public record UserResponse(string id, string email, string fName, 
    string lName, DateTimeOffset createDate, int? phoneNumber, 
    string ipAddress, string userName,  int? personalCode = null);