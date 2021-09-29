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
using LusieyBackgroundService.DataConn.DbInterface;

namespace LusieyBackgroundService.Service.EmailService
{
    public sealed class EmailService : IEmailService, IDisposable
    {
        private readonly IEmailSender _emailSender;
        private readonly IDbConnectHelper _dbConnectHelper;
        public EmailService(IEmailSender emailSender, IDbConnectHelper dbConnectHelper)
        {
            _dbConnectHelper = dbConnectHelper;
            _emailSender    = emailSender;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<List<EmailList>> GetNonSentEmails()
        {
            var result = new List<EmailList>();
            var _applicationDb = new ApplicationDbContext(await _dbConnectHelper.LusieydbContextOptions());
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
                _applicationDb.Dispose();
            }
        }
        public async Task<string> SetEmailsToSent(List<EmailList> emailLists)
        {
            var _applicationDb = new ApplicationDbContext(await _dbConnectHelper.LusieydbContextOptions());
            try
            {
                foreach (var email in emailLists.AsParallel().WithDegreeOfParallelism(2))
                {
                    var temp = email.EmailSent;
                    Attached athd = new Attached(@"C:\files\temp.txt");
                    var email2 = new Email(email.EmailAddress,email.EmailSubject,email.EmailMessage);

                    if (await _emailSender.SendEmail(email2,null, true, athd) == false)
                        return "Not Complete";
                    email.EmailSent = true;
                }
                _applicationDb.UpdateRange(emailLists);
                await _applicationDb.SaveChangesAsync();
                return "Complete";
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {   
                _applicationDb.Dispose();
            }
        }
    }
}