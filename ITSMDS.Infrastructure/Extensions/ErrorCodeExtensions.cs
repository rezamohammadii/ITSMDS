

using ITSMDS.Domain.Enums;

namespace ITSMDS.Infrastructure.Extensions;

public static class ErrorCodeExtensions
{
    public static string GetMessage(this ErrorCode errorCode)
    {
        return errorCode switch
        {
            // General
            ErrorCode.UnknownError => "خطای غیرمنتظره‌ای رخ داده است، لطفاً بعداً تلاش کنید.",
            ErrorCode.DatabaseError => "مشکلی در ارتباط با سرور پیش آمده، لطفاً بعداً تلاش کنید.",
            ErrorCode.NetworkError => "ارتباط با سرور برقرار نشد، اتصال اینترنت خود را بررسی کنید.",
            ErrorCode.ServiceUnavailable => "سرویس در دسترس نیست، لطفاً بعداً تلاش کنید.",
            ErrorCode.OperationTimeout => "زمان اجرای عملیات به پایان رسید.",
            ErrorCode.RoleCreateSuccessfully => "نقش با موفقیت ایجاد شد.",
            ErrorCode.RoleAsignToUserSuccessfully => "نقش با موفقیت به کاربر اختصاص داده شد.",

            //  Authorization and access
            ErrorCode.InvalidCredentials => "نام کاربری یا رمز عبور نادرست است.",
            ErrorCode.Unauthorized => "شما مجاز به انجام این عملیات نیستید.",
            ErrorCode.Forbidden => "دسترسی شما به این بخش محدود شده است.",
            ErrorCode.SessionExpired => "نشست شما منقضی شده است، دوباره وارد شوید.",
            ErrorCode.AccountLocked => "حساب کاربری شما قفل شده است.",
            ErrorCode.TooManyRequests => "تعداد درخواست‌های شما بیش از حد مجاز است، لطفاً کمی صبر کنید.",
            ErrorCode.LoginSuccessfully => "ورود موفقیت‌آمیز بود.",

            // Validation data
            ErrorCode.ValidationError => "اطلاعات وارد شده معتبر نیست.",
            ErrorCode.InvalidFormat => "فرمت اطلاعات وارد شده نادرست است.",
            ErrorCode.RequiredFieldMissing => "برخی فیلدهای الزامی تکمیل نشده‌اند.",
            ErrorCode.OutOfRange => "مقدار وارد شده خارج از محدوده مجاز است.",
            ErrorCode.DuplicateData => "اطلاعات وارد شده تکراری است.",

            // Source
            ErrorCode.NotFound => "موردی با این مشخصات پیدا نشد.",
            ErrorCode.AlreadyExists => "این مورد از قبل وجود دارد.",
            ErrorCode.ResourceConflict => "تعارضی در داده‌ها وجود دارد.",
            ErrorCode.ResourceLimitExceeded => "به سقف مجاز استفاده از منابع رسیده‌اید.",
            ErrorCode.ServerError => "مشکلی در سرور به وجود آمده لطفا کمی بعد تلاش کنید",

            // File and upload
            ErrorCode.FileTooLarge => "حجم فایل بیش از حد مجاز است.",
            ErrorCode.UnsupportedFileType => "نوع فایل پشتیبانی نمی‌شود.",
            ErrorCode.FileUploadFailed => "بارگذاری فایل با خطا مواجه شد.",

            // Payment and finance
            ErrorCode.PaymentFailed => "پرداخت انجام نشد.",
            ErrorCode.PaymentDeclined => "پرداخت توسط بانک رد شد.",
            ErrorCode.InsufficientBalance => "موجودی کافی نیست.",

            // Security
            ErrorCode.SuspiciousActivity => "فعالیت مشکوک شناسایی شد.",
            ErrorCode.IpBlocked => "دسترسی از این آی‌پی مسدود شده است.",
            ErrorCode.AccessRestricted => "دسترسی شما به دلیل محدودیت‌های امنیتی امکان‌پذیر نیست.",
            ErrorCode.IpNotAllowed => "آذرس IP امکان ورود ندارد",
            ErrorCode.IpNotRange => "آدرس IP مجاز نمی باشد",
            ErrorCode.InvalidIpAddress => "آدرس IP نامعتبر می باشد",

            // System
            ErrorCode.ConfigError => "خطا در تنظیمات سیستم رخ داده است.",
            ErrorCode.DependencyFailure => "یکی از سرویس‌های وابسته در دسترس نیست.",
            ErrorCode.MaintenanceMode => "سیستم در حال به‌روزرسانی یا نگهداری است.",

            _ => "خطای ناشناخته رخ داده است."
        };
    }
}
