using Looloo.BlazorServer.Models;
using Looloo.BlazorServer.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Looloo.BlazorServer.Services
{
    //public class CoopService : IDisposable
    public class CoopService
    {
        private ChromeDriver? _driver;

        public async Task<List<ProductViewModel>> Search(string searchTerm)
        {
            _driver = _driver ?? new ChromeDriver();

            _driver.Url = $"https://www.coop.se/handla/sok/?q={searchTerm.Trim()}";
            closeCookieWindow();

            var result = new List<ProductViewModel>();

            var elements = _driver.FindElements(By.CssSelector("div.ProductTeaser-info"));
            if (elements is not null)
            {

                foreach (var element in elements)
                {
                    var title = SeleniumExtensions.findElement(element, "div.ProductTeaser-headingContainer > p > a");
                    var price = SeleniumExtensions.findElement(element, "div.ProductTeaser-detailsContainer > div.SnZ2rtnf.sJx8F6kR.ProductTeaser-pricesContainer > span > span:nth-child(1)");
                    var size = SeleniumExtensions.findElement(element, "div.ProductTeaser-headingContainer > div > div");
                    var sizePrice = SeleniumExtensions.findElement(element, "div.ProductTeaser-detailsContainer > div.ProductTeaser-details > div:nth-child(2)");

                    //price = price.Replace("kr", "").Replace(",", ".").Trim();
                    //sizePrice = sizePrice.Replace("(", "").Replace(")", "").Replace(",", ".").Trim();

                    result.Add(new ProductViewModel
                    {
                        Title = title == null ? string.Empty : title.GetAttribute("text"),
                        Price = price == null ? string.Empty : price.Text,
                        //Price = decimal.Parse(price),
                        Size = size == null ? string.Empty : size.Text,
                        SizePrice = sizePrice == null ? string.Empty : sizePrice.Text,
                        //SizePrice = decimal.Parse(sizePrice),
                        Company = "Coop",
                    });
                }

            }

            _driver.Quit();

            return result;
        }

        public async Task<List<Category>> GetCategories()
        {
            _driver = _driver ?? new ChromeDriver();

            _driver.Url = "https://handlaprivatkund.ica.se/stores/1003988/categories?source=navigation";
            closeCookieWindow();

            var result = new List<Category>();

            //https://toolsqa.com/selenium-webdriver/c-sharp/findelement-and-findelements-commands-in-c/
            var elements = _driver.FindElements(By.CssSelector("#product-page > div > div._grid-item-12_tilop_45._grid-item-lg-2_tilop_249 > div.sc-1hfavqh-0.bhvoGf > ul > li > a"));
            //var categories = _driver.FindElements(By.CssSelector("#nav-menu > li > a > div > span"));
            if (elements is not null)
            {

                foreach (var element in elements)
                {
                    result.Add(new Category
                    {
                        Name = element.GetAttribute("text")
                        //Title = element.Text
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
                Thread.Sleep(1000);

                var cookieButton = _driver?.FindElement(By.XPath(".//a[contains(@class, 'cmpboxbtn cmpboxbtnyes cmptxt_btn_yes')]"));
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

        //public void Dispose()
        //{
        //    if (_driver is not null)
        //    {
        //        _driver?.Quit();
        //    }
        //}
    }
}
