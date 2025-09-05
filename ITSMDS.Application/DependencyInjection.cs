

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ITSMDS.Core;

public static class DependencyInjection
{
 

    public static IHostApplicationBuilder AddCoreServices(this IHostApplicationBuilder builder)
    {
      
        return builder; 
    }
}
