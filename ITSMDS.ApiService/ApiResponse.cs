using ITSMDS.Domain.Enums;
using ITSMDS.Infrastructure.Extensions;

namespace ITSMDS.ApiService;

public class ApiResponse<T>
{
    public bool Success { get; set; }     
    public int Code { get; set; }         
    public string Message { get; set; } 
    public T Data { get; set; }           

    public static ApiResponse<T> Ok(T data, string message = "عملیات با موفقیت انجام شد.")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Code = 0,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> Fail(ErrorCode errorCode = default , string message = "مشکلی پیش آمده بعدا تلاش کنید")
    {
        return new ApiResponse<T>
        {
            Success = false,
            Code = (int)errorCode,
            Message = string.IsNullOrEmpty(message) ? errorCode.GetMessage() : message,
            Data = default
        };
    }
}
