using Looloo.BlazorServer.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Looloo.BlazorServer.Services.Food
{
    public class IcaService
    {
        private ChromeDriver? _driver;

        public async Task<List<ProductModel>> Search(string searchTerm)
        {
            var result = new List<ProductModel>();

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            _driver = new ChromeDriver(options);
            //_driver = _driver ?? new ChromeDriver();

            var searchUrl = $"https://handlaprivatkund.ica.se/stores/1003988/search?q={searchTerm.Trim()}";
            _driver.Navigate().GoToUrl(searchUrl);
            closeCookieWindow();

            var elements = _driver.FindElements(By.XPath("//*[@id=\"product-page\"]/div/div[2]/div/div/div"));
            if (elements is not null)
            {
                foreach (var element in elements)
                {
                    var title = SeleniumExtensions.findElementByCss(element, "div.title-container > a");
                    var price = SeleniumExtensions.findElementByCss(element, "span._text_16wi0_1._text--m_16wi0_23.sc-1fkdssq-0.bwsVzh");
                    var size = SeleniumExtensions.findElementByCss(element, "span._text_16wi0_1._text--m_16wi0_23.sc-1sjeki5-0.asqfi");
                    var sizePrice = SeleniumExtensions.findElementByCss(element, "span._text_16wi0_1._text--m_16wi0_23.sc-1vpsrpe-2.sc-bnzhts-0.jixyGX.kUYwXM");
                    var imageUrl = SeleniumExtensions.findElementByCss(element, "div.header-container > a > div > img");

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
                            ImageUrl = imageUrl == null ? string.Empty : imageUrl.GetAttribute("src"),
                            SearchUrl = searchUrl
                        });
                    }
                }

            }

            _driver.Quit();

            return result;
        }

        private void closeCookieWindow()
        {
            try
            {
                //Maximize the current window
                _driver?.Manage().Window.Maximize();

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
    }
}
