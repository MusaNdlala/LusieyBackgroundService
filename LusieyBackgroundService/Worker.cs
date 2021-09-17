using System;
using System.Threading;
using System.Threading.Tasks;
using LusieyBackgroundService.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LusieyBackgroundService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;
        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
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

                        var AudioService = await scope.ServiceProvider.GetRequiredService<IAudioService>()
                           .EmailAudios(await scope.ServiceProvider.GetRequiredService<IAudioService>()
                           .DeactivateAudios());
                        _logger.LogInformation("Email unPaid audio: " + AudioService, DateTimeOffset.Now);
                    }
                }
                catch (Exception e){_logger.LogError("Error: "+e.Message, DateTimeOffset.Now);}
                finally {await Task.Delay(20000, stoppingToken);}
            }
        }
    }
}