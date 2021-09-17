using System;
using System.Collections.Generic;
using System.Text;

namespace LusieyBackgroundService.Models
{
    public sealed class EmailMessage : IDisposable
    {
        public string EmailHeader { get; set; }
        public string MessageHeader { get; set; }
        public string Message { get; set; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}