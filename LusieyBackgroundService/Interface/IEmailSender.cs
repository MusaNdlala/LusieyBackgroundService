﻿using LusieyBackgroundService.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LusieyBackgroundService.Interface
{
    


    public interface IEmailSender
    {
        public Task<bool> SendEmail(EmailDetails email);
    }
}
