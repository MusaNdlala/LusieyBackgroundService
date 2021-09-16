using System;
using System.Collections.Generic;
using System.Text;
using LusieyBackgroundService.DataConn;
using LusieyBackgroundService.Models;

namespace LusieyBackgroundService.Service.AudioBusket
{
    public class AudioBustket
    {
        private readonly ApplicationDbContext _applicationDb;
        public AudioBustket(ApplicationDbContext applicationDb)
        {
            _applicationDb = applicationDb;
        }
        public List<AudioBustket> DeactivateAudioBusket()
        {
            return null;
        }
    }
}