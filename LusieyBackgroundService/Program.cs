using LusieyBackgroundService.DataConn;
using LusieyBackgroundService.DataConn.DbInterface;
using LusieyBackgroundService.Interface;
using LusieyBackgroundService.Service.Audios;
using LusieyBackgroundService.Service.EmailService;
using LusieyBackgroundService.Service.Tester;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


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
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseMySql(hostContext.Configuration.GetConnectionString("DbConnection"))
                        , ServiceLifetime.Scoped);

                    services.AddSingleton<IEmailService, EmailService>();
                    services.AddSingleton<IEmailSender, EmailSender>();
                    services.AddSingleton<IAudioService, AudioService>();
                    services.AddSingleton<DataConn.DbInterface.IDbConnectHelper, DbConnectHelper>();
                    services.AddSingleton<IEmailTemplateService, EmailTemplateService>();
                    //services.AddSingleton<ITester, Tester>();
                });
    }
}