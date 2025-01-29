using Looloo.BlazorServer.Utilities;
using Microsoft.IdentityModel.Tokens;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Looloo.BlazorServer.Services.Food
{
    public class CityGrossService
    {
        private ChromeDriver? _driver;

        public async Task<List<ProductModel>> Search(string searchTerm)
        {
            var result = new List<ProductModel>();

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            _driver = new ChromeDriver(options);
            //_driver = _driver ?? new ChromeDriver();

            var searchUrl = "https://www.citygross.se/sok?Q=" + searchTerm.Trim() + "&page=1&store=";
            _driver.Navigate().GoToUrl(searchUrl);
            closeCookieWindow();

            var elements = _driver.FindElements(By.XPath("//*[@id=\"b-main\"]/div/div/div/div[2]/div[4]/div/div"));
            if (elements is not null)
            {
                foreach (var element in elements)
                {
                    var title = SeleniumExtensions.findElementByCss(element, "div.product-card__lower-container > div.details > h2");
                    var price = SeleniumExtensions.findElementByCss(element, "div.c-pricetag-grid > div > div > div > div > div > span");
                    var size = SeleniumExtensions.findElementByCss(element, "div.product-card__lower-container > div.details > h3");
                    var sizePrice = SeleniumExtensions.findElementByCss(element, "div.price-comparison.text-center.mt-10.mb-10 > span");
                    var imageUrl = SeleniumExtensions.findElementByCss(element, "div.product-image > div > picture > img");

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
                        Company = "Citygross",
                        ImageUrl = imageUrl == null ? string.Empty : "https://www.citygross.se"+ imageUrl.GetAttribute("srcset"),
                        SearchUrl = searchUrl
                    });
                }

            }

            //_driver.Quit();

            return result;
        }

        private void closeCookieWindow()
        {
            try
            {
                _driver?.Manage().Window.Maximize();

                Thread.Sleep(1000);

                var cookieButton = _driver?.FindElement(By.XPath("//*[@id=\"CybotCookiebotDialogBodyLevelButtonLevelOptinAllowAll\"]"));
                if (cookieButton is not null)
                {
                    cookieButton.Click();
                }

                Thread.Sleep(1000);

                var filterButton = _driver?.FindElement(By.XPath("//*[@id=\"b-main\"]/div/div/div/div[2]/div[3]/div[3]/button[2]"));
                if (filterButton is not null)
                {
                    filterButton.Click();
                }

                Thread.Sleep(2000);

            }
            catch (Exception e)
            {
            }
        }
    }
}
