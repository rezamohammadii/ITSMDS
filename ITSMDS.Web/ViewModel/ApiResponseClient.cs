using System.Text.Json.Serialization;

namespace ITSMDS.Web.ViewModel;

public class ApiResponseClient<T>
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("data")]
    public T Data { get; set; }

    [JsonPropertyName("errorCode")]
    public string ErrorCode { get; set; }

    [JsonPropertyName("additionalData")]
    public object AdditionalData { get; set; }

    public static ApiResponseClient<T> Ok(T data, string message = "Operation successful")
    {
        return new ApiResponseClient<T> { Success = true, Message = message, Data = data };
    }
}
