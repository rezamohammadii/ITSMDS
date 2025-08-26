using System.Text.Json.Serialization;

namespace ITSMDS.Web.ViewModel;

public class UserModel
{
    [JsonPropertyName("id")]
    public string HashId { get;  set; }
    [JsonPropertyName("fName")]
    public string FirstName { get; set; }
    [JsonPropertyName("lName")]
    public string LastName { get; set; }
    public string Email { get; set; }
    public int PersonalCode { get; set; }
    public int PhoneNumber { get; set; }
    public DateTimeOffset CreateDate { get; set; }
    public string UserName { get; set; }
    public string TeamName { get; set; }
    [JsonPropertyName("ipAddress")]
    public string ipAddress { get; set; }
    public bool IsActive { get; set; }
}