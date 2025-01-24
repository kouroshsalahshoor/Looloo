using OpenQA.Selenium;

namespace Looloo.BlazorServer.Utilities
{
    public class SeleniumExtensions
    {
        public static IWebElement? findElement(IWebElement element, string cssSelector)
        {
            try
            {
                return element.FindElement(By.CssSelector(cssSelector));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static IWebElement? findElementByXpath(IWebElement element, string selector)
        {
            try
            {
                return element.FindElement(By.XPath(selector));
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
