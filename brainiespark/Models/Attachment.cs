using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using brainiespark.Interfaces;

namespace brainiespark.Models
{
    public class Attachment: IAudit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] AttachmentContent { get; set; }

        public string EnteredBy { get; set; }
        public DateTime DateTimeEntered { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateTimeModified { get; set; }
        public DateTime TimeStamp { get; set; }

        public Notification Notification { get; set; }
    }
}