using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using GetTaoBaoCommodity.Extensions;
using GetTaoBaoCommodity.Models;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace GetTaoBaoCommodity
{
    class Program
    {
        static void Main(string[] args)
        {
            string URL = @"https://s.taobao.com/search?q={0}&s={1}";

            //string loginUrl = @"https://s.taobao.com/search?q={0}&s={1}";
            Console.WriteLine("请输入你要查找的商品:");
            string q = Console.ReadLine();
            Console.WriteLine("请输入要查找到多少页(每页10个商品)");

            int s = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

            //-----------------------------------------------------------------
            //ChromeOptions op = new ChromeOptions();
            //op.AddArguments("--headless"); //开启无gui模式
            //op.AddArguments("--no-sandbox"); //停用沙箱以在Linux中正常运行
            //ChromeDriver cd = new ChromeDriver(Environment.CurrentDirectory, op, TimeSpan.FromSeconds(180));
            //cd.Navigate().GoToUrl("http://chart.icaile.com/sd11x5.php");
            //string text = cd.FindElementById("fixedtable").Text;
            //cd.Quit();
            //Console.WriteLine(text);
            //Console.Read();
            for (int i = 1; i <= s; i++)
            {
                Console.WriteLine("--第{0}页--", i);

                #region 旧逻辑

                //string htmlall = GetHtml(string.Format(URL, q, 22 * i));

                #endregion

                #region 新方法

                string htmlall = GrabHtml(string.Format(URL, q, 22 * i));

                #endregion

                //List<List<string>> listData = Rule.GetAll(htmlall.Split('\n')[Rule.Inline]);
                List<DataInfoModel> listData = new List<DataInfoModel>();
               
                #region 解析html，输出list
                // 创建 HTML 解析器对象
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlall);

                // 使用 XPath 表达式选择商品元素
                var items = doc.DocumentNode.SelectNodes("//div[@class='item J_MouserOnverReq  ']");

                if (items!=null)
                {
                    // 遍历每个商品元素，并提取信息
                    foreach (var item in items)
                    {
                        #endregion
                        // 获取商品图片
                        var picNode = item.SelectSingleNode(".//div[@class='pic']/a/img");
                        var picUrl = picNode.GetAttributeValue("data-src", "");


                        // 获取商品名称
                        var titleNode = item.SelectSingleNode(".//div[@class='row row-2 title']/a");
                        var title = titleNode.InnerText;

                        // 获取商品详情链接
                        var detailNode = item.SelectSingleNode(".//div[@class='row row-2 title']/a");
                        var detailUrl = detailNode.GetAttributeValue("href", "");

                        // 获取商品价格
                        var priceNode = item.SelectSingleNode(".//div[@class='price g_price g_price-highlight']/strong");
                        var price = priceNode.InnerText;

                        // 获取卖家名称
                        var shopnameNode = item.SelectSingleNode(".//div[@class='shop']/a/span[2]");
                        var shopname = shopnameNode.InnerText;

                        // 获取卖家ID
                        var sellerIdNode = item.SelectSingleNode(".//div[@class='shop']/a");
                        var sellerId = sellerIdNode.GetAttributeValue("data-userid", "");

                        // 获取付款人数
                        var soldNode = item.SelectSingleNode(".//div[@class='deal-cnt']");
                        var soldText = soldNode.InnerText.Trim();
                        var sold = soldText.EndsWith("人付款") ? soldText.Substring(0, soldText.Length - 3) : "0";

                        // 获取店铺所在地
                        var locationNode = item.SelectSingleNode(".//div[@class='location']");
                        var location = locationNode.InnerText.Trim();


                        listData.Add(new DataInfoModel()
                        {
                            TitleName = title,
                            Price = price,
                            PicUrl = picUrl,
                            ShopName = shopname,
                            DetailUrl = detailUrl,
                            UserId = sellerId,
                            Address = location,
                            BuyNumber = sold,
                        });
                        // Console.WriteLine(title + " - " + shopname + " - " + sellerId + " - " + sold + " - " + location + " - " + picUrl);
                    }

                    string fullPath = Path.GetFullPath("..");
                    string tmpPath = Path.Combine(Environment.CurrentDirectory, @"htmlfile\", "Template.htm");
                    string resultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"htmlfile\", "result.htm");
                    HtmlHelper.WriteFile(listData, tmpPath, resultPath);

                }

                #region 遍历输出爬取信息

                //for (int x = 0; x < value.Count; x++)
                //{
                //    for (int y = 0; y < value[x].Count; y++)
                //    {
                //        Console.WriteLine("第{0}个-{1}-{2}", x + 1, Rule.Information[y][1], value[x][y]);
                //    }
                //} 

                #endregion

                Console.WriteLine("--第{0}页完毕--", i);
            }

            Console.ReadKey();
        }

        //获取网页源码
        static string GetHtml(string url)
        {
            WebClient wc = new WebClient();

            byte[] data = wc.DownloadData(url);
            return Encoding.UTF8.GetString(data);
        }

        static string GrabHtml(string url)
        {
            ChromeOptions options = new ChromeOptions();
            //options.AddArguments("--headless"); //开启无gui模式
            //options.AddArguments("--no-sandbox"); //停用沙箱以在Linux中正常运行
            // 创建ChromeDriver
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            var driverOptions = new ChromeOptions();
            driverOptions.AddArgument("start-maximized");
            driverOptions.AddArgument("disable-infobars");
            driverOptions.AddArgument("--disable-extensions");
            driverOptions.AddArgument("--disable-gpu");
            driverOptions.AddArgument("--disable-dev-shm-usage");
            driverOptions.AddArgument("--no-sandbox");
            //var driver = new ChromeDriver(driverService, driverOptions);
            
            //创建chrome驱动程序
            using (IWebDriver webDriver = new ChromeDriver(Environment.CurrentDirectory, driverOptions, TimeSpan.FromSeconds(180)))
            {
                //跳至数据列表页
                webDriver.Navigate().GoToUrl(url);

                IWebElement buttonElement = webDriver.FindElementIfExists(By.CssSelector(".fm-button.fm-submit.password-login"));
                //如果元素存在，说明跳转到了登录页，存在登录按钮
                if (buttonElement != null)
                {
                    //如果未登录账户，需要处理登录

                    //找到页面上的用户名和密码框 输入
                    webDriver.FindElement(By.Id("fm-login-id")).SendKeys("9458*****@qq.com");
                    webDriver.FindElement(By.Id("fm-login-password")).SendKeys("wu15***********");

                    //窗口最大化
                    webDriver.Manage().Window.Maximize();

                    //点击搜索按钮
                    webDriver.FindElement(By.CssSelector(".fm-button.fm-submit.password-login")).Click();

                    Thread.Sleep(5000);
                    //如果是操作页面中的iframe页面，需要切换到iframe中
                    IWebElement iframe = webDriver.FindElement(By.Id("baxia-dialog-content"));

                    if (iframe != null)
                    {
                        webDriver.SwitchTo().Frame(iframe);

                        //找到滑块元素
                        #region 滑块验证码  方法一
                        /*
                        var slide = webDriver.FindElement(By.Id("nc_1_n1z"));
                        if (slide != null)
                        {

                            var verifyContainer = webDriver.FindElement(By.CssSelector(".nc-lang-cnt"));
                            var width = verifyContainer.Size.Width;
                            var action = new Actions(webDriver);
                            //点击并按住滑块元素
                            action.ClickAndHold(slide).Perform();
                            Random random = new Random();
                            int offset = 0;
                            //模仿人工滑动
                            const int minOffset = 10;
                            const int maxOffset = 30;
                            while (width > offset & offset < 258)
                            {
                                offset += random.Next(minOffset, maxOffset);
                                offset = offset > 258 ? 258 : offset;
                                System.Threading.Thread.Sleep(500);
                                action.MoveByOffset(offset, 0);
                                //var code = webDriver.FindElement(By.CssSelector(".nc-lang-cnt")).Text;
                                //var code = verifyContainer.Text;
                                //if (code.Contains("验证通过"))
                                //{
                                //    break;
                                //}
                                System.Threading.Thread.Sleep(offset * minOffset);

                            }

                            action.Release().Perform();
                            //点击验证按钮
                            //action.Click(webDriver.FindElement(By.CssSelector("#verify"))).Perform();
                        }

                        //提交按钮
                        //webDriver.FindElement(By.CssSelector(".fm-button.fm-submit.password-login")).Click();
                        */
                        #endregion


                        #region 滑块验证码  方法二
                        // 处理滑块验证码
                        // 等待滑块验证码出现
                       IWebElement slider = webDriver.FindElement(By.Id("nc_1_n1z"));
                        //IWebElement slider = webDriver.FindElement(By.Id("nc_1__bg"));
                        Actions builder = new Actions(webDriver);

                        // 模拟滑块滑动操作
                        builder.ClickAndHold(slider).MoveByOffset(258, 0).Release().Perform();
                        //builder.ClickAndHold(slider).MoveByOffset(300, 0).Release().Perform();

                        // 等待登录成功并跳转到淘宝首页
                        webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1000);
                        //IWebElement homeTab = webDriver.FindElement(By.XPath("//a[@data-spm='tbindex']/i"));
                        //homeTab.Click();
                        #endregion

                    }
                }
                else
                {

                }

                Thread.Sleep(3000);
                string html = webDriver.PageSource.Trim(); //数据列表页面内容

                //关闭浏览器
                //webDriver.Quit();

                return html;
            }
        }

        static bool DoNext(IWebDriver webDriver)
        {
            bool result = false;
            System.Threading.Thread.Sleep(2000);
            var nextPageButton = webDriver.FindElement(By.XPath("//a[contains(@title,'下一页')]"));

            var strClass = nextPageButton.GetAttribute("class");
            if (strClass != "k-link k-pager-nav k-state-disabled")
            {
                nextPageButton.Click();
                result = true;
            }

            System.Threading.Thread.Sleep(2000);
            return result;
        }
    }
}

