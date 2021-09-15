using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LusieyBackgroundService.DataConn
{
    public class DbContextHelper
    {
        private DbContextOptions<ApplicationDbContext> GetOptions()
        {
            var oprtionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            oprtionBuilder.UseMySql("");
            return oprtionBuilder.Options;
        }
        public DbContextOptions<ApplicationDbContext> GetAllOptions() 
        {
            return GetOptions();
        }
    }
}