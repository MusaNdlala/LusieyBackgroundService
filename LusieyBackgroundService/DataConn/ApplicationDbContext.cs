using LusieyBackgroundService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LusieyBackgroundService.DataConn
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
               : base(options)
        {
        }
        public DbSet<EmailList> EmailList {get;set;}
        public DbSet<AudioBusket> AudioBuskets {get;set;}
        public DbSet<AudioTextModel> AudioTextModel {get;set;}
    }
}