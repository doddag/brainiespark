using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Action<string> _messageAction;
        private readonly ApplicationDbContext Context;

        public MyWallController(ApplicationDbContext context)
        {
            Context = context;
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
        /// <param name="attachmentsToIgnore"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNotification(Notification notification, IEnumerable<HttpPostedFileBase> files, string attachmentsToIgnore)
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

            Notification.SaveFileHandler saveFileHandler = Utils.SaveFile;
            notification.Create(Context, Server.MapPath("~/UploadedFiles"), files, attachmentsToIgnore.ToArray(new char[] { '|' }), User.Identity.GetUserId(), saveFileHandler);

            return RedirectToAction("Index", "MyWall");
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


            var notifications = Utils.GetNotificationMessage(!Request.IsAuthenticated ? null : User.Identity.GetUserId(), Context);
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
            #region "ToBeRemoved"

            //var notificationInDbNotification = Context.Notifications.Single(n => (n.Id == notification.Id &&
            //                                                                       n.ToUserId == notification
            //                                                                           .ToUserId));
            //if (notificationInDbNotification != null)
            //{
            //    try
            //    {
            //        if (set)
            //        {
            //            notificationInDbNotification
            //                .NotificationServed = DateTime.Now;
            //        }
            //        else
            //        {
            //            notificationInDbNotification
            //                .NotificationServed = null;
            //        }

            //        notificationInDbNotification.Message =
            //            notificationInDbNotification.Message.PadRight(
            //                Notification.MinMessageLength);

            //        using (var db = new ApplicationDbContext())
            //        {
            //            db.Notifications.Attach(notification);
            //            db.Entry(notification).Property(p => p.NotificationServed).IsModified = true;
            //            db.SaveChanges();
            //        }
            //    }
            //    catch (DbEntityValidationException e)
            //    {
            //        Console.WriteLine(e);
            //    }
            //}
            #endregion

            notification.SetNotificationServedDate(Context, set);
        }

        #endregion
    }
}