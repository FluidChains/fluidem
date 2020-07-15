using System;
using System.Net;
using Fluidem.Sample.WebApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Fluidem.Sample.WebApi.Extension
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
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
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.StackTrace.ToString()
                        };
                        Console.Write(obj);
                        await context.Response.WriteAsync(obj.ToString());
                    }
                });
            });
        }
    }
}