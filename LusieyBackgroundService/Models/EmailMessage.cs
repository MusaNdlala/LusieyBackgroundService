using System;
using System.Collections.Generic;
using System.Text;

namespace LusieyBackgroundService.Models
{
    public class EmailMessage
    {
        public string EmailHeader { get; set; }
        public string MessageHeader { get; set; }
        public string Message { get; set; }
    }
}