using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using brainiespark.Helpers;
using brainiespark.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace brainiespark.Controllers
{
    public class MyWallController : Controller
    {
        public ApplicationDbContext Context { get; }
        private readonly Action<string> _messageAction;
 

        public MyWallController()
        {
            Context = new ApplicationDbContext();
            _messageAction += MessageAction;
        }

        // GET: MyWall
        /// <summary>
        /// Entry point for MyWall
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account", new {returnUrl = ""});

            var users = Context.Users.ToList();

            var viewModel = new NewNotificationViewModel()
            {
                Users = users,
                Notification = new Notification()
                {
                    IsActive = true,
                    NotificationDate = DateTime.Now
                }
            };

            return View(viewModel);
        }

        /// <summary>
        /// Server Sent Message
        /// </summary>
        /// <param name="user"></param>
        public void Message(string user)
        {
            _messageAction.Invoke(user);
        }

        /// <summary>
        /// Create new Notification
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="files"></param>
        /// <param name="AttachmentsToIgnore"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNotification(Notification notification, IEnumerable<HttpPostedFileBase> files, string AttachmentsToIgnore)
        {
            if (!ModelState.IsValid)
            {
                var users = Context.Users.ToList();

                var viewModel = new NewNotificationViewModel()
                {
                    Users = users
                };

                return View("Index", viewModel);
            }

            var httpPostedFile = HttpContext.Request.Files[0];

            notification.NotificationDate = DateTime.Now;
            notification.ByUserId = User.Identity.GetUserId();
            notification.DateTimeEntered = DateTime.Now;
            notification.TimeStamp = DateTime.Now;
            notification.DateTimeModified = DateTime.Now;

            string[] filesToIgnore = AttachmentsToIgnore.Split(new char[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var file in files)
            {
                 if (file == null)
                    continue;

                var singleOrDefault = filesToIgnore.SingleOrDefault(f => f.EndsWith(file.FileName) == true);
                if (!string.IsNullOrEmpty(singleOrDefault))
                    continue;

                if (notification.Attachments == null)
                    notification.Attachments = new List<Attachment>();

                SaveFile(file);

                BinaryReader b = new BinaryReader(file.InputStream);
                byte[] binData = b.ReadBytes((int) file.InputStream.Length);

                Attachment attachment = new Attachment
                {
                    Name = file.FileName,
                    DateTimeEntered = DateTime.Now,
                    EnteredBy = notification.EnteredBy,
                    TimeStamp = DateTime.Now,
                    AttachmentContent = binData,
                    DateTimeModified = DateTime.Now,
                    ModifiedBy = notification.EnteredBy
                };

                notification.Attachments.Add(attachment);

            }
     
            Context.Notifications.Add(notification);
            Context.SaveChanges();

            return RedirectToAction("Index", "MyWall");
        }


        private bool SaveFile(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        string path = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                        file.SaveAs(path);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #region "Helper Methods"

        private void MessageAction(string user)
        {
            if (!ModelState.IsValid)
                return;

            if (!Request.IsAuthenticated)
                throw new HttpException((int)System.Net.HttpStatusCode.Forbidden, "Forbidden");

            Response.ContentType = "text/event-stream";

            DateTime startDate = DateTime.Now;

            if (!User.Identity.Name.Equals(user))
                return;


            var notifications = Utils.GetNotificationMessage(User.Identity.GetUserId(), Context);
            if (notifications == null)
                return;

            foreach (var notification in notifications)
            {
                if (notification.NotificationServed.GetValueOrDefault().Date >= DateTime.Today)
                    continue;

                string notificationData = JsonConvert.SerializeObject(notification);
                Response.Write($"data: {notificationData}\n\n");
                Response.Flush();


                UpdateNotificationServedDate(notification);
            }

            Response.Close();
        }

        protected virtual List<Notification> GetNotificationMessage(string userId)
        {
            DateTime dtNow = DateTime.Now.Date;
            List<Notification> notifications = Context.Notifications.Where(n => n.ToUserId == userId &&
                                                                                 n.NotificationExpiry >= dtNow &&
                                                                                 n.IsActive).ToList();

            return notifications;
        }

        protected void UpdateNotificationServedDate(Notification notification, bool set = true)
        {
            var notificationInDbNotification = Context.Notifications.Single(n => (n.Id == notification.Id &&
                                                                                   n.ToUserId == notification
                                                                                       .ToUserId));
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
                        db.Notifications.Attach(notification);
                        db.Entry(notification).Property(p => p.NotificationServed).IsModified = true;
                        db.SaveChanges();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        #endregion
    }
}