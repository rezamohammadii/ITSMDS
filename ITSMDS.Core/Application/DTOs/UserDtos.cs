

namespace ITSMDS.Core.Application.DTOs;

public record CreateUserRequest(string email, string firstName,
              string lastName, string password, string? personalCode,
                 string? phoneNumber, string userName, string ipAddress);


public class UpdateUserRequest
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PersonalCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string UserName { get; set; }
    public string IpAddress { get; set; }
}

public record UserResponse(string id, string email, string fName, 
    string lName, string createDate, string? phoneNumber, 
    string ipAddress, string userName,  int personalCode);