//// CustomAuthStateProvider.cs
//using Microsoft.AspNetCore.Components.Authorization;
//using Microsoft.JSInterop;
//using System.Net.Http.Headers;
//using System.Security.Claims;
//using System.Text.Json;

//public class CustomAuthStateProvider : AuthenticationStateProvider
//{
//    private readonly IJSRuntime _jsRuntime;
//    private readonly HttpClient _httpClient;
//    private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

//    public CustomAuthStateProvider(IJSRuntime jsRuntime, HttpClient httpClient)
//    {
//        _jsRuntime = jsRuntime;
//        _httpClient = httpClient;
//    }

//    public override Task<AuthenticationState> GetAuthenticationStateAsync()
//    {
//        // همیشه کاربر ناشناس رو برمی‌گردونیم تا موقع لود صفحه false نشه
//        return Task.FromResult(new AuthenticationState(_currentUser));
//    }

//    public async Task InitializeAsync()
//    {
//        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

//        if (!string.IsNullOrEmpty(token))
//        {
//            _httpClient.DefaultRequestHeaders.Authorization =
//                new AuthenticationHeaderValue("Bearer", token);

//            var claims = ParseClaimsFromJwt(token);
//            var identity = new ClaimsIdentity(claims, "jwt");
//            _currentUser = new ClaimsPrincipal(identity);

//            // اطلاع‌رسانی به Blazor برای رندر مجدد
//            NotifyAuthenticationStateChanged(
//                Task.FromResult(new AuthenticationState(_currentUser))
//            );
//        }
//    }

//    private IEnumerable<Claim> ParseClaimsFromJwt(string token)
//    {
//        // همون کدی که خودت نوشتی برای decode کردن jwt
//        // برای مثال میشه payload رو decode کرد و claim ساخت
//        throw new NotImplementedException();
//    }
//}



using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private readonly HttpClient _httpClient;
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public CustomAuthStateProvider(ProtectedLocalStorage protectedLocalStorage, HttpClient httpClient)
    {
        _protectedLocalStorage = protectedLocalStorage;
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var result = await _protectedLocalStorage.GetAsync<string>("authToken");

            if (!result.Success || string.IsNullOrEmpty(result.Value))
                return new AuthenticationState(_anonymous);

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", result.Value);

            var claims = ParseClaimsFromJwt(result.Value).ToList();

            // اضافه کردن Name claim برای Identity
            var nameClaim = claims.FirstOrDefault(c => c.Type == "unique_name" || c.Type == "sub");
            if (nameClaim != null)
            {
                claims.Add(new Claim(ClaimTypes.Name, nameClaim.Value));
            }

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            return new AuthenticationState(user);
        }
        catch
        {
            return new AuthenticationState(_anonymous);
        }
    }

    public void NotifyUserAuthentication(string token)
    {
        var claims = ParseClaimsFromJwt(token);
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(new AuthenticationState(_anonymous));
        NotifyAuthenticationStateChanged(authState);
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        return token.Claims;
    }
}
