using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Mvc;
using brainiespark.Helpers;
using brainiespark.Models;
using Microsoft.AspNet.Identity;

namespace brainiespark.Controllers.Api
{
    public class NotificationsController : ApiController
    {
        public ApplicationDbContext Context { get; }

        public NotificationsController()
        {
            Context = new ApplicationDbContext();
        }

        // PUT /api/notifications/1/flase
        [System.Web.Http.HttpPut]
        public void UpdateNotificationsActiveStatus(int id, bool isActive)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var notificationInDb = Context.Notifications.SingleOrDefault(n => n.Id == id &&
                                                                               n.ToUserId == User.Identity.GetUserId());
            if (notificationInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            notificationInDb.IsActive = false;
            Context.SaveChanges();
        }

        // PUT /api/notifications/1/flase
        [System.Web.Http.HttpPut]
        [System.Web.Http.ActionName("ReSyncActiveNoifications")]
        public void ReSyncActiveNoifications(HttpRequestMessage requestMessage)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            HttpRequestHeaders h = requestMessage.Headers;
            try
            {
                var token = h.GetValues("RequestVerificationToken").FirstOrDefault();
                string message = "";
                if (!Helpers.Security.ValidateAntiForgeryTokens(token, out message))
                {
                    return;
                }
            }
            catch (Exception)
            {
                return;
            }

            string userId = User.Identity.GetUserId();

            List<Notification> notifications = Context.Notifications.Where(n => n.ToUserId == userId &&
                                                                                 n.IsActive).ToList();
            try
            {
                foreach (var notificationInDb in notifications)
                {
                    notificationInDb.NotificationServed = null;
                    notificationInDb.Message =
                        notificationInDb.Message.PadRight(
                            Notification.MinMessageLength);

                    using (var db = new ApplicationDbContext())
                    {
                        db.Notifications.Attach(notificationInDb);
                        db.Entry(notificationInDb).Property(p => p.NotificationServed).IsModified = true;
                        db.SaveChanges();
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        // DELETE /api/notifications/1
        [System.Web.Http.HttpDelete]
        public void DeleteNotification(int id, HttpRequestMessage requestMessage)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            HttpRequestHeaders h = requestMessage.Headers;
            try
            {
                var token = h.GetValues("RequestVerificationToken").FirstOrDefault();
                string message = "";
                if (!Helpers.Security.ValidateAntiForgeryTokens(token, out message))
                {
                    return;
                }
            }
            catch (Exception)
            {
                return;
            }


            string userId = User.Identity.GetUserId();
            var notificationInDb = Context.Set<Notification>().Include(m => m.Attachments)
                .SingleOrDefault(n => n.Id == id && n.ToUserId == userId);

            if (notificationInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Context.Set<Notification>().Remove(notificationInDb);
            Context.SaveChanges();
        }

       
    }
}
