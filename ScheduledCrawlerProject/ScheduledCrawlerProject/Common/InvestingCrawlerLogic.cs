using HtmlAgilityPack;
using Newtonsoft.Json;
using OfficeOpenXml;
using ScheduledCrawlerProject.Models;

namespace ScheduledCrawlerProject.Common
{
    public class InvestingCrawlerLogic
    {
        private readonly ILogger<InvestingCrawlerLogic> _logger;

        public InvestingCrawlerLogic(ILogger<InvestingCrawlerLogic> logger)
        {
            _logger = logger;
        }

        public void CrawlAndExportData(string url)
        {
            try
            {
                using var httpClient = new HttpClient();

                // 添加常见的请求头
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");
                httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
                var html = httpClient.GetStringAsync(url).Result;

                var document = new HtmlDocument();
                document.LoadHtml(html);

                // 选择所有的文章项
                //var articleNodes = document.DocumentNode.SelectNodes("//article[@class='js-article-item articleItem     ']");
                var articleNodes = document.DocumentNode.SelectNodes("//div[@class='largeTitle']/article[@class='js-article-item articleItem     ']");
                if (articleNodes != null)
                {
                    var excelPackage = new ExcelPackage();
                    var ws = excelPackage.Workbook.Worksheets.Add("CryptoNews");

                    // 添加表头
                    ws.Cells[1, 1].Value = "标题";
                    ws.Cells[1, 2].Value = "描述";
                    //// 添加标题行
                    //ws.Cells["A1"].Value = "标题";
                    //ws.Cells["B1"].Value = "描述";

                    int row = 2;
                    // 遍历文章项并提取标题和描述
                    foreach (var articleNode in articleNodes)
                    {
                      
                        var title = articleNode.SelectSingleNode(".//a[@class='title']")?.InnerText.Trim() ?? "";
                        var description = articleNode.SelectSingleNode(".//p")?.InnerText.Trim() ?? "";
                        // 将标题和描述写入Excel
                        //ws.Cells["A" + row].Value = title;
                        //ws.Cells["B" + row].Value = description;
                        ws.Cells[row, 1].Value = title;
                        ws.Cells[row, 2].Value = description;
                        row++;
                    }
                     
                    // 使用EPPlus导出数据到Excel
                    var fileName = $"InvestinData_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    var filePath = Path.Combine(AppContext.BaseDirectory, "wwwroot", "export", fileName);

                    // 将数据保存到指定的文件路径
                    excelPackage.SaveAs(new FileInfo(filePath));
                    _logger.LogInformation($"数据已导出至 {filePath}");
                }
                else
                {
                    // 在页面上未找到文章节点时记录错误
                    _logger.LogError("未在页面上找到文章节点。");
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
