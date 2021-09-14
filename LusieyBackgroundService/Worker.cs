using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LusieyBackgroundService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                }
                catch (Exception e){
                    _logger.LogError("Error: "+e.Message, DateTimeOffset.Now);
                }
                finally {await Task.Delay(30000, stoppingToken);}   
            }
        }
    }
}