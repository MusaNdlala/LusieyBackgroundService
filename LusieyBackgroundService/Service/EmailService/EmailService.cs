using LusieyBackgroundService.DataConn;
using LusieyBackgroundService.Models;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LusieyBackgroundService.Interface;

namespace LusieyBackgroundService.Service.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IEmailSender _emailSender;
        public EmailService(ApplicationDbContext applicationDb, IEmailSender emailSender)
        {
            _applicationDb  = applicationDb;
            _emailSender    = emailSender;
        }
        public async Task<List<EmailList>> GetNonSentEmails()
        {
            var result = new List<EmailList>();
            try
            {   
                result = await (from Elist in (_applicationDb.EmailList)
                                where Elist.EmailSent == false
                                select Elist).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return new List<EmailList>();
            }
            finally
            {
                result = null;
            }
        }
        public async Task<string> SetEmailsToSent(List<EmailList> emailLists) 
        {
            try
            {
                foreach (var email in emailLists.AsParallel().WithDegreeOfParallelism(2))
                {
                    var temp = email.EmailSent;
                    Attached athd = new Attached(@"C:\files\temp.txt");

                    if (await _emailSender.SendEmail(email, true, athd) == false)
                        return "Not Complete";
                    
                    email.EmailSent = true;
                    email.EmailSent = temp;
                }
                _applicationDb.UpdateRange(emailLists);
                await _applicationDb.SaveChangesAsync();
                return "Complete";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}