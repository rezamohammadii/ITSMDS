// Services/AuthService.cs
using ITSMDS.Domain.DTOs;
using ITSMDS.Web.Components.Pages.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Security.Claims;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly NavigationManager _navigationManager;
    private readonly IMemoryCache _memoryCache;
    private readonly IJSRuntime _jsRuntime;
    private readonly AuthenticationStateProvider _authStateProvider;
    public AuthService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor,
                     NavigationManager navigationManager, IMemoryCache memoryCache ,
                     IJSRuntime jSRuntime, AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _navigationManager = navigationManager;
        _memoryCache = memoryCache;
        _jsRuntime = jSRuntime;
        _authStateProvider = authStateProvider;
    }

    public async Task<bool> LoginAsync(LoginDTO loginModel)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);

            if (response.IsSuccessStatusCode)
            {

                var context = _httpContextAccessor.HttpContext;
                if (context != null)
                {
                    var authResponse = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();

                    if (authResponse != null && !string.IsNullOrEmpty(authResponse.Token))
                    {
                        // ذخیره توکن در localStorage یا cookie
                        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", authResponse.Token);

                        // تنظیم هدر برای درخواست‌های بعدی
                        _httpClient.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", authResponse.Token);

                        // ذخیره اطلاعات کاربر در state
                        await CacheUserPermissions(authResponse);

                        // اطلاع‌رسانی به سیستم authentication
                        await _authStateProvider.GetAuthenticationStateAsync();

                        return true;
                    }
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login error: {ex.Message}");
            return false;
        }
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
        // دریافت وضعیت احراز هویت از AuthenticationStateProvider
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity?.IsAuthenticated != true)
            return false;

        // بررسی از طریق claims کاربر
        var hasPermission = user.Claims
            .Any(c => c.Type == "Permission" && c.Value == permission);

        if (hasPermission) return true;

        // اگر در claims نبود، از کش بررسی می‌کنیم
        var userId = GetCurrentUserId(user);
        if (userId.HasValue)
        {
            var cacheKey = $"user_permissions_{userId.Value}";
            if (_memoryCache.TryGetValue(cacheKey, out List<string> cachedPermissions))
            {
                return cachedPermissions?.Contains(permission) ?? false;
            }
        }

        return false;
    }

    private int? GetCurrentUserId(ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
        {
            return userId;
        }
        return null;
    }

    //public async Task LogoutAsync()
    //{
    //    var authState = await _authStateProvider.GetAuthenticationStateAsync();
    //    var user = authState.User;

    //    if (user != null)
    //    {
    //        await user.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    //    }

    //    // پاک کردن کش
    //    var userId = GetCurrentUserId();
    //    if (userId.HasValue)
    //    {
    //        ClearCache(userId.Value);
    //    }

    //    _navigationManager.NavigateTo("/", true);
    //}

    public void ClearCache(int userId)
    {
        var cacheKey = $"user_permissions_{userId}";
        _memoryCache.Remove(cacheKey);
    }
}