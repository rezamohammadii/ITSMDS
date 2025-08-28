using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
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
        httpClient.Dispose();
        return null;

    }
    public async Task<UserModel>? EditUserAsync(UserModel userModel, CancellationToken ct = default)
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
    public async Task<bool> DeleteUserAsync(int personalCode, CancellationToken ct = default)
    {
        var response = await httpClient.DeleteAsync($"/api/user/delete/{personalCode}", ct);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var contentResonse = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(contentResonse))
            {
                return false;
            }
            using JsonDocument jsonDocument = JsonDocument.Parse(contentResonse);
            JsonElement jsonElement = jsonDocument.RootElement;
            bool res = jsonElement.GetProperty("response").GetBoolean();
            return res;
            
        }
        return false;
    }
}
