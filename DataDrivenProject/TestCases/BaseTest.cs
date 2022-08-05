using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataDrivenProject.TestCases
{
    public class BaseTest
    {
        public IWebDriver driver = null;
        public ExtentReports rep;
        public ExtentTest test;
        public bool gridRun = false;

        public void openBrowser(string bType)
        {
            test.Log(Status.Info, "Opening the browser " + bType);
            if (!gridRun)
            {
                if (bType.Equals("Mozilla"))
                {
                    FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(ConfigurationManager.AppSettings["GeckoDriverPAth"], "geckodriver.exe");
                    service.FirefoxBinaryPath = @"C:\Users\amand\source\repos\CSharpcode\DataDrivenProject\DataDrivenProject\Drivers\geckodriver.exe";

                    FirefoxProfile fp = new FirefoxProfile();
                    fp.SetPreference("dom.webnotifications.enabled", false);
                    FirefoxOptions options = new FirefoxOptions();
                    options.Profile = fp;
                    driver = new FirefoxDriver(service, options, TimeSpan.FromSeconds(20));

                }
                else if (bType.Equals("Chrome"))
                {
                    driver = new ChromeDriver(ConfigurationManager.AppSettings["chromeDrivePath"]);
                }
                else if (bType.Equals("Edge"))
                {
                    driver = new InternetExplorerDriver(ConfigurationManager.AppSettings["MSEdgeDriverPath"]);
                }
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            }

            else
            {
                DesiredCapabilities cap = null;
                if (bType.Equals("Mozilla"))
                {
                    FirefoxProfile fp = new FirefoxProfile();
                    fp.SetPreference("dom.webnotifications.enabled", false);
                    FirefoxOptions options = new FirefoxOptions();
                    options.Profile = fp;
                    cap = DesiredCapabilities.Firefox();

                    cap.SetCapability(CapabilityType.BrowserName, "firefox");
                    cap.SetCapability(CapabilityType.Platform, "WINDOWS");
                    cap = (DesiredCapabilities)options.ToCapabilities();
                }
                else if (bType.Equals("Chrome"))
                {
                    cap = DesiredCapabilities.Chrome();
                    cap.SetCapability(CapabilityType.BrowserName, "chrome");
                    cap.SetCapability(CapabilityType.Platform, "WINDOWS");
                }
                driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), cap);

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            }

            test.Log(Status.Info, "Browser successfully opened : " + bType);
        }

        public void navigate(string urlKey)
        {
            test.Log(Status.Info, "Navigating to " + ConfigurationManager.AppSettings[urlKey]);
            driver.Url = ConfigurationManager.AppSettings[urlKey];
        }

        public void click(string xpathExpKey)
        {
            test.Log(Status.Info, "Clicking on " + xpathExpKey);
            getElement(xpathExpKey).Click();
            test.Log(Status.Info, "Clicked successfully on " + xpathExpKey);


        }

        public void type(string xpathExpKey, string data)
        {
            test.Log(Status.Info, "Tying in " + xpathExpKey + "-Data :" + data);
            getElement(xpathExpKey).SendKeys(data);
            test.Log(Status.Info, "Typed successfully in " + xpathExpKey);

        }

        public IWebElement getElement(string locatorKey)
        {
            //finding webelement and returing it
            IWebElement e = null;
            try
            {
                if (locatorKey.EndsWith("_Xpath"))
                {
                    e = driver.FindElement(By.XPath(ConfigurationManager.AppSettings[locatorKey]));
                }
                else if (locatorKey.EndsWith("_id"))
                {
                    e = driver.FindElement(By.Id(ConfigurationManager.AppSettings[locatorKey]));
                }
                else if (locatorKey.EndsWith("_name"))
                {
                    e = driver.FindElement(By.Name(ConfigurationManager.AppSettings[locatorKey]));
                }
                else
                {
                    reportFailure("Locator not correct " + locatorKey);
                    Assert.Fail("Locator not correct " + locatorKey);
                }
            }
            catch (Exception ex)
            {
                //fail the test and report the error
                reportFailure(ex.Message);
                Assert.Fail("Fail the test - " + ex.Message);
            }
            return e;

        }

        public void clickAndWait(string locator_clicked, string locator_pressed)
        {
            test.Log(Status.Info, "Clicking on - " + locator_clicked + " and waiting for -" + locator_pressed);
            int count = 5;
            for (int i = 0; i < count; i++)
            {
                getElement(locator_clicked).Click();
                Thread.Sleep(5000);
                isElementPresent(locator_pressed);
                break;
            }
        }

        public void clickOnLead(string leadName)
        {
            int rNum = getLeadRowNum(leadName);
            driver.FindElement(By.XPath(ConfigurationManager.AppSettings["leadPart1_Xpath"] + rNum + ConfigurationManager.AppSettings["leadPart2_Xpath"])).Click();
            //table[@id='listViewTable']/tbody/tr[3]/td[4]
        }

        public void wait(int timeToWaitInSec)
        {
            try
            {
                Thread.Sleep(timeToWaitInSec * 1000);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /******************* Validation functions********************/

        public bool verifyTitle()
        {
            return false;
        }

        public bool isElementPresent(string locatorKey)
        {
            IList<IWebElement> elementList = null;
            if (locatorKey.EndsWith("_Xpath"))
            {
                elementList = driver.FindElements(By.XPath(ConfigurationManager.AppSettings[locatorKey]));
            }
            else if (locatorKey.EndsWith("_id"))
            {
                elementList = driver.FindElements(By.Id(ConfigurationManager.AppSettings[locatorKey]));
            }
            else if (locatorKey.EndsWith("_name"))
            {
                elementList = driver.FindElements(By.Name(ConfigurationManager.AppSettings[locatorKey]));
            }
            else
            {
                reportFailure("Locator not correct " + locatorKey);
                Assert.Fail("Locator not correct " + locatorKey);
            }
            if (elementList.Count == 0)
                return false;
            else
                return true;
        }

        public bool verifyText(string locatorKey, string expectedTextKey)
        {
            string actualText = getElement(locatorKey).Text;
            string expectedText = expectedTextKey;
            if (actualText.Equals(expectedText))
                return true;
            else
                return false;
        }

        /************** Reporting functions********************/

        public void reportPass(string msg)
        {
            test.Log(Status.Pass, msg);
        }

        public void reportFailure(string msg)
        {
            test.Log(Status.Fail, msg);
            takeScreenshot();
            Assert.Fail(msg);
        }

        public void takeScreenshot()
        {
            //filename of the screenshot
            string screenshotFile = DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_") + ".png";
            ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
            Screenshot screenshot = screenshotDriver.GetScreenshot();
            string filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            filePath = Directory.GetParent(Directory.GetParent(filePath).FullName).FullName;

            string screenshotPath = filePath + "\\Screenshots\\" + screenshotFile;

            screenshot.SaveAsFile(filePath + "\\Screenshots\\" + screenshotFile, ScreenshotImageFormat.Png);
            test.Log(Status.Info, "Screenshot - ", MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
            //test.AddScreenCaptureFromPath(screenshotPath);
        }

        /************* Application Functions********************/

        public bool doLogin(string username, string password)
        {
            test.Log(Status.Info, "Trying to login with " + username + " ," + password);
            click("login_Xpath");
            type("username_Xpath", username);
            type("password_Xpath", password);
            click("signInButton_Xpath");
            if (isElementPresent("crmLink_Xpath"))
            {
                test.Log(Status.Info, "Login Success");
                return true;
            }
            else
            {
                test.Log(Status.Info, "Login Failed");
                return false;
            }
        }

        public int getLeadRowNum(string leadName)
        {
            test.Log(Status.Info, "Finding the lead -" + leadName);
            IList<IWebElement> leadNames = driver.FindElements(By.XPath(ConfigurationManager.AppSettings["leadNamesCol_Xpath"]));
            for (int i = 0; i < leadNames.Count; i++)
            {
                Console.WriteLine(leadNames[i].Text);
                if (leadNames[i].Text.Trim().Equals(leadName))
                {
                    test.Log(Status.Info, "Lead found in rowNo. - " + (i + 1));
                    return (i + 1);
                }
            }
            test.Log(Status.Info, "Lead not found");
            return -1;
        }

        public int getAccountRowNum(string leadCompany)
        {
            test.Log(Status.Info, "Finding the account -" + leadCompany);
            IList<IWebElement> accountNames = driver.FindElements(By.XPath(ConfigurationManager.AppSettings["accountNamesCol_Xpath"]));
            for (int i = 0; i < accountNames.Count; i++)
            {
                Console.WriteLine(accountNames[i].Text);
                if (accountNames[i].Text.Trim().Equals(leadCompany))
                {
                    test.Log(Status.Info, "Account found in rowNo. : " + (i + 1));
                    return (i + 1);
                }
            }
            test.Log(Status.Info, "Account not found");
            return -1;

        }

        public int getPotentialRowNum(string potentialName)
        {
            test.Log(Status.Info, "Finding the potential -" + potentialName);
            IList<IWebElement> potentialNames = driver.FindElements(By.XPath(ConfigurationManager.AppSettings["potentialNameCol_Xpath"]));
            for (int i = 0; i < potentialNames.Count; i++)
            {
                Console.WriteLine(potentialNames[i].Text);
                if (potentialNames[i].Text.Trim().Equals(potentialName))
                {
                    test.Log(Status.Info, "Potential found in rowNo. : " + (i + 1));
                    return (i + 1);
                }
            }
            test.Log(Status.Info, "Potential not found");
            return -1;

        }

        public void selectDate(string date)
        {
            //logic
            //convert the date to DateTime
            test.Log(Status.Info, "Selecting the date" + date);

            DateTime dateToBeSelected = Convert.ToDateTime(date);
            DateTime currentDate = DateTime.Now;

            string monthToBeSelected = dateToBeSelected.ToString("MMMM");
            Console.WriteLine(monthToBeSelected);

            string yearToBeSelected = dateToBeSelected.ToString("yyyy");
            Console.WriteLine(yearToBeSelected);

            string dayToBeSelected = dateToBeSelected.ToString("dd");
            Console.WriteLine(dayToBeSelected);

            string monthYearToBeSelected = monthToBeSelected + " " + yearToBeSelected;

            while (true)
            {
                if (currentDate.CompareTo(dateToBeSelected) == 1)
                {
                    //back btn
                    click("backBtn_Xpath");
                }
                else if (currentDate.CompareTo(dateToBeSelected) == -1)
                {
                    //front btn
                    click("frontBtn_Xpath");
                }
                if (monthYearToBeSelected.Equals(getText("monthYearDisplayed_Xpath")))
                {
                    break;
                }
            }
            driver.FindElement(By.XPath("//td[text()='" + dayToBeSelected + "']")).Click();
            test.Log(Status.Info, "Date selection successful");
        }

        public string getText(string locatorKey)
        {
            test.Log(Status.Info, "Getting the text from " + locatorKey);
            return getElement(locatorKey).Text;
        }

    }
}
