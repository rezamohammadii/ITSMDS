using System.ComponentModel.DataAnnotations;

namespace ITSMDS.Web.ViewModel
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "لطفا نام کاربری را وارد کنید")]
        [Display(Name = "نام کاربری")]
        [MinLength(3, ErrorMessage = "نام کاربری باید حداقل 3 کاراکتر باشد")]
        public string Username { get; set; }


        [Required(ErrorMessage = "لطفا رمز عبور را وارد کنید")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "رمز عبور باید حداقل 6 کاراکتر باشد")]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا تکرار رمز عبور را وارد کنید")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن مطابقت ندارند")]
        [Display(Name = "تکرار رمز عبور")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "لطفا موافقت با قوانین را تأیید کنید")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "لطفا با قوانین موافقت کنید")]
        [Display(Name = "موافقت با قوانین")]
        public bool AgreeToTerms { get; set; }
    }
}
