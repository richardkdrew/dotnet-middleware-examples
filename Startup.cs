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
            // Separate file middleware, adds some basic request metrics to the response header and then 
            // has the option to allow execution (passthrough) to the next delegate in the pipeline (uses app.Use() internally).
            app.UseMetrics();

            // Inline middleware, runs a command and then has the option to allow execution (passthrough)
            // to the next delegate in the pipeline (app.Use()).
            app.Use((context, next) =>
            {
                // Add an arbitrary value to the response
                context.Response.Headers.Add("X-I-Am-The-Man", "true");

                // Call the next delegate/middleware in the pipeline
                return next();
            });

            // Inline middleware that branches the pipeline based on the logic supplied
            // example 1 - the path '/my-heartbeat' is matched the branch will be run.
            // This is the final step in the chain, there's no next delegate so if you
            // put anything below it will not be executed as part of the pipeline (app.Map()).
            app.Map(
                new PathString("/my-heartbeat"),
                branch => branch.Run(async context =>
                {
                    await context.Response.WriteAsync("Boom, Boom, Boom!");
                })
            );

            // example 2 - the path '/your-heartbeat' is matched the branch will be run.
            // This is the final step in the chain, there's no next delegate so if you
            // put anything below it will not be executed as part of the pipeline (app.Map()).
            app.Map(
                new PathString("/your-heartbeat"),
                branch => branch.Run(async context =>
                {
                    await context.Response.WriteAsync("Thud, Thud, Thud!");
                })
            );     

            // example 3 - a querystring value of 'my-thing' is matched the branch will be run.
            // This is the final step in the chain, there's no next delegate so if you
            // put anything below it will not be executed as part of the pipeline (app.MapWhen()).
            app.MapWhen(context => {
                return context.Request.Query.ContainsKey("my-thing");
            }, branch => branch.Run(async context =>
               {
                    await context.Response.WriteAsync("This is my thing!");
               })
            );       

            // Inline middleware as a catch-all. No path string so anything that doesn't match
            // the rules above will come here. This is the final step in the chain, there's no next 
            // delegate so if you put anything below it will not be executed as part of the pipeline (app.Run())
            app.Run(async context =>
            {
                await context.Response.WriteAsync("This is the catch-all handler.");
            });
        }
    }
}