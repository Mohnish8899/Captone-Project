using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ABCHealthcareHealthcareTesting.StepDefinitions
{
    [Binding]
    public class LoginStepDefinitions
    {
        private ChromeDriver chromeDriver;

        public LoginStepDefinitions() => chromeDriver = new ChromeDriver("C:\\Users\\mohsin.baviskar\\Downloads\\chromedriver_win32");
    
        [Given(@"I log in")]
        public void GivenILogIn()
        {
            chromeDriver.Navigate().GoToUrl("http://localhost:3000/login");
            WebDriverWait wait = new WebDriverWait(chromeDriver, TimeSpan.FromMilliseconds(1500));
            var user = chromeDriver.FindElement(By.XPath("//*[@name='username']"));
            user.SendKeys("Mohnish");
        }

        [Then(@"I enter password")]
        public void ThenIEnterPassword()
        {
            WebDriverWait wait = new WebDriverWait(chromeDriver, TimeSpan.FromMilliseconds(1500));
            var password = chromeDriver.FindElement(By.XPath("//*[@name='password']"));
            password.SendKeys("Abc@123");

            WebDriverWait wait1 = new WebDriverWait(chromeDriver, TimeSpan.FromMilliseconds(1500));
            var SignInButton = chromeDriver.FindElement(By.XPath("/html/body/div/div/div/form/button"));
            chromeDriver.ExecuteScript("arguments[0].click();", SignInButton);
        }
    }
}
