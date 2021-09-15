using LusieyBackgroundService.DataConn;
using LusieyBackgroundService.Interface;
using LusieyBackgroundService.Service.EmailService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
                    services.AddHostedService<Worker>();
                    var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                        optionBuilder.UseMySql(hostContext.Configuration.GetConnectionString("DbConnection"));

                    services.AddSingleton<ApplicationDbContext>(d=> new ApplicationDbContext(optionBuilder.Options));
                    services.AddSingleton<IEmailService, EmailService>();
                });
    }
}