using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using brainiespark.Interfaces;

namespace brainiespark.Models
{ 
    public class Notification : IAudit
    {
        public delegate byte[] SaveFileHandler(HttpPostedFileBase file, string serverPath);

        public Notification()
        {

        }

        #region "Init"

        public const int MinMessageLength = 50;

        #endregion

        #region "DB Fields"

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

        #endregion


        #region "Methods"

        public void SetNotificationServedDate(ApplicationDbContext dataSource, bool set = true)
        {
            var notificationInDbNotification = dataSource.Notifications.Single(n => (n.Id == Id &&
                                                                                              n.ToUserId == ToUserId));
            if (notificationInDbNotification != null)
            {
                try
                {
                    if (set)
                    {
                        notificationInDbNotification
                            .NotificationServed = DateTime.Now;
                    }
                    else
                    {
                        notificationInDbNotification
                            .NotificationServed = null;
                    }

                    notificationInDbNotification.Message =
                        notificationInDbNotification.Message.PadRight(
                            Notification.MinMessageLength);

                    using (var db = new ApplicationDbContext())
                    {
                        db.Notifications.Attach(this);
                        db.Entry(this).Property(p => p.NotificationServed).IsModified = true;
                        db.SaveChanges();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void Create(ApplicationDbContext dataSource,
            string serverPath,
            IEnumerable<HttpPostedFileBase> files, 
            string[] filesToIgnore,
            string createdById,
            SaveFileHandler saveFile)
        {
            NotificationDate = DateTime.Now;
            ByUserId = createdById;
            DateTimeEntered = DateTime.Now;
            TimeStamp = DateTime.Now;
            DateTimeModified = DateTime.Now;

            foreach (var file in files)
            {
                if (file == null)
                    continue;

                var singleOrDefault = filesToIgnore.SingleOrDefault(f => f.EndsWith(file.FileName) == true);
                if (!string.IsNullOrEmpty(singleOrDefault))
                    continue;

                if (Attachments == null)
                    Attachments = new List<Attachment>();

                byte[] binData = saveFile(file, serverPath);

                Attachment attachment = new Attachment
                {
                    Name = file.FileName,
                    DateTimeEntered = DateTime.Now,
                    EnteredBy = this.EnteredBy,
                    TimeStamp = DateTime.Now,
                    AttachmentContent = binData,
                    DateTimeModified = DateTime.Now,
                    ModifiedBy = this.EnteredBy
                };

                this.Attachments.Add(attachment);

            }

            dataSource.Notifications.Add(this);
            dataSource.SaveChanges();
        }

        #endregion
    }
}