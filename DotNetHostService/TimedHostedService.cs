using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetHostService
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private readonly AppSettingsModel _appSettingsModel;
        private readonly IApplicationLifetime _appLifetime;


        //IOptionsSnapshot<AppSettingsModel> settings
        public TimedHostedService(ILogger<TimedHostedService> logger, IOptions<AppSettingsModel> settings, IApplicationLifetime appLifetime)
        {
            _logger = logger;
            _appSettingsModel = settings.Value;
            _appLifetime = appLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation($"[{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff")}] Timed Background Service is working.");
            _logger.LogInformation(_appSettingsModel.ConnectString);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");

            _logger.LogInformation(AppContext.BaseDirectory);
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_appSettingsModel.RefreshInterval));
        }

        private void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called.");
        }

        private void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
