using LusieyBackgroundService.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LusieyBackgroundService.Interface
{
    public interface IAudioService
    {
        public Task<List<AudioTextModel>> DeactivateAudios();
        public Task<int> EmailAudios(List<AudioTextModel> ToBeEmailed);
    }
}