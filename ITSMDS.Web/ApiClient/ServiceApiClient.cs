using ITSMDS.Domain.DTOs;
using ITSMDS.Web.ViewModel;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public class ServiceApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ServiceApiClient> _logger;

    public ServiceApiClient(HttpClient httpClient, ILogger<ServiceApiClient> logger)
    {
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
        _logger = logger;
    }

    public async Task<(bool Success, string Message, List<ServiceViewModel?> Data)> GetServicesAsync(CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/service", ct);
            var content = await response.Content.ReadAsStringAsync(ct);

            if (string.IsNullOrEmpty(content))
                return (false, "پاسخی از سرور دریافت نشد.", []);

            var apiResponse = JsonSerializer.Deserialize<ApiResponseClient<ServiceViewModel[]>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null)
                return (false, "خطا در پردازش پاسخ سرور.", []);

            return (apiResponse.Success, apiResponse.Message, []);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetServicesAsync");
            return (false, "مشکلی در ارتباط با سرور پیش آمده.", []);
        }
    }

    public async Task<(bool Success, string Message, ServiceViewModel? Data)> GetServiceAsync(long id, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/service/{id}", ct);
            var content = await response.Content.ReadAsStringAsync(ct);

            if (string.IsNullOrEmpty(content))
                return (false, "پاسخی از سرور دریافت نشد.", null);

            var apiResponse = JsonSerializer.Deserialize<ApiResponseClient<ServiceViewModel>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null)
                return (false, "خطا در پردازش پاسخ سرور.", null);

            return (apiResponse.Success, apiResponse.Message, apiResponse.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetServiceAsync for id {Id}", id);
            return (false, "مشکلی در ارتباط با سرور پیش آمده.", null);
        }
    }

    public async Task<(bool Success, string Message, ServiceViewModel? Data)> CreateServiceAsync(ServiceViewModel model, CancellationToken ct = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/service", content, ct);
            var contentResponse = await response.Content.ReadAsStringAsync(ct);

            if (string.IsNullOrEmpty(contentResponse))
                return (false, "پاسخی از سرور دریافت نشد.", null);

            var apiResponse = JsonSerializer.Deserialize<ApiResponseClient<ServiceViewModel>>(contentResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null)
                return (false, "خطا در پردازش پاسخ سرور.", null);

            return (apiResponse.Success, apiResponse.Message, apiResponse.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateServiceAsync");
            return (false, "مشکلی در ارتباط با سرور پیش آمده.", null);
        }
    }

    public async Task<(bool Success, string Message, ServiceViewModel? Data)> UpdateServiceAsync(ServiceViewModel model, CancellationToken ct = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/service/{model.ServerId}", content, ct);
            var contentResponse = await response.Content.ReadAsStringAsync(ct);

            if (string.IsNullOrEmpty(contentResponse))
                return (false, "پاسخی از سرور دریافت نشد.", null);

            var apiResponse = JsonSerializer.Deserialize<ApiResponseClient<ServiceViewModel>>(contentResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null)
                return (false, "خطا در پردازش پاسخ سرور.", null);

            return (apiResponse.Success, apiResponse.Message, apiResponse.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateServiceAsync for id {Id}", model.ServerId);
            return (false, "مشکلی در ارتباط با سرور پیش آمده.", null);
        }
    }

    public async Task<(bool Success, string Message)> DeleteServiceAsync(long id, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/service/{id}", ct);
            var content = await response.Content.ReadAsStringAsync(ct);

            if (string.IsNullOrEmpty(content))
                return (false, "پاسخی از سرور دریافت نشد.");

            var apiResponse = JsonSerializer.Deserialize<ApiResponseClient<object>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null)
                return (false, "خطا در پردازش پاسخ سرور.");

            return (apiResponse.Success, apiResponse.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteServiceAsync for id {Id}", id);
            return (false, "مشکلی در ارتباط با سرور پیش آمده.");
        }
    }
    public async Task<(bool Success, string Message, ServiceWidget? Data)> GetServiceWidgetAsync(CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/service/widget", ct);
            var content = await response.Content.ReadAsStringAsync(ct);

            if (string.IsNullOrEmpty(content))
                return (false, "پاسخی از سرور دریافت نشد.", null);

            var apiResponse = JsonSerializer.Deserialize<ApiResponseClient<ServiceWidget>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null)
                return (false, "خطا در پردازش پاسخ سرور.", null);

            return (apiResponse.Success, apiResponse.Message, apiResponse.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetServerWidgetAsync ");
            return (false, "مشکلی در ارتباط با سرور پیش آمده.", null);
        }
    }

}
