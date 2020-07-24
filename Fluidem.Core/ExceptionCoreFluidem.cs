using System;
using System.Net;
using Fluidem.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;


namespace Fluidem.Core
{
    public static class ExceptionCoreFluidem
    {
        public static void UseFluidem(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        //logger.LogError($"{contextFeature.Error}");
                        var obj = new DetailError()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Host = context.Request.Host.ToString(),
                            ExceptionType = context.GetType().ToString(),
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            StackTrace = contextFeature.Error.StackTrace
                        };
                        var middleware = context.RequestServices.GetService<PostgressFluidem>();
                        await middleware.SaveExceptionAsync(obj);
                    }
                });
            });
        }
    }
}