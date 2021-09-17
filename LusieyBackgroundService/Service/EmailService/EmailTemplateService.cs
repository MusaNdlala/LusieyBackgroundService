using System;
using System.IO;
using LusieyBackgroundService.Interface;

namespace LusieyBackgroundService.Service.EmailService
{
    public sealed class EmailTemplateService: IEmailTemplateService, IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public  string SetEmailMessage(string Message/*string Template_Url, EmailMessage email*/)
        {
            try
            {
                string FilePath = @"C:\Users\Musa\source\repos\SendEmail\SendEmail\html\EmailTEmplate1\Template.html";//Template_Url; 
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();

                string MyHeader = "Lusiey"; //"M-Ndlala";
                string MessageHeader = "Welcome to the message";
                string MyMessage = "Hi this is the message";// Message;// "Hi this is the message";

                MailText = MailText.Replace("[MyHeader]", MyHeader);
                MailText = MailText.Replace("[MessageHeader]", MessageHeader);
                MailText = MailText.Replace("[Message]", MyMessage);

                return MailText;
            }
            catch (Exception) { return ""; }
        }
    }
}