using LusieyBackgroundService.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LusieyBackgroundService.Models
{
    public class AudioBusket
    {
        [NotMapped]
        public string EncryptID { get; set; }
        [Key]
        public int id { get; set; }
        public Guid paypalId { get; set; } = new Guid();
        public string AudioRef { get; set; }
        public Active Active { get; set; }
        public Payment paid { get; set; } = Payment.unpaid;
        //public ApplicationUserModel ApplicationUserModel { get; set; }
        public IEnumerable<AudioTextModel> AudioTextModelList { get; set; } = new List<AudioTextModel>();
        public DateTime deletiontime { get; set; }
        public DateTime ArchiveDate { get; set; }
    }
}