using LusieyBackgroundService.DataConn;
using LusieyBackgroundService.Interface;
using LusieyBackgroundService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LusieyBackgroundService.Service.Tester
{
    public class Tester : ITester, IDisposable
    {
        private readonly IDbConnectHelper _dbConnectHelper;

        public Tester(IDbConnectHelper dbConnectHelper)
        {
            _dbConnectHelper = dbConnectHelper;
        }

        public async Task<List<AudioTextModel>> DeactivateAudios()
        { 
            using (var _applicationDbContext = new ApplicationDbContext(await _dbConnectHelper.LusieydbContextOptions()))
            {
                return new List<AudioTextModel>();
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}