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
    public sealed class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private Dictionary<string, object> setUpMailMessage(EmailDetails sendEmail, Email Email, bool sslOn_Of)
        {
            sendEmail.SendingEmail = _configuration["EmailSettings:email"];
            sendEmail.password = _configuration["EmailSettings:password"];
            sendEmail.portParam = Convert.ToInt32(_configuration["EmailSettings:Port"]);
            sendEmail.smtpParam = _configuration["EmailSettings:Smtp"];
            sendEmail.RecievingEmail = Email.RecievingEmail;
            sendEmail.subject = Email.subject;
            sendEmail.body = Email.body;

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
                Body = sendEmail.body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(sendEmail.RecievingEmail);

            var result = new Dictionary<string, object>();

            result.Add("sc", smtpClient);
            result.Add("mm", mailMessage); 
            result.Add("se", sendEmail);

            return result;
        }
        public async Task<bool> SendEmail(Email Email, string TemplateType, bool sslOn_Of = true , Attached attached = null)
        {
            var sendEmail = new EmailDetails();
            try
            {
                var MyDictionary = setUpMailMessage(sendEmail, Email, sslOn_Of);
                var mailMessage = (MailMessage)MyDictionary["mm"];
                var smtpClient = (SmtpClient)MyDictionary["sc"];
                sendEmail = (EmailDetails)MyDictionary["se"];

                sendEmail.body = SetEmailMessage(sendEmail.body);

                var pathurl = @_configuration["TemplateUrl"];//@"C:\Users\Musa\source\repos\LusieyBackgroundService\LusieyBackgroundService\HtmlTemplate\EmailTEmplate1\images\email.png";

                var TempMailMessage = EmbedPictures(mailMessage, pathurl, sendEmail.body);
                if (TempMailMessage != null) {
                    mailMessage = TempMailMessage;
                }

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
            catch (Exception) 
            {   return false; }
            finally{
                //sendEmail.Dispose();
                //attached.Dispose();
                //Email.Dispose();
            }
        }
        private MailMessage EmbedPictures(MailMessage temp, string imageUrl, string htmlBody)
        {
            if (string.IsNullOrEmpty(imageUrl) || string.IsNullOrEmpty(htmlBody) || temp == null)
                return null;
            
            var picturez = new Attached(imageUrl);
            try
            { 
                var attachment2 = new Attachment(picturez.url, picturez.MediaType);
                AlternateView alternativeView = GetEmbeddedImage(picturez.url, htmlBody);
                temp.AlternateViews.Add(alternativeView);
                return temp;
            }
            catch (Exception)
            {
                return null;
            }
            finally { 
                //temp.Dispose();
                //picturez.Dispose();
            }
        }
        private AlternateView GetEmbeddedImage(String filePath, string EmailBody)
        {
            LinkedResource res = new LinkedResource(filePath);
            //AlternateView alternateView = AlternateView.CreateAlternateViewFromString("");
            try{
                res.ContentId = Guid.NewGuid().ToString();
                string htmlBody = EmailBody.Replace("[LogoImage]", "cid:" + res.ContentId);
                AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
                alternateView.LinkedResources.Add(res);
                return alternateView;
            }
            catch (Exception e){
                return null;
            }
        }
        private string SetEmailMessage(string Message)
        {
            try
            {
                string FilePath = _configuration["LusieyEmailTample1"];
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();

                string MyHeader = "Lusiey"; //"M-Ndlala";
                string MessageHeader = _configuration["MessageHeader"];//"Welcome to the message";
                string MyMessage = Message;

                MailText = MailText.Replace("[MyHeader]", MyHeader);
                MailText = MailText.Replace("[MessageHeader]", MessageHeader);
                MailText = MailText.Replace("[Message]", MyMessage);

                return MailText;
            }
            catch (Exception) { return ""; }
        }
    }
}
