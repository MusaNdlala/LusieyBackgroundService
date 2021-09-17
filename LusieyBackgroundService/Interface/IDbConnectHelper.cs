using LusieyBackgroundService.DataConn;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LusieyBackgroundService.Interface
{
    public interface IDbConnectHelper
    {
        public Task<DbContextOptions<ApplicationDbContext>> LusieydbContextOptions();
    }
}