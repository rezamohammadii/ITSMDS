using ITSMDS.Core.Application.Abstractions;
using ITSMDS.Core.Application.Services;
using ITSMDS.Extensions;
using ITSMDS.Infrastructure.Database;
using ITSMDS.Infrastructure.Repository;
using ITSMDS.Infrastructure.Services;
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
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(assemblies);
        }
        );

        return builder;
    }

    public static IHostApplicationBuilder AddDatabases(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("ITSMDS");
        builder.Services.AddDbContext<ApplicationDbContext>(d => d.UseSqlServer(cs ,sqlOption =>
        {
            sqlOption.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
        }));

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IUserService, UserService>();

        builder.Services.AddHealthChecks()
            .AddSqlServer(cs!, name: "sqlserver");
        ;

        return builder;
    }

    public static IServiceProvider AddServiceProvider(this IServiceProvider provider)
    {
        using (var scope = provider.CreateScope())
        {
            Thread.Sleep(2000);
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }
        provider.SeedData();
        return provider;
    }
}
