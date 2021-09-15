using System;
using System.Threading;
using System.Threading.Tasks;
using LusieyBackgroundService.DataConn;
using LusieyBackgroundService.Interface;
using LusieyBackgroundService.Service.EmailService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LusieyBackgroundService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEmailService _emailService;
        public Worker(ILogger<Worker> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    _logger.LogInformation("Email service: " +await _emailService.SetEmailsToSent(await _emailService.GetNonSentEmails()), DateTimeOffset.Now);
                }
                catch (Exception e){
                    _logger.LogError("Error: "+e.Message, DateTimeOffset.Now);
                }
                finally {await Task.Delay(10000, stoppingToken);}   
            }
        }
    }
}