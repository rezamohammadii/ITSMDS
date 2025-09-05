using System.Text;
using System.Text.Json;
using ITSMDS.Domain.DTOs;

namespace ITSMDS.Web.ApiClient;

public class RoleApiClient(HttpClient httpClient)
{
    public async Task<PageResultDto<RoleDto>>? RoleListAsync(int pageNumber = 1, int pageSize = 10, string searchTerm = "", CancellationToken cancellationToken = default)
    {
        try
        {
            var url = $"/api/role/list?pageNumber={pageNumber}&pageSize={pageSize}&searchTerm={searchTerm}";
            var result = await httpClient.GetFromJsonAsync<PageResultDto<RoleDto>>(url, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching paged roles: {ex.Message}");
            return new PageResultDto<RoleDto>();
        }
    }

    public async Task<bool> CreateRoleAsync(RoleDtoIn roleIn, CancellationToken ct = default)
    {
        try
        {
            string serializeModel = JsonSerializer.Serialize(roleIn);
            var content = new StringContent(serializeModel, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/api/role/create", content, ct);
            var res = await response.Content.ReadAsStringAsync();
            
            return bool.Parse(res);
        }
        catch (Exception)
        {
            return false;

        }
    }

}
