using System.ComponentModel.DataAnnotations;
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
    [Required(ErrorMessage = "🛑 ایمیل الزامی است.")]
    [EmailAddress(ErrorMessage = "🛑 ایمیل معتبر وارد کنید.")]
    public string Email { get; set; }

    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber { get; set; }
    [JsonPropertyName("createDate")]
    public string CreateDate { get; set; }
    [JsonPropertyName("userName")]

    public string UserName { get; set; }
    public string TeamName { get; set; }
    [JsonPropertyName("ipAddress")]
    [Required(ErrorMessage = "🛑 IP الزامی است.")]
    [RegularExpression(@"^(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)){3}$",
        ErrorMessage = "🛑 IP وارد شده معتبر نیست.")]

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


public class UserModelIn
{
    [Required(ErrorMessage = "🛑 نام الزامی است.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "🛑  نام خانوادگی الزامی است.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "🛑  نام کاربری الزامی است.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "🛑 IP الزامی است.")]
    [RegularExpression(@"^(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)){3}$",
        ErrorMessage = "🛑 IP وارد شده معتبر نیست.")]
    public string IpAddress { get; set; }

    [Required(ErrorMessage = "🛑 ایمیل الزامی است.")]
    [EmailAddress(ErrorMessage = "🛑 ایمیل معتبر وارد کنید.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "🛑 رمز عبور الزامی است.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
        ErrorMessage = "🛑 رمز عبور باید حداقل ۸ کاراکتر و شامل حرف بزرگ، کوچک، عدد و علامت خاص باشد.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "🛑 رمز عبور الزامی است.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
        ErrorMessage = "🛑 رمز عبور باید حداقل ۸ کاراکتر و شامل حرف بزرگ، کوچک، عدد و علامت خاص باشد.")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "🛑 کد پرسنلی الزامی است.")]
    [RegularExpression(@"^\d{4,6}$", ErrorMessage = "🛑 کد پرسنلی باید عددی و بین ۴ تا ۶ رقم باشد.")]
    public string PersonalCode { get; set; }

    [Required(ErrorMessage = "🛑 شماره تماس الزامی است.")]
    [RegularExpression(@"^\d{8,11}$", ErrorMessage = "🛑 شماره تماس باید عددی و بین ۸ تا ۱۱ رقم باشد.")]
    public string PhoneNumber { get; set; }

}