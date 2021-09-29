using System;

namespace LusieyBackgroundService.Models
{
    public sealed class Email : IDisposable
    {
        private string RecievingEmailProp;
        private string subjectProp;
        private string bodyProp;
        private string EmailHeaderProp;
        private string HeaderMessageProp;
        private string MessageProp;

        public Email(string recievingEmail, string subject, string body)
        {
            RecievingEmail = recievingEmail;
            this.subject = subject;
            this.body = body;
        }
        public Email() { }
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
        public string EmailHeader
        {
            get { return this.EmailHeaderProp; }
            set
            {
                if (string.IsNullOrEmpty(value) && string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Email Header can not be empty :", nameof(EmailHeader) + "=>" + value);
                this.EmailHeaderProp = value;
            }
        }
        public string HeaderMessage
        {
            get { return this.HeaderMessageProp; }
            set
            {
                if (string.IsNullOrEmpty(value) && string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Email Message Header can not be empty :", nameof(HeaderMessage) + "=>" + value);
                this.HeaderMessageProp = value;
            }
        }
        public string Message
        {
            get { return this.MessageProp; }
            set
            {
                if (string.IsNullOrEmpty(value) && string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Email Message can not be empty :", nameof(Message) + "=>" + value);
                this.RecievingEmailProp = value;
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}