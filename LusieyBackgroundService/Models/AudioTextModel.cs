using System;
using LusieyBackgroundService.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LusieyBackgroundService.Models
{
    public class AudioTextModel
    {
        [NotMapped]
        public string EncryptID { get; set; }
        [Key]
        public int id { get; set; }
        [Required]
        public Guid publicId { get; set; }
        [Required]
        public string AudioName { get; set; }
        [Required]
        public string AudioUrl { get; set; }
        public string TextUrl { get; set; }
        public string DownloadUrl { get; set; }
        //[Required]
        //public ApplicationUserModel CustomerUser { get; set; }
        public string TransribeUser { get; set; }//foreignkey
        public DateTime transcribtionDate { get; set; }
        public string MarkingUser { get; set; }//foreignkey
        public DateTime MarkingDate { get; set; }
        //[Required]
        //public SubjectType SubjectType { get; set; }
        public Active Active { get; set; } = Active.Active;
        //public IEnumerable<AudioTextDetails> AudioTextDetails { get; set; }
        public decimal priceCharged { get; set; }
        public decimal freelancePay { get; set; }
        public decimal MarkingPay { get; set; }
        //public AffiliatePayout AffiliatePayout { get; set; }
        public double Audiolenght { get; set; }
        public int NumberOfSpeakers { get; set; }
        public int UnClaims { get; set; }
        //public ComplitionHours ComplitionHours { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime PaymentDate { get; set; }
        public Payment Payment { get; set; } = Payment.unpaid;
        //public Claimed Claimed { get; set; } = Claimed.UnClaimed;
        public bool addedToBucket { get; set; } = false;
        public AudioBusket AudioBusket { get; set; }
        public string AffiliatCode { get; set; }
        public decimal AffiliatePayment { get; set; }
    }
}