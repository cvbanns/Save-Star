using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NCrontab;

namespace WebApi.Services
{
    public class SavingsBackgroundService : BackgroundService
    {
        private readonly ILogger<SavingsBackgroundService> _logger;
        private DateTime _nextRun;
        public IServiceScopeFactory _serviceScopeFactory { get; set; }

        //private string Schedule => "* 0 2 * * *"; // runs everyday
        private string Schedule => "* */5 * * * *"; // runs everyday

        public SavingsBackgroundService(ILogger<SavingsBackgroundService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var _schedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });

                if (now > _nextRun)
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var _savingsConfigurationService = scope.ServiceProvider.GetService<ISavingsConfigurationService>();
                        _savingsConfigurationService.ProcessSavings();
                    }
                    _nextRun = _schedule.GetNextOccurrence(now);

                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
