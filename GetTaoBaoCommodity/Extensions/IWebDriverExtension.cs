using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTaoBaoCommodity.Extensions
{
    public static class IWebDriverExtension
    {
        public static IWebElement FindElementIfExists(this IWebDriver driver, By by)
        {
            var elements = driver.FindElements(by);
            return (elements.Count >= 1) ? elements.First() : null;
        }

    }
}
