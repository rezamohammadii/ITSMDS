using Microsoft.AspNetCore.Components.Authorization;

namespace ITSMDS.Web.Services;

public class PermissionService
{
    private readonly AuthenticationStateProvider _authStateProvider;
    private List<string> _userPermissions = new();
    private bool _initialized = false;

    public PermissionService(AuthenticationStateProvider authStateProvider)
    {
        _authStateProvider = authStateProvider;
    }

    public async Task InitializeAsync()
    {
        if (_initialized) return;

        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            var permsClaim = user.Claims.FirstOrDefault(c => c.Type == "Permissions")?.Value ?? "";

            // جدا کردن پرمیشن‌ها (با کاما یا هر جداکننده‌ای که JWT استفاده کرده)
            var temp = permsClaim.Split(',', StringSplitOptions.RemoveEmptyEntries);
            _userPermissions = temp.SelectMany(x => x.Split('|', StringSplitOptions.RemoveEmptyEntries))
                                   .ToList();
        }

        _initialized = true;
    }

    public bool HasPermission(string permission)
    {
        return _userPermissions.Contains(permission, StringComparer.OrdinalIgnoreCase);
    }

    public List<string> GetAllPermissions() => _userPermissions;
}
