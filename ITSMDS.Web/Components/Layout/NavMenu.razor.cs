using ITSMDS.Application.Constants;
using ITSMDS.Application.Constants;
using ITSMDS.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
namespace ITSMDS.Web.Components.Layout;

public partial class NavMenu
{
    [Inject]
    public AuthenticationStateProvider AuthStateProvider { get; set; }
    [Inject]
    public PermissionService PermissionService { get; set; }

    private List<MenuItem> userMenuItems = new();
    private List<MenuItem> roleMenuItems = new();


    public class MenuItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }

        public MenuItem(string title, string url, string icon)
        {
            Title = title;
            Url = url;
            Icon = icon;
        }
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await PermissionService.InitializeAsync();

            if (PermissionService.HasPermission(PermissionName.USER_READ))
            {
                userMenuItems.Add(new MenuItem("لیست کاربران", "/users", "fas fa-list"));
            }
            if (PermissionService.HasPermission(PermissionName.USER_CREATE))
            {
                userMenuItems.Add(new MenuItem("افزودن کاربر", "/user/create", "fas fa-user-plus"));
            }

            if (PermissionService.HasPermission(PermissionName.PERMISSION_MANAGE))
            {
                userMenuItems.Add(new MenuItem("مدیریت دسترسی‌ها", "/user/permission", "fas fa-user-shield"));
            }
            if (PermissionService.HasPermission(PermissionName.ROLE_READ))
            {
                roleMenuItems.Add(new MenuItem("لیست نقش ها", "/role/list", "fas fa-list"));
            }
            if (PermissionService.HasPermission(PermissionName.ROLE_CREATE))
            {
                roleMenuItems.Add(new MenuItem("افزودن نقش", "/role/create", "fas fa-user-plus"));
            }
            StateHasChanged();
        }
    }

    private async Task Logout()
    {
        await AuthService.LogoutAsync();
    }
}
