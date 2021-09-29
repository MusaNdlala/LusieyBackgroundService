using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LusieyBackgroundService.DataConn;
using LusieyBackgroundService.DataConn.DbInterface;

namespace LusieyBackgroundService.Service.AudioBusket
{
    public sealed class AudioBustket : IDisposable
    {
        private readonly IDbConnectHelper _dbConnectHelper;
        public AudioBustket(IDbConnectHelper dbConnectHelper)
        {
            _dbConnectHelper = dbConnectHelper;
        }
        public async Task<List<AudioBustket>> DeactivateAudioBusket()
        {
            using (var _applicationDb = new ApplicationDbContext(await _dbConnectHelper.LusieydbContextOptions())) 
            {
                return null;
            }
        }
        public void Dispose(){GC.SuppressFinalize(this);}
    }
}