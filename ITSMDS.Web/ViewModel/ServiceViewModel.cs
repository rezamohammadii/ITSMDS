using ITSMDS.Domain.Enums;
using ITSMDS.Pages.Services;
using System.ComponentModel.DataAnnotations;

namespace ITSMDS.Web.ViewModel
{
    public class ServiceViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "نام سرویس الزامی است")]
        [MaxLength(100, ErrorMessage = "نام سرویس نمی‌تواند بیشتر از 100 کاراکتر باشد")]
        public string ServiceName { get; set; }

        [MaxLength(50, ErrorMessage = "نسخه نمی‌تواند بیشتر از 50 کاراکتر باشد")]
        public string? Version { get; set; }

        [MaxLength(500, ErrorMessage = "توضیحات نمی‌تواند بیشتر از 500 کاراکتر باشد")]
        public string? Description { get; set; }

        [Display(Name = "مسیر مستندات")]
        public string? DocumentFilePath { get; set; }

        [Display(Name = "مسیر اجرا")]
        public string? ExcutionPath { get; set; }

        [Required(ErrorMessage = "امتیاز بحرانی بودن الزامی است")]
        public ServiceEnum.CriticalityScore CriticalityScore { get; set; }

        [Range(1, 65535, ErrorMessage = "پورت باید بین 1 تا 65535 باشد")]
        public int Port { get; set; }

        [Required(ErrorMessage = "شناسه سرور الزامی است")]
        public string ServerName { get; set; }

        [Display(Name = "زمان ایجاد")]
        public string? CreateTime { get; set; }

        public bool IsActive { get; set; }
    }
    public class ServiceWidgetCritical
    {
        public int VeryHighServiceCount { get; set; }
        public int HighServiceCount { get; set; }
        public int NormalServiceCount { get; set; }
        public int LowServiceCount { get; set; }
    }
}
