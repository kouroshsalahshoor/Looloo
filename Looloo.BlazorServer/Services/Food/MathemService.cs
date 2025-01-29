using Looloo.BlazorServer.Models;
using Looloo.BlazorServer.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Looloo.BlazorServer.Services.Food
{
    public class MathemService
    {
        private ChromeDriver? _driver;

        public async Task<List<ProductModel>> Search(string searchTerm)
        {
            var result = new List<ProductModel>();

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            _driver = new ChromeDriver(options);
            //_driver = _driver ?? new ChromeDriver();

            var searchUrl = $"https://www.mathem.se/se/search/products/?q={searchTerm.Trim()}";
            _driver.Navigate().GoToUrl(searchUrl);

            closeCookieWindow();

            var elements = _driver.FindElements(By.XPath("//*[@id=\"main-content\"]/div/div/div/div[3]/div"));
            if (elements is not null)
            {

                foreach (var element in elements)
                {
                    var title = SeleniumExtensions.findElementByCss(element, "div.k-flex.k-flex--gap-0.k-flex--direction-column > a > h2");
                    var price = SeleniumExtensions.findElementByCss(element, "div.k-flex.k-align-items-flex-start.k-flex--gap-0.k-flex--direction-column > span");
                    var size = SeleniumExtensions.findElementByCss(element, "div.k-flex.k-flex--gap-0.k-flex--direction-column > span");
                    var sizePrice = SeleniumExtensions.findElementByCss(element, "div.k-flex.k-align-items-flex-start.k-flex--gap-0.k-flex--direction-column > p");
                    var img = SeleniumExtensions.findElementByCss(element, "div.k-aspect-ratio.k-aspect-ratio--1-1.styles_ProductTileImageBox__5rE_H.k-border-radius-small.k-position-relative > img");

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
                        Company = "Mathem",
                        ImageUrl = img == null ? string.Empty : img.GetAttribute("src"),
                        SearchUrl = searchUrl
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
                // Maximize the current window
                _driver?.Manage().Window.Maximize();

                Thread.Sleep(1000);

                var cookieButton = _driver?.FindElement(By.XPath("//*[@id=\"__next\"]/div[1]/div/div[3]/div/div/button[1]"));
                if (cookieButton is not null)
                {
                    cookieButton.Click();
                }
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
