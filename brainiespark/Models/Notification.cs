using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using brainiespark.Interfaces;

namespace brainiespark.Models
{ 
    public class Notification : IAudit
    {
        public const int MinMessageLength = 50;
        public string AttachmentsToIgnore = "";

        public string EnteredBy { get; set; }
        public DateTime DateTimeEntered { get; set; }

        public string ModifiedBy { get; set; }
        public DateTime DateTimeModified { get; set; }
        public DateTime TimeStamp { get; set; }

        public int Id { get; set; }
        public string ByUserId { get; set; }

        [Display(Name = "Notification Date")]
        public DateTime NotificationDate { get; set; }

        [Display(Name = "Notification For")]
        public string ToUserId { get; set; }

        [Display(Name = "Notification Expiry")]
        public DateTime NotificationExpiry { get; set; }

        [Display(Name = "Active ?")]
        public bool IsActive { get; set; }

        [Display(Name = "Public Notification")]
        public bool IsPublic { get; set; }

        [MinLength(MinMessageLength)]
        [AllowHtml]
        public string Message { get; set; }

        [Display(Name = "Notification Served")]
        public DateTime? NotificationServed { get; set; }

        public IList<Attachment> Attachments { get; set; }

        public Attachment Attachment { get; set; }
    }
}