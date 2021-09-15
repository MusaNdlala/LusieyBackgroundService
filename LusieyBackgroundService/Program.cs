using LusieyBackgroundService.DataConn;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LusieyBackgroundService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    //services.Configure<MyConfig>(hostContext.Configuration.GetSection("MyConfig"));
                    services.AddHostedService<Worker>();
                    //services.AddTransient<MyConfig>(_ => _.GetRequiredService<IOptions<MyConfig>>().Value);
                    //services
                    services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseMySql(hostContext.Configuration.GetSection("DbConnection").Value));
                           
                });
    }
}
