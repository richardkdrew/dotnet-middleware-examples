using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace MiddlewareExamples
{
    public class Program
    {
        public static void Main(string[] args)
        {           
            var host = new WebHostBuilder()
                .UseKestrel()         
                .UseStartup<Startup>()               
                .Build();

            host.Run();        
        }
    }
}
