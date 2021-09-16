using System;
using System.Threading;
using System.Threading.Tasks;
using LusieyBackgroundService.DataConn;
using LusieyBackgroundService.Interface;
using LusieyBackgroundService.Service.EmailService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LusieyBackgroundService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        //private readonly IEmailService _emailService;
        IServiceProvider _serviceProvider;
        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider/*IEmailService emailService*/)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            //_emailService = emailService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    using (var scope = _serviceProvider.CreateScope()) {
                        var temp = await scope.ServiceProvider.GetRequiredService<IEmailService>()
                            .SetEmailsToSent(await scope.ServiceProvider.GetRequiredService<IEmailService>()
                            .GetNonSentEmails());
                        _logger.LogInformation("Email service: " + temp, DateTimeOffset.Now);
                        //_logger.LogInformation("Email service: " + await _emailService.SetEmailsToSent(await _emailService.GetNonSentEmails()), DateTimeOffset.Now);
                    }
                }
                catch (Exception e){
                    _logger.LogError("Error: "+e.Message, DateTimeOffset.Now);
                }
                finally {await Task.Delay(10000, stoppingToken);}   
            }
        }
    }
}