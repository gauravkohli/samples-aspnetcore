using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using System;
using System.IO;
using OpenQA.Selenium.Chrome;	

namespace okta_aspnetcore_mvc_example.Tests.Selenium
{
    [TestClass]
    public class HomePageTests{

    private TestContext testContextInstance;
    private IWebDriver driver;
    private string appURL;

    [TestMethod]
    public void HomeControllerTest_Home(){
      driver.Navigate().GoToUrl(appURL);
      Assert.AreEqual("Welcome", driver.FindElement(By.XPath("//h1")).Text);
      Assert.IsTrue(driver.Title.Contains("okta_aspnetcore_mvc_example"), "Verified title of the page");
    }

   [TestMethod]
    public void HomeControllerTest_Privacy(){
      driver.Navigate().GoToUrl(appURL);
      driver.FindElement(By.LinkText("Privacy")).Click();

      Assert.AreEqual("Privacy Policy", driver.FindElement(By.XPath("//h1")).Text);
      Assert.IsTrue(driver.Title.Contains("Privacy Policy"), "Verified title of the page");
    }

    [TestMethod]
    public void HomeControllerTest_Signin(){
      driver.Navigate().GoToUrl(appURL);
      driver.FindElement(By.LinkText("Sign In")).Click();

      Assert.AreEqual("Sign In", driver.FindElement(By.XPath("//h2")).Text);
      Assert.IsTrue(driver.Title.Contains("Sign In"), "Verified title of the page");
    }

    public TestContext TestContext{
      get{
        return testContextInstance;
      }
      set{
        testContextInstance = value;
      }
    }

    [TestInitialize()]
    public void SetupTest(){
      appURL = TestContext.Properties["webAppUrl"].ToString();

      var chromeOptions = new ChromeOptions();
      chromeOptions.AddArguments("headless");
      driver = new ChromeDriver(chromeOptions);

    }

    [TestCleanup()]
    public void MyTestCleanup(){
      driver.Quit();
    }

    }
}