// CustomAuthStateProvider.cs
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime;
    private readonly HttpClient _httpClient;
    private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthStateProvider(IJSRuntime jsRuntime, HttpClient httpClient)
    {
        _jsRuntime = jsRuntime;
        _httpClient = httpClient;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // همیشه کاربر ناشناس رو برمی‌گردونیم تا موقع لود صفحه false نشه
        return Task.FromResult(new AuthenticationState(_currentUser));
    }

    public async Task InitializeAsync()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            _currentUser = new ClaimsPrincipal(identity);

            // اطلاع‌رسانی به Blazor برای رندر مجدد
            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(_currentUser))
            );
        }
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string token)
    {
        // همون کدی که خودت نوشتی برای decode کردن jwt
        // برای مثال میشه payload رو decode کرد و claim ساخت
        throw new NotImplementedException();
    }
}
