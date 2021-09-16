using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using LusieyBackgroundService.Models;
using LusieyBackgroundService.DataConn;
using LusieyBackgroundService.Models.Enums;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LusieyBackgroundService.Interface;
using Microsoft.Extensions.Configuration;

namespace LusieyBackgroundService.Service.Audios
{
    public class AudioService : IAudioService
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        public AudioService(ApplicationDbContext applicationDb, IEmailSender emailSender, IConfiguration configuration)
        {
            _configuration  = configuration;
            _applicationDb  = applicationDb;
            _emailSender    = emailSender;
        }
        public async Task<List<AudioTextModel>> DeactivateAudios()
        {
            try{
                var tempList = await (from temp in _applicationDb.AudioTextModel
                                    where   temp.Active == Active.Active 
                                            && temp.Payment == Payment.unpaid
                                    select temp).ToListAsync();

                if (tempList.Count() == 0)
                    new List<AudioTextModel>();
                
                DateTime now = DateTime.Now;
                var returnresult = new List<AudioTextModel>();

                foreach (var audio in tempList.AsParallel().WithDegreeOfParallelism(2)) {
                    if ((audio.CreationDate > now.AddHours(-24) && audio.CreationDate<=now) == true)
                        returnresult.Add(audio);
                }
                tempList = null;
                return returnresult;
            }
            catch (Exception) {return new List<AudioTextModel>();}
        }
        public async Task<int> EmailAudios(List<AudioTextModel> ToBeEmailed)
        {
            try { 
                if (ToBeEmailed.Count() == 0)
                    return 0;
                var MailMessage = new StringBuilder();
                MailMessage.AppendLine("Good Day \n The Following are Audios That where not Paid for.");

                foreach (var MaileMe in ToBeEmailed)
                {
                    MailMessage.AppendLine("Id      : " + MaileMe.id.ToString());
                    MailMessage.AppendLine("Name    : " + MaileMe.AudioName);
                    MailMessage.AppendLine("Price   : " + MaileMe.priceCharged);
                }

                var email = new Email();
                email.RecievingEmail    = _configuration["AdminEmail:Email"];
                email.HeaderMessage     = _configuration["AdminEmail:Header"];
                email.EmailHeader       = _configuration["AdminEmail:EmailHeader"];
                email.subject           = _configuration["AdminEmail:Subject"];
                email.Message           = MailMessage.ToString();

                if (await _emailSender.SendEmail(email, null, true, null) == true) {
                    return ToBeEmailed.Count();
                }
                return 0;
            }
            catch (Exception) {
                return 0;
            }
        }
    }
}