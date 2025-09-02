using ITSMDS.Web.ApiClient;

namespace ITSMDS.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddCustomHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        string baseAddress = configuration.GetSection("BaseAddress").Value!;
        services.AddHttpClient<WeatherApiClient>(client =>
        {

            client.BaseAddress = new(baseAddress);
        });
        services.AddHttpClient<UserApiClient>(client =>
        {

            client.BaseAddress = new(baseAddress);
        }); 
        services.AddHttpClient<RoleApiClient>(client =>
        {

            client.BaseAddress = new(baseAddress);
        });

        return services;
    }

}
