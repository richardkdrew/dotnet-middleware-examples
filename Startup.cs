using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MiddlewareExamples
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {			
           // Simple path mapped to a builder branch
            app.Map(
                new PathString("/heartbeat"),
                branch => branch.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync("Boom, Boom, Boom!");
                })
            );
        }
    }
}