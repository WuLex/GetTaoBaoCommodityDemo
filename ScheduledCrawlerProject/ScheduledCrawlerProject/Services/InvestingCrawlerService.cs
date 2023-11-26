using ScheduledCrawlerProject.Common;

namespace ScheduledCrawlerProject.Services
{
    public class InvestingCrawlerService : IHostedService, IDisposable
    {
        private readonly ILogger<InvestingCrawlerService> _logger;
        private readonly IServiceProvider _provider;
        private readonly string _odailyUrl = "https://cn.investing.com/news/cryptocurrency-news";
        private readonly Timer _timer;

        public InvestingCrawlerService(ILogger<InvestingCrawlerService> logger, IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;

            _timer = new Timer(DoWork, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("InvestingCrawlerService正在启动.");
            // 首次启动时手动执行一次，而不等待定时任务触发
            DoWork(null);
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (var scope = _provider.CreateScope())
            {
                var crawlerServiceLogic = scope.ServiceProvider.GetRequiredService<InvestingCrawlerLogic>();
                // 在这里执行爬取和导出Excel的逻辑
                crawlerServiceLogic.CrawlAndExportData(_odailyUrl);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("InvestingCrawlerService服务正在停止.");
            // 在这里执行停止任务的逻辑
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
