using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetHostService
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private readonly AppSettingsModel _appSettingsModel;


        //IOptionsSnapshot<AppSettingsModel> settings
        public TimedHostedService(ILogger<TimedHostedService> logger, IOptions<AppSettingsModel> settings)
        {
            _logger = logger;
            _appSettingsModel = settings.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_appSettingsModel.RefreshInterval));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation(string.Format("[{0:yyyy-MM-dd hh:mm:ss}]Timed Background Service is working.", DateTime.Now));
            _logger.LogInformation(_appSettingsModel.ConnectString);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
