using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ITSMDS.Web.ViewModel;

namespace ITSMDS.Web.ApiClient;

public class UserApiClient(HttpClient httpClient)
{
    
    public async Task<UserModel[]> GetUserListAsync(CancellationToken ct = default)
    {
        List<UserModel>? userList = null;
        httpClient.Timeout = TimeSpan.FromSeconds(30);
        await foreach (var user in httpClient.GetFromJsonAsAsyncEnumerable<UserModel>("/api/user/GetAll", ct))
        {
            if (user is not null)
            {
                userList ??= [];
                userList.Add(user);
            }
        }

        return userList?.ToArray() ?? [];

    }
    public async Task<UserModel>? GetUserAsync(int personalCode, CancellationToken ct = default)
    {
        httpClient.Timeout = TimeSpan.FromSeconds(30);

        var response = await httpClient.GetAsync($"/api/user/{personalCode}");

        if(response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
            var user = JsonSerializer.Deserialize<UserModel>(content);
            return user ?? new UserModel { };
        }
        return null;

    }
    public async Task<UserModel>? EditUserAsync(UserModel userModel, CancellationToken ct = default)
    {
        httpClient.Timeout = TimeSpan.FromSeconds(30);

        var jsonString = JsonSerializer.Serialize(userModel);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        var response = await httpClient.PutAsync($"/api/user/edit", content);

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var contentResonse = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(contentResonse))
            {
                return null;
            }
            var user = JsonSerializer.Deserialize<UserModel>(contentResonse);
            return user ?? new UserModel { };
        }
        return null;

    }
    public async Task<bool> DeleteUserAsync(string hashId, CancellationToken ct = default)
    {
        return true;

    }
}
