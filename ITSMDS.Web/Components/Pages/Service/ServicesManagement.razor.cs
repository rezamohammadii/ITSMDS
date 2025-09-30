using BlazorBootstrap;
using ITSMDS.Domain.DTOs;
using ITSMDS.Domain.Enums;
using ITSMDS.Web.ApiClient;
using ITSMDS.Web.Services;
using ITSMDS.Web.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ITSMDS.Pages.Services
{
    public partial class ServicesManagement : ServiceManagementBase
    {
    }

    public class ServiceManagementBase : ComponentBase
    {
        [Inject] protected IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected ISweetAlertService SweetAlert { get; set; } = default!;
        [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
        [Inject] ServiceApiClient ServiceApi { get; set; } = default!;

        protected List<ServiceViewModel>? AllServicesData { get; set; }
        protected ServiceWidget? ServiceWidgetData { get; set; }
        protected ServiceWidgetCritical? ServiceWidgetCriticalData { get; set; }
        protected List<ServerDistribution>? ServerDistribution { get; set; }
        protected string SearchTerm { get; set; } = "";
        protected bool isDataLoaded = false;
        protected Grid<ServiceViewModel>? ServiceGrid;

        protected override async Task OnInitializedAsync()
        {
            await LoadServicesData();
        }

        private async Task LoadServicesData()
        {

            try
            {
                var res = await ServiceApi.GetServicesAsync();
                var resWidget = await ServiceApi.GetServiceWidgetAsync();
                if (res.Success && res.Data != null)
                {
                    AllServicesData = res.Data;
                    ServiceWidgetData = resWidget.Data;
                    isDataLoaded = true;

                    // پس از بارگذاری داده‌ها، Grid را refresh کن
                    await InvokeAsync(StateHasChanged);
                    await RefreshGridAsync();
                }
                else
                {
                    await SweetAlert.ShowErrorAsync(res.Message ?? "خطا در دریافت داده‌ها");
                }
            }
            catch (Exception ex)
            {
                await SweetAlert.ShowErrorAsync($"خطا: {ex.Message}");
            }
            try
            {
                // شبیه‌سازی بارگذاری داده‌ها
                await Task.Delay(500);

                // داده‌های نمونه
                ServiceWidgetCriticalData = new ServiceWidgetCritical
                {
                    VeryHighServiceCount = AllServicesData.Where(x => x.CriticalityScore == ServiceEnum.CriticalityScore.VeryHigh).Count(),
                    HighServiceCount = AllServicesData.Where(x => x.CriticalityScore == ServiceEnum.CriticalityScore.High).Count(),
                    NormalServiceCount = AllServicesData.Where(x => x.CriticalityScore == ServiceEnum.CriticalityScore.Normal).Count(),
                    LowServiceCount = AllServicesData.Where(x => x.CriticalityScore == ServiceEnum.CriticalityScore.Low).Count(),                   
                };

                ServerDistribution = new List<ServerDistribution>
                {
                    new() { ServerName = "سرور پایگاه داده", ServiceCount = 35 },
                    new() { ServerName = "سرور وب اصلی", ServiceCount = 28 },
                    new() { ServerName = "سرور ایمیل", ServiceCount = 15 },
                    new() { ServerName = "سرور فایل", ServiceCount = 12 },
                    new() { ServerName = "سرور پشتیبان", ServiceCount = 8 }
                };

                AllServicesData = GenerateSampleServices();
                isDataLoaded = true;

                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                await SweetAlert.ShowErrorAsync($"خطا در بارگذاری داده‌ها: {ex.Message}");
            }
        }
        private async Task RefreshGridAsync()
        {
            if (ServiceGrid != null)
            {
                await ServiceGrid.RefreshDataAsync();
            }
        }
        protected async Task<GridDataProviderResult<ServiceViewModel>> ServiceDataProvider(GridDataProviderRequest<ServiceViewModel> request)
        {
            if (!isDataLoaded || AllServicesData == null)
                return await Task.FromResult(request.ApplyTo(new List<ServiceViewModel>()));

            var filteredServices = FilterServices();
            return await Task.FromResult(request.ApplyTo(filteredServices));
        }

        protected List<ServiceViewModel> FilterServices()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm) || AllServicesData == null)
                return AllServicesData ?? new List<ServiceViewModel>();

            var searchLower = SearchTerm.ToLower();
            return AllServicesData.Where(service =>
                (service.ServerName?.ToLower().Contains(searchLower) == true) ||
                (service.Version?.ToLower().Contains(searchLower) == true) ||
                (service.ServerName?.ToLower().Contains(searchLower) == true) ||
                (service.Description?.ToLower().Contains(searchLower) == true)
            ).ToList();
        }

        protected async Task OnSearchChanged(ChangeEventArgs e)
        {
            SearchTerm = e.Value?.ToString() ?? string.Empty;
            if (ServiceGrid != null)
            {
                await ServiceGrid.RefreshDataAsync();
            }
            StateHasChanged();
        }

        protected void AddNewService()
        {
            NavigationManager.NavigateTo("/services/create");
        }

        protected void EditService(long id)
        {
            NavigationManager.NavigateTo($"/services/edit/{id}");
        }

        protected async Task DeleteService(long id)
        {
            var confirmed = await JSRuntime.InvokeAsync<bool>("showDeleteConfirmation",
                new object[] { "آیا از حذف این سرویس مطمئن هستید؟" });

            if (confirmed)
            {
                // منطق حذف سرویس
                await SweetAlert.ShowSuccessAsync("سرویس با موفقیت حذف شد");
                await LoadServicesData();
            }
        }

        protected async Task ShowServiceDetails(ServiceViewModel service)
        {
            // نمایش جزئیات سرویس در مودال
            await SweetAlert.ShowInfoAsync($"جزئیات سرویس: {service.ServerName}", service.Description ?? "بدون توضیحات");
        }

        protected BadgeColor GetCriticalityBadgeColor(ServiceEnum.CriticalityScore criticality)
        {
            return criticality switch
            {
                ServiceEnum.CriticalityScore.VeryHigh => BadgeColor.Danger,
                ServiceEnum.CriticalityScore.High => BadgeColor.Warning,
                ServiceEnum.CriticalityScore.Normal => BadgeColor.Info,
                ServiceEnum.CriticalityScore.Low => BadgeColor.Success,
                _ => BadgeColor.Secondary
            };
        }

        protected string GetCriticalityText(ServiceEnum.CriticalityScore criticality)
        {
            return criticality switch
            {
                ServiceEnum.CriticalityScore.VeryHigh => "بسیار بالا",
                ServiceEnum.CriticalityScore.High => "بالا",
                ServiceEnum.CriticalityScore.Normal => "متوسط",
                ServiceEnum.CriticalityScore.Low => "پایین",
                _ => "نامشخص"
            };
        }

        protected BadgeColor GetDistributionBadgeColor(int count)
        {
            return count switch
            {
                > 30 => BadgeColor.Danger,
                > 20 => BadgeColor.Warning,
                > 10 => BadgeColor.Info,
                _ => BadgeColor.Success
            };
        }

        private List<ServiceViewModel> GenerateSampleServices()
        {
            return new List<ServiceViewModel>
            {
                new() {
                    ServerId = 1,
                    ServerName = "سرویس پایگاه داده",
                    Version = "2.1.0",
                    CriticalityScore = ServiceEnum.CriticalityScore.VeryHigh,
                    IsActive = true,
                    Description = "سرویس مدیریت پایگاه داده اصلی"
                },
                new() {
                    ServerId = 2,
                    ServerName = "وب سرویس",
                    Version = "1.5.2",
                    CriticalityScore = ServiceEnum.CriticalityScore.High,
                    IsActive = true,
                    Description = "سرویس ارائه API های وب"
                },
                // ... سایر سرویس‌های نمونه
            };
        }
    }


    public class CriticalityStats
    {
        public int High { get; set; }
        public int Medium { get; set; }
        public int Low { get; set; }
        public int VeryLow { get; set; }
    }

    public class ServerDistribution
    {
        public string ServerName { get; set; } = string.Empty;
        public int ServiceCount { get; set; }
    }
}