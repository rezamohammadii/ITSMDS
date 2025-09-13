using Microsoft.JSInterop;

namespace ITSMDS.Web;


public interface ISweetAlertService
{
    Task ShowSuccessAsync(string message, string title = "موفقیت");
    Task ShowErrorAsync(string message, string title = "خطا");
    Task ShowWarningAsync(string message, string title = "هشدار");
    Task ShowInfoAsync(string message, string title = "اطلاعات");
    Task<bool> ShowConfirmAsync(string message, string title = "تأیید عملیات");
}

public class SweetAlertService : ISweetAlertService
{
    private readonly IJSRuntime _jsRuntime;

    public SweetAlertService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task ShowSuccessAsync(string message, string title = "موفقیت")
    {
        await _jsRuntime.InvokeVoidAsync("Swal.fire", title, message, "success");
    }

    public async Task ShowErrorAsync(string message, string title = "خطا")
    {
        await _jsRuntime.InvokeVoidAsync("Swal.fire", title, message, "error");
    }

    public async Task ShowWarningAsync(string message, string title = "هشدار")
    {
        await _jsRuntime.InvokeVoidAsync("Swal.fire", title, message, "warning");
    }

    public async Task ShowInfoAsync(string message, string title = "اطلاعات")
    {
        await _jsRuntime.InvokeVoidAsync("Swal.fire", title, message, "info");
    }

    public async Task<bool> ShowConfirmAsync(string message, string title = "تأیید عملیات")
    {
        return await _jsRuntime.InvokeAsync<bool>("Swal.fire", new
        {
            title = title,
            text = message,
            icon = "warning",
            showCancelButton = true,
            confirmButtonColor = "#3085d6",
            cancelButtonColor = "#d33",
            confirmButtonText = "بله",
            cancelButtonText = "خیر",
            focusCancel = true
        });
    }
}
