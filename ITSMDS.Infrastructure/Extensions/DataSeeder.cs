using ITSMDS.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;

namespace ITSMDS.Extensions
{
    public static class DataSeeder
    {
        public static void  SeedData(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.SeedData();
        }
    }
}
