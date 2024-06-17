using CCConstants7;
using ccEventNotifications;
using CCNotifications;
using System.Threading;
using System.Timers;

namespace ccEventNotificationService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private System.Timers.Timer _timer = new System.Timers.Timer(180000);
        private EventNotifier _notifier;
        private CCEmail _mailer = new CCEmail(CommonEnums.CompEnum.Carecraft);

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _notifier = new EventNotifier();
            if (!_notifier.CheckEnvironment())
            {
                var msg = "CCEventNotifier Service environment not set";
                _mailer.SendITSupport("CCEventNotifier Service Environment", msg);
                throw new Exception(msg);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() => SetUpTimer());
        }

        private void SetUpTimer()
        {
            _timer.Elapsed += new ElapsedEventHandler(ElapsedTime);
            _timer.Start();
        }


        private void ElapsedTime(object source, ElapsedEventArgs args)
        {
            try
            {
                _notifier.Process();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"CCEventNotifier fired an error {ex.Message}");
                _mailer.SendITSupport("CCEventNotifier", $"CCEventNotifier fired an error {ex.Message}").Wait();
            }
        }
    }
}
