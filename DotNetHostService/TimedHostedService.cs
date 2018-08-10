using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace DotNetHostService
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly AppSettingsModel _appSettingsModel;
        private readonly IApplicationLifetime _appLifetime;
        private readonly AzureStorage _azureStorage;
        private System.Timers.Timer _timer;
        private Thread _testThread;


        //IOptionsSnapshot<AppSettingsModel> settings
        public TimedHostedService(ILogger<TimedHostedService> logger, IOptions<AppSettingsModel> settings, IApplicationLifetime appLifetime, AzureStorage azureStorage)
        {
            _logger = logger;
            _appSettingsModel = settings.Value;
            _appLifetime = appLifetime;
            _azureStorage = azureStorage;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");
            LogHelper.LogFile(LogType.Info, "Timed Background Service is starting.");
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
            LogHelper.LogFile(LogType.Info, "Timed Background Service is stopping.");
            //_timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");

            LogHelper.LogFile(LogType.Info, "OnStarted has been called.");

            InitTimer();

            _logger.LogInformation(AppContext.BaseDirectory);
            //_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_appSettingsModel.RefreshInterval));
        }

        private void OnStopping()
        {
            LogHelper.LogFile(LogType.Info, "OnStopping has been called.");
            _logger.LogInformation("OnStopping has been called.");
        }

        private void OnStopped()
        {
            LogHelper.LogFile(LogType.Info, "OnStopped has been called.");
            _logger.LogInformation("OnStopped has been called.");
        }


        private void InitTimer()
        {
            _timer = new System.Timers.Timer();
            _timer.Enabled = true;
            _timer.Interval = _appSettingsModel.RefreshInterval;
            _timer.Elapsed += timer_Elapsed;
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _testThread = new Thread(new ThreadStart(TestMessage));
                _testThread.IsBackground = true;
                _testThread.Start();
            }
            catch (Exception)
            {

            }
        }

        private void TestMessage()
        {
            try
            {
                _timer.Enabled = false;
                _timer.Stop();

                _logger.LogInformation($"[{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff")}] Timed Background Service is working.");
                _logger.LogInformation(_appSettingsModel.ConnectString);
                _azureStorage.InitAzureStorageConnectAsync().GetAwaiter().GetResult();

                _timer.Enabled = true;
                _timer.Start();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"TestMessage Exception:{ex.ToString()}");
                _timer.Enabled = true;
                _timer.Start();
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
            LogHelper.LogFile(LogType.Info, "Dispose has been called.");
        }
    }
}
