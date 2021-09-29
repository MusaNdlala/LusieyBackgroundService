using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LusieyBackgroundService.DataConn.DbInterface
{
    public interface IDbConnectHelper
    {
        Task<DbContextOptions<ApplicationDbContext>> LusieydbContextOptions();
    }
}