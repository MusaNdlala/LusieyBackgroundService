using LusieyBackgroundService.Interface;
using LusieyBackgroundService.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace LusieyBackgroundService.Service.EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailTemplateService _EmailTemplateService;
        public EmailSender(IConfiguration configuration, IEmailTemplateService EmailTemplateService)
        {
            _configuration = configuration;
            _EmailTemplateService = EmailTemplateService;
        }
        public async Task<bool> SendEmail(Email Email, string TemplateType, bool sslOn_Of = true , Attached attached = null)
        {
            try {
                var sendEmail = new EmailDetails();
                sendEmail.SendingEmail  = _configuration["EmailSettings:email"];
                sendEmail.password      = _configuration["EmailSettings:password"];
                sendEmail.portParam     = Convert.ToInt32(_configuration["EmailSettings:Port"]);
                sendEmail.smtpParam     = _configuration["EmailSettings:Smtp"];
                sendEmail.RecievingEmail = Email.RecievingEmail;
                sendEmail.subject       = Email.subject;
                sendEmail.body          = Email.body;

                var smtpClient = new SmtpClient(sendEmail.smtpParam)
                {
                    Port = sendEmail.portParam,
                    Credentials = new NetworkCredential(sendEmail.SendingEmail, sendEmail.password),
                    EnableSsl = sslOn_Of,
                };
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(sendEmail.SendingEmail),
                    Subject = sendEmail.subject,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(sendEmail.RecievingEmail);
                var pathurl = @"C:\Users\Musa\source\repos\SendEmail\SendEmail\html\EmailTEmplate1\images\email.png";

                sendEmail.body = SetEmailMessage(sendEmail.body);
                mailMessage = EmbedPictures(mailMessage, pathurl, sendEmail.body);

                if (mailMessage == null)
                    return false;

                if (attached != null)
                {
                    if (attached.MediaType == null)
                        attached.MediaType = "";

                    var attachment = new Attachment(attached.url, attached.MediaType);
                    mailMessage.Attachments.Add(attachment);
                    smtpClient.Send(mailMessage);
                    return true;
                }
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception) { return false; }
        }
        private MailMessage EmbedPictures(MailMessage temp, string imageUrl, string htmlBody)
        {
            try
            {
                var picturez = new Attached(imageUrl);
                var attachment2 = new Attachment(picturez.url, picturez.MediaType);
                AlternateView alternativeView = GetEmbeddedImage(picturez.url, htmlBody);
                temp.AlternateViews.Add(alternativeView);
                return temp;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private AlternateView GetEmbeddedImage(String filePath, string EmailBody)
        {
            LinkedResource res = new LinkedResource(filePath);
            res.ContentId = Guid.NewGuid().ToString();

            string htmlBody = EmailBody.Replace("[LogoImage]", "cid:" + res.ContentId);

            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(res);
            return alternateView;
        }
        private string SetEmailMessage(string Message/*string Template_Url, EmailMessage email*/)
        {
            try
            {
                string FilePath = _configuration["LusieyEmailTample1"];//@"C:\\Users\Musa\\source\\repos\SendEmail\\SendEmail\\html\\EmailTEmplate1\\Template.html";//Template_Url; 
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
