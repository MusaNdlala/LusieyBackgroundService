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
using LusieyBackgroundService.DataConn.DbInterface;

namespace LusieyBackgroundService.Service.Audios
{
    public sealed class AudioService : IAudioService, IDisposable
    {
        private readonly IDbConnectHelper _dbConnectHelper;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        public AudioService(IDbConnectHelper dbConnectHelper, IEmailSender emailSender, IConfiguration configuration)
        {
            _dbConnectHelper = dbConnectHelper;
            _configuration  = configuration;
            _emailSender    = emailSender;
        }

        private async Task<List<AudioTextModel>> DeactivateAudios(List<AudioTextModel> audios) 
        {
            if (audios.Count==0)
                return new List<AudioTextModel>();
            
            var _applicationDb = new ApplicationDbContext(await _dbConnectHelper.LusieydbContextOptions());
            try
            {
                _applicationDb.AudioTextModel.UpdateRange(audios);
                await _applicationDb.SaveChangesAsync();
                return audios;
            }
            catch (Exception) {return new List<AudioTextModel>(); }
        }
        public async Task<List<AudioTextModel>> DeactivateAudios()
        {
            return await DeactivateAudios(await GetAudiosToDeactive());
        }

        private async Task<List<AudioTextModel>> GetAudiosToDeactive()
        {
            var _applicationDb = new ApplicationDbContext(await _dbConnectHelper.LusieydbContextOptions());
            try
            {
                var tempList = await (from temp in _applicationDb.AudioTextModel
                                      where temp.Active == Active.Active
                                              && temp.Payment == Payment.unpaid
                                      select temp).ToListAsync();

                if (tempList.Count() == 0)
                    new List<AudioTextModel>();

                DateTime now = DateTime.Now;
                var returnresult = new List<AudioTextModel>();

                foreach (var audio in tempList.AsParallel().WithDegreeOfParallelism(2))
                {
                    if ((audio.CreationDate < now.AddHours(-24) && audio.CreationDate <= now) == true) {
                        audio.Active = Active.NonActive;
                        returnresult.Add(audio);
                    }
                }
                tempList = null;
                return returnresult;
            }
            catch (Exception) { return new List<AudioTextModel>(); }
            finally { await _applicationDb.DisposeAsync(); }
        }

        void IDisposable.Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<int> EmailAudios(List<AudioTextModel> ToBeEmailed)
        {
            if (ToBeEmailed.Count() == 0)
                return 0;
            var MailMessage = new StringBuilder();
            var email = new Email();
            try {
                
                MailMessage.AppendLine("Good Day \n The Following are Audios That where not Paid for within 24hrs.");
                foreach (var MaileMe in ToBeEmailed)
                {
                    MailMessage.AppendLine("Id      : " + MaileMe.id.ToString());
                    MailMessage.AppendLine("Name    : " + MaileMe.AudioName);
                    MailMessage.AppendLine("Price   : " + MaileMe.priceCharged);
                }

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
            finally { 
                MailMessage = null;
                email.Dispose();
            }
        }
    }
}