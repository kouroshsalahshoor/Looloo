using Looloo.BlazorServer.Models;
using Looloo.BlazorServer.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Looloo.BlazorServer.Services.Food
{
    public class HemkopService
    {
        private ChromeDriver? _driver;

        public async Task<List<ProductModel>> Search(string searchTerm)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            _driver = new ChromeDriver(options);
            //_driver = _driver ?? new ChromeDriver();

            var searchUrl = $"https://www.hemkop.se/sok?q={searchTerm.Trim()}";
            _driver.Navigate().GoToUrl(searchUrl);
            closeCookieWindow();

            var result = new List<ProductModel>();

            var elements = _driver.FindElements(By.XPath("//*[@id=\"__next\"]/div[2]/div[2]/div/div/div/div[4]/div"));
            if (elements is not null)
            {
                foreach (var element in elements)
                {
                    var title = SeleniumExtensions.findElementByCss(element, "div.sc-56f3097b-1.hkpGQL > a > p");
                    var price = SeleniumExtensions.findElementByCss(element, "div.sc-94a42247-0.fepGlo > div > div > p > span:nth-child(1)");
                    var size = SeleniumExtensions.findElementByCss(element, "div.sc-56f3097b-4.fWarFk > p > span:nth-child(2)");
                    var sizePrice = SeleniumExtensions.findElementByCss(element, "div.sc-56f3097b-8.dNMbpO > div > p");
                    var imageUrl = SeleniumExtensions.findElementByCss(element, "div.sc-82e986e3-0.eviaLe > a > div > img");

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
                        Company = "Hemköp",
                        ImageUrl = imageUrl == null ? string.Empty : imageUrl.GetAttribute("src"),
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

                //Thread.Sleep(1000);

                //var loginButton = _driver?.FindElement(By.XPath(".//button[contains(@class, 'gUGSFhfR CkqGWkRo ucdesrxw qfkHWAKt')]"));
                //if (loginButton is not null)
                //{
                //    loginButton.Click();
                //}
            }
            catch (Exception e)
            {
            }
        }
    }
}
