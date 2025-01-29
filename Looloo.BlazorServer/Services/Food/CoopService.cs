using Looloo.BlazorServer.Models;
using Looloo.BlazorServer.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Looloo.BlazorServer.Services.Food
{
    //public class CoopService : IDisposable
    public class CoopService
    {
        private ChromeDriver? _driver;

        public async Task<List<ProductModel>> Search(string searchTerm)
        {
            var result = new List<ProductModel>();

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            _driver = new ChromeDriver(options);
            //_driver = _driver ?? new ChromeDriver();

            var searchUrl = $"https://www.coop.se/sok/?q={searchTerm.Trim()}";
            _driver.Navigate().GoToUrl(searchUrl);
            closeCookieWindow();

            var elements = _driver.FindElements(By.XPath("//*[@id=\"pageContent\"]/div/div[5]/div/div/div"));
            if (elements is not null)
            {
                foreach (var element in elements)
                {
                    var title = SeleniumExtensions.findElementByCss(element, "div.ProductTeaser-headingContainer > p > a");
                    var price = SeleniumExtensions.findElementByCss(element, "div.ProductTeaser-detailsContainer > div.SnZ2rtnf.sJx8F6kR.ProductTeaser-pricesContainer > span > span:nth-child(1)");
                    var size = SeleniumExtensions.findElementByCss(element, "div.ProductTeaser-headingContainer > div > div");
                    var sizePrice = SeleniumExtensions.findElementByCss(element, "div.ProductTeaser-detailsContainer > div.ProductTeaser-details > div:nth-child(2)");
                    var imageUrl = SeleniumExtensions.findElementByCss(element, "div.ProductTeaser-media > a > div > img");

                    //price = price.Replace("kr", "").Replace(",", ".").Trim();
                    //sizePrice = sizePrice.Replace("(", "").Replace(")", "").Replace(",", ".").Trim();

                    result.Add(new ProductModel
                    {
                        Title = title == null ? string.Empty : title.GetAttribute("text"),
                        Price = price == null ? string.Empty : price.Text,
                        //Price = decimal.Parse(price),
                        Size = size == null ? string.Empty : size.Text,
                        SizePrice = sizePrice == null ? string.Empty : sizePrice.Text,
                        //SizePrice = decimal.Parse(sizePrice),
                        Company = "Coop",
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
                _driver?.Manage().Window.Maximize();

                Thread.Sleep(1000);

                var cookieButton = _driver?.FindElement(By.XPath(".//a[contains(@class, 'cmpboxbtn cmpboxbtnyes cmptxt_btn_yes')]"));
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
