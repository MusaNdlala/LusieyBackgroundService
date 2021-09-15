using System;
using System.ComponentModel.DataAnnotations;

namespace LusieyBackgroundService.Models
{
    public class EmailList
    {
        [Key]
        public int id { get; set; }
        public string EmailAddress { get; set; }
        public string EmailSubject { get; set; }
        public string EmailMessage { get; set; }
        public Boolean EmailSent { get; set; } = false;
        public DateTime DeletionDate { get; set; } = DateTime.UtcNow.AddDays(1);
    }
}
