using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using ITSMDS.Domain.DTOs;
using ITSMDS.Web.ViewModel;

namespace ITSMDS.Web.ApiClient;



public class UserApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserApiClient> _logger;

    public UserApiClient(HttpClient httpClient, ILogger<UserApiClient> logger)
    {
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
        _logger = logger;
    }

    public async Task<(bool Success, string Message, UserModel[] Data)> GetUserListAsync(CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/user/GetAll", ct);
            var content = await response.Content.ReadAsStringAsync(ct);

            if (string.IsNullOrEmpty(content))
                return (false, "پاسخی از سرور دریافت نشد.", Array.Empty<UserModel>());

            var apiResponse = JsonSerializer.Deserialize<ApiResponseClient<UserModel[]>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null)
                return (false, "خطا در پردازش پاسخ سرور.", Array.Empty<UserModel>());

            return (apiResponse.Success, apiResponse.Message, apiResponse.Data ?? Array.Empty<UserModel>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetUserListAsync");
            return (false, "مشکلی در ارتباط با سرور پیش آمده.", Array.Empty<UserModel>());
        }
    }

    public async Task<(bool Success, string Message, UserModel? Data)> GetUserAsync(int personalCode, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/user/{personalCode}", ct);
            var content = await response.Content.ReadAsStringAsync(ct);

            if (string.IsNullOrEmpty(content))
                return (false, "پاسخی از سرور دریافت نشد.", null);

            var apiResponse = JsonSerializer.Deserialize<ApiResponseClient<UserModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null)
                return (false, "خطا در پردازش پاسخ سرور.", null);

            return (apiResponse.Success, apiResponse.Message, apiResponse.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetUserAsync for code {Code}", personalCode);
            return (false, "مشکلی در ارتباط با سرور پیش آمده.", null);
        }
    }


    public async Task<(bool Success, string Message, UserModel? Data)> EditUserAsync(UserModel userModel, CancellationToken ct = default)
    {
        try
        {
            var editUserRequest = new EditUserModel(
                userModel.Email,
                userModel.FirstName,
                userModel.LastName,
                userModel.PersonalCode.ToString(),
                userModel.PhoneNumber,
                userModel.UserName,
                userModel.IpAddress
            );

            var jsonString = JsonSerializer.Serialize(editUserRequest);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("/api/user/edit", content, ct);
            var contentResponse = await response.Content.ReadAsStringAsync(ct);

            if (string.IsNullOrEmpty(contentResponse))
                return (false, "پاسخی از سرور دریافت نشد.", null);

            var apiResponse = JsonSerializer.Deserialize<ApiResponseClient<UserModel>>(contentResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null)
                return (false, "خطا در پردازش پاسخ سرور.", null);

            return (apiResponse.Success, apiResponse.Message, apiResponse.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in EditUserAsync for user {Code}", userModel.PersonalCode);
            return (false, "مشکلی در ارتباط با سرور پیش آمده.", null);
        }
    }


    public async Task<(bool Success, string Message)> DeleteUserAsync(int personalCode, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/user/delete/{personalCode}", ct);
            var contentResponse = await response.Content.ReadAsStringAsync(ct);

            if (string.IsNullOrEmpty(contentResponse))
                return (false, "پاسخی از سرور دریافت نشد.");

            var apiResponse = JsonSerializer.Deserialize<ApiResponseClient<bool>>(contentResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null)
                return (false, "خطا در پردازش پاسخ سرور.");

            return (apiResponse.Success, apiResponse.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteUserAsync for user {Code}", personalCode);
            return (false, "مشکلی در ارتباط با سرور پیش آمده.");
        }
    }


    public async Task<(bool Success, string Message)> CreateUserAsync(UserModelIn modelIn, CancellationToken ct = default)
    {
        try
        {
            var serializedModel = JsonSerializer.Serialize(modelIn);
            var content = new StringContent(serializedModel, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/user/create", content, ct);
            var contentResponse = await response.Content.ReadAsStringAsync(ct);

            _logger.LogInformation("API Response: {Response}", contentResponse);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = JsonSerializer.Deserialize<ApiResponseClient<UserResponse>>(contentResponse, options);

            if (result is not null)
            {
                if (result.Success)
                {
                    _logger.LogInformation("User created: {Username}, Result: {Result}", modelIn.UserName, result.Data);
                    return (true, result.Message); 
                }
                else
                {
                    _logger.LogWarning("User creation failed: {Username}, Message: {Message}", modelIn.UserName, result.Message);
                    return (false, result.Message); 
                }
            }

            return (false, "پاسخی از سرور دریافت نشد.");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization error");
            return (false, "خطا در پردازش پاسخ سرور.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateUserAsync for user {Username}", modelIn.UserName);
            return (false, "مشکلی در ارتباط با سرور پیش آمده، لطفاً بعداً تلاش کنید.");
        }
    }


    public async Task<(bool Success, string Message, List<PermissionDto> Data)> GetPermissionListAsync(CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/user/permissions", ct);
            var content = await response.Content.ReadAsStringAsync(ct);

            if (string.IsNullOrEmpty(content))
                return (false, "پاسخی از سرور دریافت نشد.", new List<PermissionDto>());

            var apiResponse = JsonSerializer.Deserialize<ApiResponseClient<List<PermissionDto>>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null)
                return (false, "خطا در پردازش پاسخ سرور.", new List<PermissionDto>());

            return (apiResponse.Success, apiResponse.Message, apiResponse.Data ?? new List<PermissionDto>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetPermissionListAsync");
            return (false, "مشکلی در ارتباط با سرور پیش آمده.", new List<PermissionDto>());
        }
    }

}

