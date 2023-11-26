using HtmlAgilityPack;
using OfficeOpenXml;

namespace ScheduledCrawlerProject.Common
{
    public class CoinGeckoCrawlerLogic
    {
        private readonly ILogger<CoinGeckoCrawlerLogic> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public CoinGeckoCrawlerLogic(ILogger<CoinGeckoCrawlerLogic> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public void CrawlAndExportData(string url)
        {
            try
            {
                // 使用HttpClient下载页面内容
                var httpClient = _httpClientFactory.CreateClient();

                // 添加常见的请求头
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");
                httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");

                var html = httpClient.GetStringAsync(url).Result;

                // 解析HTML，提取表格数据
                var document = new HtmlDocument();
                document.LoadHtml(html);

                var table = document.DocumentNode.SelectSingleNode("//table[@data-coin-index-target='table']");
                if (table != null)
                {
                    var excelPackage = new ExcelPackage();
                    var ws = excelPackage.Workbook.Worksheets.Add("CoinData");


                    // 添加表头
                    int headerCol = 1;
                    foreach (var headerNode in table.SelectNodes("thead/tr/th"))
                    {
                        ws.Cells[1, headerCol].Value = headerNode.InnerText.Trim();
                        headerCol++;
                    }

                    int row = 2; // 从第二行开始写入数据
                    foreach (var rowNode in table.SelectNodes("tbody/tr"))
                    {
                        int col = 1;
                        foreach (var cellNode in rowNode.SelectNodes("td"))
                        {
                            ws.Cells[row, col].Value = cellNode.InnerText.Trim();
                            col++;
                        }
                        row++;
                    }

                    // 使用EPPlus导出数据到Excel
                    var fileName = $"CoinGeckoData_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    var filePath = Path.Combine(AppContext.BaseDirectory, "wwwroot", "export", fileName);

                    // 保存数据到指定的文件路径
                    excelPackage.SaveAs(new FileInfo(filePath));
                    _logger.LogInformation($"数据已导出至 {filePath}");
                }
                else
                {
                    // 在页面上未找到表格时记录错误
                    _logger.LogError("未在页面上找到表格。");
                }
            }
            catch (Exception ex)
            {
                // 在抓取和导出数据时发生错误时记录异常
                _logger.LogError(ex, "在抓取和导出数据时发生错误。");
            }
        }
    }
}
