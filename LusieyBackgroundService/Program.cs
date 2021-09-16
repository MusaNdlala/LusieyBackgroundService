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
            //var builder = new ConfigurationBuilder()
                //.SetBasePath("path here") //<--You would need to set the path
              //  .AddJsonFile("appsettings.json"); //or what ever file you have the settings

            //IConfiguration configuration = builder.Build();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    //var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                    //optionBuilder.UseMySql(hostContext.Configuration.GetConnectionString("DbConnection"));
                    //optionBuilder.UseMySql("server=localhost;database=Lusiey_DB;user=root;password=ggjktdINSTR951702;Convert Zero Datetime=True");

                    //services.AddScoped<ApplicationDbContext>(d=> new ApplicationDbContext(optionBuilder.Options));
                    services.AddDbContext<ApplicationDbContext>(options =>
                        //options.UseMySql("server=localhost;database=Lusiey_DB;user=root;password=ggjktdINSTR951702;Convert Zero Datetime=True")
                        options.UseMySql(hostContext.Configuration.GetConnectionString("DbConnection"))
                        , ServiceLifetime.Scoped);
                    services.AddScoped<IEmailService, EmailService>();
                    services.AddScoped<IEmailSender, EmailSender>();
                    services.AddSingleton<IEmailTemplateService, EmailTemplateService>();
                });
    }
}