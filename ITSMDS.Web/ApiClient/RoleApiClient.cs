using ITSMDS.Domain.DTOs;
using ITSMDS.Web.ViewModel;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace ITSMDS.Web.ApiClient;



public class RoleApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<RoleApiClient> _logger;

    public RoleApiClient(HttpClient _httpClient, ILogger<RoleApiClient> _logger)
    {
        this._httpClient = _httpClient;
        this._logger = _logger;
    }

    public async Task<(bool Success, string Message, PageResultDto<RoleDto> Data)> RoleListAsync(int pageNumber = 1, int pageSize = 10, string searchTerm = "", CancellationToken cancellationToken = default)
    {
        try
        {
            var url = $"/api/role/list?pageNumber={pageNumber}&pageSize={pageSize}&searchTerm={searchTerm}";
            var response = await _httpClient.GetAsync(url, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            if (string.IsNullOrEmpty(content))
                return (false, "پاسخی از سرور دریافت نشد.", new PageResultDto<RoleDto>());

            var apiResponse = JsonSerializer.Deserialize<ApiResponseClient<PageResultDto<RoleDto>>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null)
                return (false, "خطا در پردازش پاسخ سرور.", new PageResultDto<RoleDto>());

            return (apiResponse.Success, apiResponse.Message, apiResponse.Data ?? new PageResultDto<RoleDto>());
        }
        catch (Exception ex)
        {
            return (false, "مشکلی در ارتباط با سرور پیش آمده.", new PageResultDto<RoleDto>());
        }
    }


    public async Task<(bool Success, string Message)> CreateRoleAsync(RoleDtoIn roleIn, CancellationToken ct = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(roleIn);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/role/create", content, ct);
            var responseContent = await response.Content.ReadAsStringAsync(ct);

            if (string.IsNullOrEmpty(responseContent))
                return (false, "پاسخی از سرور دریافت نشد.");

            var apiResponse = JsonSerializer.Deserialize<ApiResponseClient<bool?>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null)
                return (false, "خطا در پردازش پاسخ سرور.");

            return (apiResponse.Success, apiResponse.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateRoleAsync");
            return (false, "مشکلی در ارتباط با سرور پیش آمده.");
        }
    }


    public async Task<(bool Success, string Message)> AssignRoleAsync(string personalCode, int roleId, CancellationToken ct = default)
    {
        try
        {
            var url = $"/api/role/assignRole?personalCode={personalCode}&roleId={roleId}";
            var response = await _httpClient.PutAsync(url, null, ct);
            var responseContent = await response.Content.ReadAsStringAsync(ct);

            if (string.IsNullOrEmpty(responseContent))
                return (false, "پاسخی از سرور دریافت نشد.");

            var apiResponse = JsonSerializer.Deserialize<ApiResponseClient<bool?>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null)
                return (false, "خطا در پردازش پاسخ سرور.");

            return (apiResponse.Success, apiResponse.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in AssignRoleAsync");
            return (false, "مشکلی در ارتباط با سرور پیش آمده.");
        }
    }

    public async Task<(bool Success, string Message)> DeleteRoleAsync(int roleId, CancellationToken ct = default)
    {
        try
        {
            var url = $"/api/role?roleId={roleId}";
            var response = await _httpClient.DeleteAsync(url, ct);
            var responseContent = await response.Content.ReadAsStringAsync(ct);

            if (string.IsNullOrEmpty(responseContent))
                return (false, "پاسخی از سرور دریافت نشد.");

            var apiResponse = JsonSerializer.Deserialize<ApiResponseClient<bool?>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null)
                return (false, "خطا در پردازش پاسخ سرور.");

            return (apiResponse.Success, apiResponse.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteRoleAsync");
            return (false, "مشکلی در ارتباط با سرور پیش آمده.");
        }
    }
}
