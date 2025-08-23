using ITSMDS.Extensions;
using ITSMDS.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ITSMDS.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddServiceInfrastructure(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddDistributedMemoryCache();
        builder.AddDatabases(configuration);
            ;


        return builder;
    }

    public static IHostApplicationBuilder AddDatabases(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("itsmds");
        builder.Services.AddDbContext<ApplicationDbContext>(d => d.UseSqlServer(cs));

        builder.Services.AddHealthChecks()
            .AddSqlServer(cs!, name: "sqlserver");
        ;

        return builder;
    }

    public static IServiceProvider AddServiceProvider(this IServiceProvider provider)
    {
        using (var scope = provider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }
        provider.SeedData();
        return provider;
    }
}
