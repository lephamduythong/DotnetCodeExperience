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
            var options = new ChromeOptions();
            // options.AddArgument("headless");
            var driver = new ChromeDriver(options);
            var js = (IJavaScriptExecutor)driver;
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
            driver.Url = "http://www.phimmoizz.net/phim/phim-doraemon-nobita-va-nhung-ban-khung-long-moi-9271/xem-phim.html";
            var startTime = DateTime.Now;
            
            while (true) 
            {
                Thread.Sleep(2000);
                
            }
        }
    }
}
