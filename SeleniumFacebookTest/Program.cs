using System;
using System.IO;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumFacebookTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var driver = new ChromeDriver();
            var js = (IJavaScriptExecutor)driver;
            
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
            driver.Url = "https://www.facebook.com/";
            var startTime = DateTime.Now;
            
            var emailEl = driver.FindElement(By.Id("email"));
            var passEl = driver.FindElement(By.Id("pass"));
            var submitLoginEl = driver.FindElement(By.Id("u_0_b"));
            emailEl.SendKeys("0522811478");
            passEl.SendKeys("Rongcon@3");
            submitLoginEl.Click();
            while (true) 
            {
                Thread.Sleep(2000);
                var pageSourceLower = driver.PageSource.ToLower();
                if (pageSourceLower.Contains("phạm thị"))
                {
                    System.Console.WriteLine("ok");
                    js.ExecuteScript("console.log('test test test')");
                }
                else 
                {
                    System.Console.WriteLine("no");
                }
            }
        }
    }
}
