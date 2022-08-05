using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDrivenProject
{
    public class ExtentManager
    {
        public static ExtentHtmlReporter htmlReporter;

        private static ExtentReports extent;

        private ExtentManager()
        {

        }

        public static ExtentReports getInstance()
        {
            if (extent == null)
            {
                string reportFile = DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_") + ".html";
                htmlReporter = new ExtentHtmlReporter(@"C:\Users\amand\source\repos\CSharpcode\DataDrivenProject\DataDrivenProject\Reports" + reportFile);
                extent = new ExtentReports();
                extent.AttachReporter(htmlReporter);
                extent.AddSystemInfo("OS", "Windows");
                extent.AddSystemInfo("Host Name", "Anil");
                extent.AddSystemInfo("Environment", "QA");
                extent.AddSystemInfo("UserName", "Anil");

                //string filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
                //filePath = Directory.GetParent(Directory.GetParent(filePath).FullName).FullName + "\\extent-config.xml";
                //filePath = filePath + "\\extent-config.xml";
                //Console.WriteLine(filePath);
                htmlReporter.LoadConfig(ConfigurationManager.AppSettings["extentConfigPath"]);
            }
            return extent;
        }
    }
}
