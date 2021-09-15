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
        private ApplicationDbContext _applicationDb;
        public EmailService(ApplicationDbContext applicationDb)
        {
            _applicationDb = applicationDb;
        }
        private DbContextOptions<ApplicationDbContext> GetOptions()
        {
            var oprtionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            oprtionBuilder.UseMySql("");
            return oprtionBuilder.Options;
        }

        public async Task<List<EmailList>> GetNonSentEmails()
        {
            var result = new List<EmailList>();
            try
            {
                
                result =  await (from Elist in ( _applicationDb.EmailList)
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