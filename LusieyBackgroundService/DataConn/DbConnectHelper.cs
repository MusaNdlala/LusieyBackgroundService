﻿using LusieyBackgroundService.DataConn.DbInterface;
using LusieyBackgroundService.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LusieyBackgroundService.DataConn
{
    public sealed class DbConnectHelper : IDbConnectHelper, IDisposable
    {
        private readonly IConfiguration _configuration;
        private DbContextOptionsBuilder<ApplicationDbContext> _options;
        public DbConnectHelper(IConfiguration configuration/*, DbContextOptionsBuilder<ApplicationDbContext> options*/)
        {
            _configuration = configuration;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<DbContextOptions<ApplicationDbContext>> LusieydbContextOptions()
        {
            /*try{
                return _options.UseMySql(_configuration.GetConnectionString("DbConnection")).Options;
            }
            catch (Exception){
                return new DbContextOptions<ApplicationDbContext>();
            }*/
            /*using (_options = new DbContextOptionsBuilder<ApplicationDbContext>())
            {
                return _options.UseMySql(_configuration.GetConnectionString("DbConnection")).Options;
            }*/
            _options = new DbContextOptionsBuilder<ApplicationDbContext>();

            try { return _options.UseMySql(_configuration.GetConnectionString("DbConnection")).Options;}

            catch (Exception) { return new DbContextOptions<ApplicationDbContext>();}

            finally { _options = null; }
        }
    }
}