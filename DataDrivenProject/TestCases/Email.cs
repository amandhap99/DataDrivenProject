using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DataDrivenProject.TestCases
{
    [SetUpFixture]
    public class Email
    {
        [OneTimeTearDown]
        public void sendMail()
        {
            string reportFolder = ConfigurationManager.AppSettings["reportFolder"];

            DirectoryInfo DirInfo = new DirectoryInfo(reportFolder);

            FileInfo[] files = DirInfo.GetFiles();
            DateTime lastWrite = DateTime.MinValue;
            FileInfo lastWritenFile = null;

            foreach (FileInfo file in files)
            {
                if (file.LastWriteTime > lastWrite)
                {
                    lastWrite = file.LastWriteTime;
                    lastWritenFile = file;
                }
            }
            Console.WriteLine(lastWritenFile);

            MailMessage m = new MailMessage();
            SmtpClient sc = new SmtpClient();
            try
            {
                m.From = new MailAddress("anil.mandhapati@yahoo.com", "Anil");
                m.To.Add(new MailAddress("anil.mandhapati@gmail.com", "Anil"));
                //m.To.Add(new MailAddress("anil.mandhapati@gmail.com", "Anil"));
                //m.CC.Add(new MailAddress("anil.mandhapati@gmail.com", "Anil"));
                //similarly BCC
                m.Subject = "Automation Test Reports";
                m.Body = "Please find the reports attached.\n\n Regards\nAnil";

                m.Attachments.Add(new Attachment(reportFolder + lastWritenFile));

                sc.Host = "smtp.gmail.com";
                sc.Port = 587;
                sc.Credentials = new System.Net.NetworkCredential("anil.mandhapati@gmail.com", "pass@1234");
                sc.EnableSsl = true; // runtime encrypt the SMTP communications using SSL
                sc.Send(m);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
