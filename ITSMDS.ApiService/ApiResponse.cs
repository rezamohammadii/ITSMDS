namespace ITSMDS.ApiService;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public static ApiResponse<T> Ok(T data, string message = "Operation successful")
    {
        return new ApiResponse<T> { Success = true, Message = message, Data = data };
    }
}
