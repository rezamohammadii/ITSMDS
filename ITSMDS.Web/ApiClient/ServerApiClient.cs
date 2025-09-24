
using ITSMDS.Domain.DTOs;
using ITSMDS.Web.ViewModel;
using System.Text;
using System.Text.Json;


namespace ITSMDS.Web.ApiClient;
public class ServerApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ServerApiClient> _logger;

    public ServerApiClient(HttpClient httpClient, ILogger<ServerApiClient> logger)
    {
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
        _logger = logger;
    }

    public async Task<(bool Success, string Message, List<ServerViewModel> Data)> GetServerListAsync(CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/server/list", ct);
            var content = await response.Content.ReadAsStringAsync(ct);

            if (string.IsNullOrEmpty(content))
                return (false, "پاسخی از سرور دریافت نشد.", []);

            var apiResponse = JsonSerializer.Deserialize<ApiResponseClient<List<ServerViewModel>>>(content, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null)
                return (false, "خطا در پردازش پاسخ سرور.", []);

            return (apiResponse.Success, apiResponse.Message, apiResponse.Data ?? []);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetServerListAsync");
            return (false, "مشکلی در ارتباط با سرور پیش آمده.", []);
        }
    }

    public async Task<(bool Success, string Message, ServerViewModel? Data)> GetServerAsync(long id, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/server/{id}", ct);
            var content = await response.Content.ReadAsStringAsync(ct);

            if (string.IsNullOrEmpty(content))
                return (false, "پاسخی از سرور دریافت نشد.", null);

            var apiResponse = JsonSerializer.Deserialize<ApiResponseClient<ServerViewModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null)
                return (false, "خطا در پردازش پاسخ سرور.", null);

            return (apiResponse.Success, apiResponse.Message, apiResponse.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetServerAsync for server {Id}", id);
            return (false, "مشکلی در ارتباط با سرور پیش آمده.", null);
        }
    }

    public async Task<(bool Success, string Message, ServerViewModel? Data)> CreateServerAsync(ServerViewModelIn request, CancellationToken ct = default)
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/server/create", content, ct);
            var contentResponse = await response.Content.ReadAsStringAsync(ct);

            if (string.IsNullOrEmpty(contentResponse))
                return (false, "پاسخی از سرور دریافت نشد.", null);

            var apiResponse = JsonSerializer.Deserialize<ApiResponseClient<ServerViewModel>>(contentResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null)
                return (false, "خطا در پردازش پاسخ سرور.", null);

            return (apiResponse.Success, apiResponse.Message, apiResponse.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateServerAsync");
            return (false, "مشکلی در ارتباط با سرور پیش آمده.", null);
        }
    }

    public async Task<(bool Success, string Message, ServerViewModel? Data)> EditServerAsync(ServerViewModelIn request, CancellationToken ct = default)
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("/api/server/edit", content, ct);
            var contentResponse = await response.Content.ReadAsStringAsync(ct);

            if (string.IsNullOrEmpty(contentResponse))
                return (false, "پاسخی از سرور دریافت نشد.", null);

            var apiResponse = JsonSerializer.Deserialize<ApiResponseClient<ServerViewModel>>(contentResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null)
                return (false, "خطا در پردازش پاسخ سرور.", null);

            return (apiResponse.Success, apiResponse.Message, apiResponse.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in EditServerAsync for server {ServerName}", request.ServerName);
            return (false, "مشکلی در ارتباط با سرور پیش آمده.", null);
        }
    }

    public async Task<(bool Success, string Message)> DeleteServerAsync(long id, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/server/delete/{id}", ct);
            var contentResponse = await response.Content.ReadAsStringAsync(ct);

            if (string.IsNullOrEmpty(contentResponse))
                return (false, "پاسخی از سرور دریافت نشد.");

            var apiResponse = JsonSerializer.Deserialize<ApiResponseClient<object>>(contentResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null)
                return (false, "خطا در پردازش پاسخ سرور.");

            return (apiResponse.Success, apiResponse.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteServerAsync for server {Id}", id);
            return (false, "مشکلی در ارتباط با سرور پیش آمده.");
        }
    }
}
