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

    public async Task<UserModel[]> GetUserListAsync(CancellationToken ct = default)
    {
        try
        {
            
            
            List<UserModel>? userList = null;

            await foreach (var user in _httpClient.GetFromJsonAsAsyncEnumerable<UserModel>("/api/user/GetAll", ct))
            {
                if (user is not null)
                {
                    userList ??= [];
                    userList.Add(user);
                }
            }

            _logger.LogInformation("Fetched {Count} users", userList?.Count ?? 0);
            return userList?.ToArray() ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetUserListAsync");
            return [];
        }
    }

    public async Task<UserModel?> GetUserAsync(int personalCode, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/user/{personalCode}", ct);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(ct);
                if (string.IsNullOrEmpty(content))
                {
                    _logger.LogWarning("Empty response for user {Code}", personalCode);
                    return null;
                }

                var user = JsonSerializer.Deserialize<UserModel>(content);
                _logger.LogInformation("Fetched user {Code}", personalCode);
                return user ?? new UserModel { };
            }

            _logger.LogWarning("Failed to fetch user {Code}, Status: {Status}", personalCode, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetUserAsync for code {Code}", personalCode);
            return null;
        }
    }

    public async Task<UserModel?> EditUserAsync(UserModel userModel, CancellationToken ct = default)
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

            if (response.IsSuccessStatusCode)
            {
                var contentResponse = await response.Content.ReadAsStringAsync(ct);
                if (string.IsNullOrEmpty(contentResponse))
                {
                    _logger.LogWarning("Empty response on edit for user {Code}", userModel.PersonalCode);
                    return null;
                }

                var user = JsonSerializer.Deserialize<UserModel>(contentResponse);
                _logger.LogInformation("User edited successfully: {Code}", userModel.PersonalCode);
                return user ?? new UserModel { };
            }

            _logger.LogWarning("Failed to edit user {Code}, Status: {Status}", userModel.PersonalCode, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in EditUserAsync for user {Code}", userModel.PersonalCode);
            return null;
        }
    }

    public async Task<bool> DeleteUserAsync(int personalCode, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/user/delete/{personalCode}", ct);

            if (response.IsSuccessStatusCode)
            {
                var contentResponse = await response.Content.ReadAsStringAsync(ct);
                if (string.IsNullOrEmpty(contentResponse))
                {
                    _logger.LogWarning("Empty delete response for user {Code}", personalCode);
                    return false;
                }

                using var jsonDocument = JsonDocument.Parse(contentResponse);
                var jsonElement = jsonDocument.RootElement;
                bool res = jsonElement.GetProperty("response").GetBoolean();

                _logger.LogInformation("User deleted: {Code}, Result: {Result}", personalCode, res);
                return res;
            }

            _logger.LogWarning("Failed to delete user {Code}, Status: {Status}", personalCode, response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteUserAsync for user {Code}", personalCode);
            return false;
        }
    }

    public async Task<bool> CreateUserAsync(UserModelIn modelIn, CancellationToken ct = default)
    {
        try
        {
            var serializedModel = JsonSerializer.Serialize(modelIn);
            var content = new StringContent(serializedModel, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/user/create", content, ct);

            if (response.IsSuccessStatusCode)
            {
                var contentResponse = await response.Content.ReadAsStringAsync(ct);
                using var jsonDocument = JsonDocument.Parse(contentResponse);
                var jsonElement = jsonDocument.RootElement;
                bool result = jsonElement.GetProperty("response").GetBoolean();

                _logger.LogInformation("User created: {Username}, Result: {Result}", modelIn.UserName, result);
                return result;
            }

            _logger.LogWarning("Failed to create user {Username}, Status: {Status}", modelIn.UserName, response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateUserAsync for user {Username}", modelIn.UserName);
            return false;
        }
    }

    public async Task<List<PermissionDto>> GetPermissionListAsync(CancellationToken ct = default)
    {
        try
        {            
            var response = await _httpClient.GetFromJsonAsync<List<PermissionDto>>("/api/user/permissions", ct);            

            _logger.LogInformation("Fetched {Count} permission", response?.Count ?? 0);
            return response ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetPermissionListAsync");
            return [];
        }
    }
}

