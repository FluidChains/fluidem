using System;
using System.Net;
using Fluidem.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fluidem.Core
{
    public static class Builder
    {
        public static IServiceCollection AddFluidem<T>(this IServiceCollection services, Action<FluidemOptions> options) 
            where T : class, IProvider
        {
            var build = services.AddSingleton<IProvider, T>();
            return build.Configure(options);
        }
        
        public static void UseFluidem(this IApplicationBuilder app)
        {
            app.ApplicationServices.GetService<IProvider>().BootstrapProvider();
            
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var detailError = new ErrorDetail
                        {
                            Id = Guid.NewGuid(),
                            Host = context.Request.Host.ToString(),
                            ExceptionType = context.GetType().ToString(),
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            StackTrace = contextFeature.Error.StackTrace,
                            TimeUtc = DateTime.UtcNow
                        };
                        var provider = context.RequestServices.GetService<IProvider>();
                        try
                        {
                            await provider.SaveExceptionAsync(detailError);
                        }
                        catch (Exception e)
                        {
                            var logger = context.RequestServices.GetService<ILogger<IProvider>>();
                            logger.LogError($"Error saving exception: {e.Message}", e);
                        }
                    }
                });
            });
        }
    }
}    