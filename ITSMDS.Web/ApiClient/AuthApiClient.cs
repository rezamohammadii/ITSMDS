// Services/AuthService.cs
using ITSMDS.Domain.DTOs;
using ITSMDS.Web.Components.Pages.Auth;
using ITSMDS.Web.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Security.Claims;

public class AuthApiClient
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;
    private readonly IMemoryCache _memoryCache;
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private readonly CustomAuthStateProvider _authStateProvider;

    public AuthApiClient(HttpClient httpClient,
                       NavigationManager navigationManager,
                       IMemoryCache memoryCache,
                       ProtectedLocalStorage protectedLocalStorage,
                       AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
        _memoryCache = memoryCache;
        _protectedLocalStorage = protectedLocalStorage;
        _authStateProvider = (CustomAuthStateProvider)authStateProvider;
    }

    public async Task<(bool Success, string Message)> LoginAsync(LoginDTO loginModel)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseClient<LoginResponseDTO>>();

        if (apiResponse == null)
        {
            return (false, "پاسخی از سرور دریافت نشد.");
        }

        if (!apiResponse.Success)
        {
            // اینجا پیام خطا برمی‌گرده (مثل: نام کاربری یا رمز عبور نادرست است)
            return (false, apiResponse.Message);
        }

        var authResponse = apiResponse.Data;
        if (authResponse == null || string.IsNullOrEmpty(authResponse.Token))
        {
            return (false, "توکن معتبر دریافت نشد.");
        }

        await _protectedLocalStorage.SetAsync("authToken", authResponse.Token);

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", authResponse.Token);

        await CacheUserPermissions(authResponse);

        // اطلاع به Blazor
        _authStateProvider.NotifyUserAuthentication(authResponse.Token);

        return (true, apiResponse.Message);
    }

    public async Task LogoutAsync()
    {
        await _protectedLocalStorage.DeleteAsync("authToken");
        _httpClient.DefaultRequestHeaders.Authorization = null;

        _authStateProvider.NotifyUserLogout();

        _navigationManager.NavigateTo("/login", true);
    }

    private async Task CacheUserPermissions(LoginResponseDTO authResponse)
    {
        var cacheKey = $"user_permissions_{authResponse.UserId}";
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(30))
            .SetAbsoluteExpiration(TimeSpan.FromHours(1));

        _memoryCache.Set(cacheKey, authResponse.PermissionNames, cacheOptions);
    }

    public async Task<bool> HasPermission(string permission)
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity?.IsAuthenticated != true)
            return false;

        return user.Claims.Any(c => c.Type == "Permissions" && c.Value.Contains(permission));
    }

}
