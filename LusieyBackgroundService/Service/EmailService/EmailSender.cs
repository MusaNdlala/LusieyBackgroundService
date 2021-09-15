using LusieyBackgroundService.Interface;
using LusieyBackgroundService.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace LusieyBackgroundService.Service.EmailService
{
    public class EmailSender : IEmailSender
    {
        private IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> SendEmail(EmailList Email, bool sslOn_Of = true , Attached attached = null)
        {
            try {
                
                var sendEmail = new EmailDetails();
                sendEmail.SendingEmail  = _configuration["EmailSettings:email"];
                sendEmail.password      = _configuration["EmailSettings:password"];
                sendEmail.portParam     = Convert.ToInt32(_configuration["EmailSettings:Port"]);
                sendEmail.smtpParam     = _configuration["EmailSettings:Smtp"];
                sendEmail.RecievingEmail = Email.EmailAddress;
                sendEmail.subject       = Email.EmailSubject;
                sendEmail.body          = Email.EmailMessage;
                

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
                    //Body =  sendEmail.body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(sendEmail.RecievingEmail);

                var pathurl = @"C:\Users\Musa\source\repos\SendEmail\SendEmail\html\EmailTEmplate1\images\email.png";
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
    }
}
