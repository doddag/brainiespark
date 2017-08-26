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
        private readonly ApplicationDbContext _context;

        public NotificationsController()
        {
            _context = ApplicationDbContext.Create();
        }

        // PUT /api/notifications/1/flase
        [System.Web.Http.HttpPut]
        public void UpdateNotificationsActiveStatus(int id, bool isActive)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var notificationInDb =_context.Notifications.SingleOrDefault(n => n.Id == id &&
                                                                               n.ToUserId == User.Identity.GetUserId());
            if (notificationInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            notificationInDb.IsActive = false;
           _context.SaveChanges();
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

            var notifications = Utils.GetNotificationMessage(userId, _context);
            try
            {
                foreach (var notificationInDb in notifications)
                {
                    notificationInDb.NotificationServed = null;
                    notificationInDb.Message =
                        notificationInDb.Message.PadRight(
                            Notification.MinMessageLength);


                   _context.Notifications.Attach(notificationInDb);
                   _context.Entry(notificationInDb).Property(p => p.NotificationServed).IsModified = true;
                   _context.SaveChanges();
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
            var notificationInDb =_context.Set<Notification>().Include(m => m.Attachments)
                .SingleOrDefault(n => n.Id == id && n.ToUserId == userId);

            if (notificationInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

           _context.Set<Notification>().Remove(notificationInDb);
           _context.SaveChanges();
        }

       
    }
}
