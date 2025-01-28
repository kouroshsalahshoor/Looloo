using Looloo.BlazorServer.Models;
using Looloo.BlazorServer.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Looloo.BlazorServer.Services
{
    //https://www.toolsqa.com/selenium-c-sharp/
    //https://toolsqa.com/selenium-webdriver/c-sharp/iwebdriver-browser-commands-in-c-sharp/
    //public class IcaService : IDisposable
    public class IcaService
    {
        private ChromeDriver? _driver;
        //public IcaService()
        //{
        //    _driver = new ChromeDriver();

        //    //_driver.Url = "https://handlaprivatkund.ica.se/stores/1003988/categories?source=navigation";
        //    ////string Title = driver.Title;
        //    ////Console.WriteLine("Title of the page is : " + Title);

        //    //////Mazimize current window
        //    ////driver.Manage().Window.Maximize();

        //    //////Delay execution for 5 seconds to view the maximize operation
        //    ////Thread.Sleep(5000);

        //    //closeCookieWindow();

        //    //https://toolsqa.com/selenium-webdriver/c-sharp/webelement-commands-in-c/
        //    //bool staus = driver.FindElement(By.Id("UserName")).Displayed;
        //    //bool staus = driver.FindElement(By.Id("UserName")).Enabled;
        //    //bool staus = driver.FindElement(By.Id("UserName")).Selected;

        //}

        public async Task<List<ProductModel>> Search(string searchTerm)
        {
            var result = new List<ProductModel>();

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            _driver = new ChromeDriver(options);
            //_driver = _driver ?? new ChromeDriver();

            _driver.Navigate().GoToUrl($"https://handlaprivatkund.ica.se/stores/1003988/search?q={searchTerm.Trim()}");
            //_driver.Url = $"https://handlaprivatkund.ica.se/stores/1003988/search?q={searchTerm.Trim()}";
            closeCookieWindow();

            var elements = _driver.FindElements(By.CssSelector("div.footer-container"));
            if (elements is not null)
            {
                foreach (var element in elements)
                {
                    var title = SeleniumExtensions.findElementByCss(element, "div.title-container > a");
                    //var title = element.FindElement(By.CssSelector("div.title-container > a"));
                    //var title = element.FindElement(By.CssSelector("div.title-container > a")).GetAttribute("text");

                    var price = SeleniumExtensions.findElementByCss(element, "span._text_16wi0_1._text--m_16wi0_23.sc-1fkdssq-0.bwsVzh");
                    //var price = element.FindElement(By.CssSelector("span._text_16wi0_1._text--m_16wi0_23.sc-1fkdssq-0.bwsVzh"));

                    var size = SeleniumExtensions.findElementByCss(element, "span._text_16wi0_1._text--m_16wi0_23.sc-1sjeki5-0.asqfi");
                    //var size = element.FindElement(By.CssSelector("span._text_16wi0_1._text--m_16wi0_23.sc-1sjeki5-0.asqfi")).Text;

                    var sizePrice = SeleniumExtensions.findElementByCss(element, "span._text_16wi0_1._text--m_16wi0_23.sc-1vpsrpe-2.sc-bnzhts-0.jixyGX.kUYwXM");
                    //var sizePrice = element.FindElement(By.CssSelector("span._text_16wi0_1._text--m_16wi0_23.sc-1vpsrpe-2.sc-bnzhts-0.jixyGX.kUYwXM")).Text;

                    //price = price.Replace("kr", "").Replace(",", ".").Trim();
                    //sizePrice = sizePrice.Replace("(", "").Replace(")", "").Replace(",", ".").Trim();

                    if (string.IsNullOrEmpty(title?.GetAttribute("text")) == false)
                    {
                        result.Add(new ProductModel
                        {
                            Title = title == null ? string.Empty : title.GetAttribute("text"),
                            Price = price == null ? string.Empty : price.Text.Replace("kr", "").Replace(",", ".").Trim(),
                            //Price = decimal.Parse(price),
                            Size = size == null ? string.Empty : size.Text,
                            SizePrice = sizePrice == null ? string.Empty : sizePrice.Text,
                            //SizePrice = decimal.Parse(sizePrice)
                            Company = "ICA",
                        });
                    }
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
                //Delay execution for 5 seconds to view the maximize operation
                Thread.Sleep(1000);

                var cookieButton = _driver.FindElement(By.XPath(".//*[@id='onetrust-accept-btn-handler']"));
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
