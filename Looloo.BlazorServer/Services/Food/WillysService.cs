using Looloo.BlazorServer.Models;
using Looloo.BlazorServer.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Looloo.BlazorServer.Services.Food
{
    public class WillysService
    {
        private ChromeDriver? _driver;

        public async Task<List<ProductModel>> Search(string searchTerm)
        {
            var result = new List<ProductModel>();

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            _driver = new ChromeDriver(options);
            //_driver = _driver ?? new ChromeDriver();

            var searchUrl = $"https://www.willys.se/sok?q={searchTerm.Trim()}";

            _driver.Navigate().GoToUrl(searchUrl);
            closeCookieWindow();

            var elements = _driver.FindElements(By.XPath("//*[@id=\"__next\"]/div/div[4]/main/section/div/div[4]/div/div[1]/div"));
            if (elements is not null)
            {
                foreach (var element in elements)
                {
                    var title = SeleniumExtensions.findElementByCss(element, "div.sc-7fa12c71-6.gMsSQO");
                    var price = SeleniumExtensions.findElementByCss(element, "span.sc-49402b38-2.gjPFeg");
                    var size = SeleniumExtensions.findElementByCss(element, "div.sc-7fa12c71-7.hzKTNx > span");
                    var sizePrice = SeleniumExtensions.findElementByCss(element, "div.sc-7fa12c71-5.dbJhzZ > div");
                    var img = SeleniumExtensions.findElementByCss(element, "div.sc-7fa12c71-2.bHOIhx > a > div > div > img");

                    //price = price.Replace("kr", "").Replace(",", ".").Trim();
                    //sizePrice = sizePrice.Replace("(", "").Replace(")", "").Replace(",", ".").Trim();

                    result.Add(new ProductModel
                    {
                        Title = title == null ? string.Empty : title.Text,
                        Price = price == null ? string.Empty : price.Text,
                        //Price = decimal.Parse(price),
                        Size = size == null ? string.Empty : size.Text,
                        SizePrice = sizePrice == null ? string.Empty : sizePrice.Text,
                        //SizePrice = decimal.Parse(sizePrice),
                        Company = "Willys",
                        ImageUrl = img == null ? string.Empty : img.GetAttribute("src"),
                        SearchUrl = searchUrl
                    });
                }

            }

            _driver.Quit();

            return result;
        }

        private void closeCookieWindow()
        {
            try
            {
                // Maximize the current window
                _driver?.Manage().Window.Maximize();

                Thread.Sleep(1000);

                var cookieButton = _driver?.FindElement(By.XPath("//*[@id=\"onetrust-accept-btn-handler\"]"));
                if (cookieButton is not null)
                {
                    cookieButton.Click();
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
