using System.Text.Json.Serialization;

namespace ITSMDS.Web.ViewModel;

public partial class UserModel
{
    [JsonPropertyName("id")]
    public string HashId { get;  set; }
    [JsonPropertyName("fName")]
    public string FirstName { get; set; }
    [JsonPropertyName("lName")]
    public string LastName { get; set; }
    [JsonPropertyName("email")]

    public string Email { get; set; }
    [JsonPropertyName("phoneNumber")]

    public string PhoneNumber { get; set; }
    [JsonPropertyName("createDate")]
    public string CreateDate { get; set; }
    [JsonPropertyName("userName")]

    public string UserName { get; set; }
    public string TeamName { get; set; }
    [JsonPropertyName("ipAddress")]
    public string IpAddress { get; set; }
    public bool IsActive { get; set; }


    private int? _personalCode;

    [JsonPropertyName("personalCode")]
    public int? PersonalCode
    {
        get => _personalCode;
        set => _personalCode = value;
    }
    //[JsonPropertyName("personalCode")]

    public string PersonalCodeString
    {
        get => _personalCode?.ToString() ?? "";
        set
        {
            if (int.TryParse(value, out int result))
            {
                _personalCode = result;
            }
            else
            {
                _personalCode = null;
            }
        }
    }
}


public record EditUserModel(string email, string firstName,
              string lastName, string? personalCode,
                 string? phoneNumber, string userName, string ipAddress);