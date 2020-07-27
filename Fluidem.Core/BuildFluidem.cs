using System;
using System.Net;
using Fluidem.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Fluidem.Core
{
    public static class BuildFluidem
    {
        public static IServiceCollection AddFluidem<T>(this IServiceCollection services, Action<FluidemOptions> options) 
            where T : class, IProvider
        {
            var build = services.ConfigureFluidem<T>();
            return build.Configure(options);
        }
        
        private static IServiceCollection ConfigureFluidem<T>(this IServiceCollection services) 
            where T : class, IProvider
        {
            return services.AddSingleton<T>();
        }
        
        public static void UseFluidem(this IApplicationBuilder app)
        {
            var provider = app.ApplicationServices.GetService<IProvider>();
            provider.BootstrapProvider();
            
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var detailError = new DetailError
                        {
                            Id = Guid.NewGuid().ToString(),
                            Host = context.Request.Host.ToString(),
                            ExceptionType = context.GetType().ToString(),
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            StackTrace = contextFeature.Error.StackTrace
                        };
                        var middleware = context.RequestServices.GetService<IProvider>();
                        await middleware.SaveExceptionAsync(detailError);
                    }
                });
            });
        }
    }
}    