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

namespace LusieyBackgroundService.Service.Audios
{
    public class AudioService
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IEmailSender _emailSender;

        public AudioService(ApplicationDbContext applicationDb, IEmailSender emailSender)
        {
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
            if(ToBeEmailed.Count() ==0)
                return 0;

            foreach (var MaileMe in ToBeEmailed)
            {
                _emailSender.SendEmail(new Email(),null,true,null);
            }
            return 0;
        }
    }
}