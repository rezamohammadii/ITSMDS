using ITSMDS.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ITSMDS.Web.ViewModel;

public class ServerViewModel
{
    public string ServerName { get; private set; }
    public int RAM { get; private set; }
    public string CPU { get; private set; }
    public string MainBoardModel { get; private set; }
    public int StorageSize { get; private set; }
    public int StorageType { get; private set; }
    public string OS { get; private set; }
    public string IpAddress { get; private set; }
    public string Location { get; private set; }
    public bool IsEnable { get; private set; }
    public long? DepartmentId { get; private set; }
}


public class ServerViewModelIn
{
    [Display(Name = "نام سرور")]
    [Required(ErrorMessage = "نام سرور الزامی است")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "نام سرور باید بین 2 تا 100 کاراکتر باشد")]
    [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s\-_\.0-9]+$",
       ErrorMessage = "نام سرور فقط می‌تواند شامل حروف، اعداد، فاصله و کاراکترهای - _ . باشد")]
    public string ServerName { get; set; }

    [Display(Name = "حافظه RAM")]
    [Required(ErrorMessage = "مقدار RAM الزامی است")]
    [Range(1, 2048, ErrorMessage = "مقدار RAM باید بین 1 تا 2048 GB باشد")]
    public int RAM { get; set; }

    [Display(Name = "پردازنده")]
    [Required(ErrorMessage = "مدل پردازنده الزامی است")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "مدل پردازنده باید بین 2 تا 150 کاراکتر باشد")]
    public string CPU { get; set; }

    [Display(Name = "مدل مادربرد")]
    [StringLength(100, ErrorMessage = "مدل مادربرد نمی‌تواند بیش از 100 کاراکتر باشد")]
    public string MainBoardModel { get; set; }

    [Display(Name = "حجم ذخیره‌سازی")]
    [Required(ErrorMessage = "حجم ذخیره‌سازی الزامی است")]
    [Range(1, 100000, ErrorMessage = "حجم ذخیره‌سازی باید بین 1 تا 100000 GB باشد")]
    public int StorageSize { get; set; }

    [Display(Name = "نوع ذخیره‌سازی")]
    [Required(ErrorMessage = "نوع ذخیره‌سازی الزامی است")]
    [Range(0, 3, ErrorMessage = "نوع ذخیره‌سازی نامعتبر است")]
    public int StorageType { get; set; }

    [Display(Name = "سیستم عامل")]
    [Required(ErrorMessage = "سیستم عامل الزامی است")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "نام سیستم عامل باید بین 2 تا 50 کاراکتر باشد")]
    public string OS { get; set; }

    [Display(Name = "آدرس IP")]
    [Required(ErrorMessage = "آدرس IP الزامی است")]
    [RegularExpression(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$",
        ErrorMessage = "آدرس IP معتبر نیست")]
    public string IpAddress { get; set; }

    [Display(Name = "موقعیت")]
    [Required(ErrorMessage = "موقعیت سرور الزامی است")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "موقعیت باید بین 2 تا 200 کاراکتر باشد")]
    public string Location { get; set; }

    [Display(Name = "توضیحات")]
    public string? Description { get; set; }

    [Display(Name = "وضعیت فعال بودن")]
    public bool IsEnable { get; set; } = true;

    [Display(Name = "مسئول سرور")]
    [Required(ErrorMessage = "نام مسئول الزامی است")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "نام مسئول  باید بین 2 تا 50 کاراکتر باشد")]
    public string? ServerManager { get; set; }
}
