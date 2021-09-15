using LusieyBackgroundService.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LusieyBackgroundService.Interface
{
    public interface IEmailService
    {
        public Task<List<EmailList>> GetNonSentEmails();
        public Task<string> SetEmailsToSent(List<EmailList> emailLists);
    }
}
