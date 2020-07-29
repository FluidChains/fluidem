using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Fluidem.Core.Models;
using Fluidem.Core.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fluidem.Core.Builder
{
    public static class BuilderExtensions
    {
        public static IServiceCollection AddFluidem<T>(this IServiceCollection services, Action<FluidemOptions> options)
            where T : class, IProvider
        {
            var build = services.AddSingleton<IProvider, T>();
            return build.Configure(options);
        }

        /// <summary>
        /// Register the exception handler and api endpoints. This method should be
        /// configure after UseRouting method
        /// </summary>
        /// <param name="app"></param>
        public static void UseFluidem(this IApplicationBuilder app)
        {
            app.ApplicationServices.GetService<IProvider>().BootstrapProvider();

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var baseException = contextFeature.Error.GetBaseException();
                        var detailError = new ErrorDetail
                        {
                            Id = Guid.NewGuid(),
                            Host = context.Request.Host.ToString(),
                            ExceptionType = baseException.GetType().FullName,
                            StatusCode = context.Response.StatusCode,
                            Message = baseException.Message,
                            StackTrace = baseException.StackTrace,
                            Source = baseException.Source,
                            TimeUtc = DateTimeOffset.UtcNow,
                            User = context.User.Claims
                                .SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty,
                            DetailJson = await ServerVariables.AsJson(context)
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

            // Configure api endpoints to return exceptions info
            app.UseEndpoints(endpoints =>
            {
                var options = app.ApplicationServices.GetService<IOptions<FluidemOptions>>();
                endpoints.MapGet(options.Value.ErrorLogApiUrl, async context =>
                {
                    var provider = context.RequestServices.GetService<IProvider>();
                    var list = await provider.GetExceptionsAsync();
                    await context.Response.WriteAsJsonAsync(list);
                });

                endpoints.MapGet(options.Value.ErrorLogApiUrl + "/{id}", async context =>
                {
                    context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                    var provider = context.RequestServices.GetService<IProvider>();
                    var id = context.Request.RouteValues.GetValueOrDefault("id");

                    if (id == null) return;
                    if (!Guid.TryParse(id.ToString(), out var guid)) return;

                    var ex = await provider.GetExceptionAsync(guid);
                    if (ex == null) return;

                    context.Response.StatusCode = (int) HttpStatusCode.OK;
                    await context.Response.WriteAsJsonAsync(ex);
                });
            });
        }
    }
}