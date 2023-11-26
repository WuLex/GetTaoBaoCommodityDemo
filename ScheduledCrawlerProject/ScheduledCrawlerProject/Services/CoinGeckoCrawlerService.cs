using Microsoft.Extensions.Configuration;
using ScheduledCrawlerProject.Common;

namespace ScheduledCrawlerProject.Services
{
    public class CoinGeckoCrawlerService : IHostedService, IDisposable
    {
        private readonly ILogger<CoinGeckoCrawlerService> _logger;
        private readonly IServiceProvider _provider;
        private readonly string _coingeckoUrl = "https://www.coingecko.com/?items=300";
        private readonly bool _runInitialization; //是否是初始化运行标识
        public CoinGeckoCrawlerService(ILogger<CoinGeckoCrawlerService> logger, IServiceProvider provider, IConfiguration configuration)
        {
            _logger = logger;
            _provider = provider;


            // 设置定时任务
            // 创建并配置 Timer，在启动后立即执行 DoWork，之后每隔5分钟执行一次
            //var timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            // 创建并配置 Timer，在启动后不立即执行 DoWork，而是等待5分钟后开始执行，之后每隔5分钟执行一次
            var _timer = new Timer(DoWork, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
            //_runInitialization = configuration.GetValue<bool>("RunInitialization");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CoinGeckoCrawlerService正在启动.");
            //if (!_runInitialization)
            //{
            //    _logger.LogInformation("Initialization is skipped.");
            //    return Task.CompletedTask;
            //}
            //// 设置定时任务
            //var timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
          
            
            // 首次启动时不执行初始化逻辑，等待定时任务触发
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            // 在这里执行爬取和导出Excel的逻辑
            using (var scope = _provider.CreateScope())
            {
                var crawlerServiceLogic = scope.ServiceProvider.GetRequiredService<CoinGeckoCrawlerLogic>();

                // 爬取和导出Excel
                crawlerServiceLogic.CrawlAndExportData(_coingeckoUrl);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CoinGeckoCrawlerService服务正在停止.");

            // 在这里执行停止任务的逻辑

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            // 在这里执行资源清理的逻辑
        }
    }
}
