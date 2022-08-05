using AventStack.ExtentReports;
using DataDrivenProject.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDrivenProject.TestCases
{
    [TestFixture]
    [Parallelizable]
    public class LoginTest : BaseTest
    {
        static string testCaseName = "LoginTest";
        static ExcelReaderFile xls = new ExcelReaderFile(ConfigurationManager.AppSettings["xlsPath"]);

        [Test, TestCaseSource("getData")]
        public void doLogin(Dictionary<string, string> data)
        {

            rep = ExtentManager.getInstance();
            test = rep.CreateTest("LoginTest", "This test will describe my LoginTest");
            if (!DataUtil.isTestRunnable(testCaseName, xls) || data["Runmode"].Equals("Y"))
            {
                test.Log(Status.Skip, "Skipping the test as runmode is No");
                Assert.Ignore("Skipping the test as runmode is No");
            }
            openBrowser(data["Browser"]);
            navigate("appurl");
            bool actualResult = doLogin(data["Username"], data["Password"]);

            bool expectedResult = false;
            if (data["ExpectedResult"].Equals("Y"))
                expectedResult = true;
            else
                expectedResult = false;

            if (expectedResult != actualResult)
                reportFailure("Test Failed");
            else
                reportPass("Login Test Passed");

        }

        //Data Source
        public static object[] getData()
        {
            //reads data for only testCaseName
            return DataUtil.getTestData(xls, testCaseName);
        }

        [TearDown]
        public void quit()
        {
            if (rep != null)
                rep.Flush();
            if (driver != null)
                driver.Quit();

        }
    }
}
