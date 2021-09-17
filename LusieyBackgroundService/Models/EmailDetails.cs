using System;
using System.Collections.Generic;
using System.Text;

namespace LusieyBackgroundService.Models
{
    public sealed class EmailDetails : IDisposable
    {
        private string SendingEmailProp;
        private string passwordProp;
        private int portParamProp;
        private string smtpParamProp;
        private string RecievingEmailProp;
        private string subjectProp;
        private string bodyProp;
        public string SendingEmail 
        {
            get { return SendingEmailProp; }
            set
            {
                if (value.Contains("@") && value.Contains("."))
                    SendingEmailProp = value;
                else
                    throw new ArgumentException("This is not a correct Email address :", nameof(SendingEmail) + "=>" + value);
            }
        }
        public string password
        {
            get { return this.passwordProp; }
            set
            {
                if (string.IsNullOrEmpty(value) && string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Password is required :", nameof(password) + "=>" + value);
                this.passwordProp = value;
            }
        }
        public int portParam
        {
            get { return this.portParamProp; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Port Number must be higher than 0. If your not sure try 587:", nameof(portParam) + "=>" + value.ToString());
                this.portParamProp = value;
            }
        }
        public string smtpParam
        {
            get { return this.smtpParamProp; }
            set
            {
                if (value.Contains(".") == false)
                    throw new ArgumentException("The smtp signature is not correct :", nameof(smtpParam) + "=>" + value);
                this.smtpParamProp = value;
            }
        }
        public string RecievingEmail
        {
            get { return this.RecievingEmailProp; }
            set
            {
                if (value.Contains("@") == false && value.Contains(".") == false)
                    throw new ArgumentException("This is not a correct Email address :", nameof(RecievingEmail) + "=>" + value);
                this.RecievingEmailProp = value;
            }
        }
        public string subject
        {
            get { return this.subjectProp; }
            set
            {
                if (string.IsNullOrEmpty(value) && string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Subject can not be empty :", nameof(subject) + "=>" + value);
                this.subjectProp = value;
            }
        }
        public string body
        {
            get { return this.bodyProp; }
            set
            {
                if (string.IsNullOrEmpty(value) && string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Body can not be empty :", nameof(body) + "=>" + value);
                this.bodyProp = value;
            }
        }

        public EmailDetails(){}

        public EmailDetails(string sendingEmail, string password, int portParam, string smtpParam, string recievingEmail, string subject, string body)
        {
            SendingEmail = sendingEmail;
            this.password = password;
            this.portParam = portParam;
            this.smtpParam = smtpParam;
            RecievingEmail = recievingEmail;
            this.subject = subject;
            this.body = body;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}