using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using System;

namespace MiddlewareExamples
{
    public class MetricsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Stopwatch _watch;
        private string _start;
        private string _complete;

        public MetricsMiddleware(RequestDelegate next)
        {
            _next = next;
            _watch = new Stopwatch();
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.OnStarting(SetMetricsHeaders, state: context);

            _watch.Start();

            _start = DateTime.Now.ToUniversalTime().ToString();

            await _next(context);

            _complete = DateTime.Now.ToUniversalTime().ToString();
        
        }

        private Task SetMetricsHeaders(object context)
        {
            var httpContext = (HttpContext)context;

            httpContext.Response.Headers["X-Processing-Started"] = new[] { _start };
            httpContext.Response.Headers["X-Processing-Completed"] = new[] { _complete };
            httpContext.Response.Headers["X-Processing-Time-Milliseconds"] = new[] { _watch.ElapsedMilliseconds.ToString() };

            return Task.FromResult(0);
        }
    }

    public static class MetricsMiddlewareExtensions
    {
        public static IApplicationBuilder UseMetrics(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MetricsMiddleware>();
        }
    }
}