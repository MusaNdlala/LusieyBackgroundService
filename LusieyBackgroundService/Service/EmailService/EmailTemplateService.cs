using LusieyBackgroundService.Interface;
using LusieyBackgroundService.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LusieyBackgroundService.Service.EmailService
{
    public class EmailTemplateService: IEmailTemplateService
    {
        private readonly IConfiguration _configuration;

        public EmailTemplateService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public  string SetEmailMessage(string Message/*string Template_Url, EmailMessage email*/)
        {
            try
            {
                //string FilePath = @"C:\Users\Musa\source\repos\SendEmail\SendEmail\html\EmailTEmplate1\Template.html";//Template_Url; 
                string FilePath = @_configuration["LusieyEmailTample1"];//@"C:\Users\Musa\source\repos\SendEmail\SendEmail\html\EmailTEmplate1\Template.html";//Template_Url; 
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