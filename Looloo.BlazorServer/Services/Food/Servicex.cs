using Looloo.BlazorServer.Models;
using Looloo.BlazorServer.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Looloo.BlazorServer.Services.Food
{
    //https://www.toolsqa.com/selenium-c-sharp/
    //https://toolsqa.com/selenium-webdriver/c-sharp/iwebdriver-browser-commands-in-c-sharp/
    //public class IcaService : IDisposable
    public class Servicex
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

            //ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--headless");
            //_driver = new ChromeDriver(options);
            _driver = _driver ?? new ChromeDriver();

            _driver.Navigate().GoToUrl($"https://www.willys.se/sok?q={searchTerm.Trim()}");
            //_driver!.Url = $"https://www.willys.se/sok?q={searchTerm.Trim()}";
            closeCookieWindow();

            //https://www.selenium.dev/documentation/webdriver/waits/
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);

            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
            //wait.Until(d => revealed.Displayed);

            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2))
            //{
            //    PollingInterval = TimeSpan.FromMilliseconds(300),
            //};
            //wait.IgnoreExceptionTypes(typeof(ElementNotInteractableException));

            //wait.Until(d => {
            //    revealed.SendKeys("Displayed");
            //    return true;
            //});

            //https://www.selenium.dev/documentation/webdriver/elements/locators/
            //var elements = _driver.FindElements(By.CssSelector("#__next > div > div.sc-504002c3-3.ePiOKs > main > section > div > div.infinite-scroll-component__outerdiv > div > div > div"));
            var elements = _driver.FindElements(By.XPath("//*[@id=\"__next\"]/div/div[4]/main/section/div/div[4]/div/div[1]/div"));
            //var elements = _driver.FindElements(By.CssSelector("div.ProductTeaser-info"));
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
                        ImageUrl = img == null ? string.Empty : img.GetAttribute("src")
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
                //Thread.Sleep(1000);
                // Maximize the current window
                //_driver?.Manage().Window.Maximize();

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

        //public void Dispose()
        //{
        //    if (_driver is not null)
        //    {
        //        _driver?.Quit();
        //    }
        //}
    }
}
