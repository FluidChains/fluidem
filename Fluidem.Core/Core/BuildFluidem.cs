using System;
using Fluidem.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Fluidem.Core
{
    public static class BuildFluidem
    {
        
        public static IServiceCollection ConfigureFluidem<T>(this IServiceCollection services) where T : class
        {
            return services.AddSingleton<T>();
        }
        
        public static IServiceCollection AddFluidem<T>(this IServiceCollection services, Action<Options> options) where T : class
        {
            var build = services.ConfigureFluidem<T>();
            build.Configure(options);
            return services.AddSingleton<T>();
        }
    }
}    