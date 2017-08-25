using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using brainiespark.Models;

namespace brainiespark.Helpers
{
    public class Security
    {
        public static bool ValidateAntiForgeryTokens(string token, out string message)
        {
            message = "Token Invalid";

            if (string.IsNullOrEmpty(token))
                return false;

            string[] tokens = token.Split(new char[] {':'}, StringSplitOptions.None);
            if (tokens.Length != 2)
                return false;

            try
            {
                AntiForgery.Validate(tokens[0].Trim(), tokens[1].Trim());
                message = "";
                return true;
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }
        }
    }

    public class Utils
    {
        public static List<Notification> GetNotificationMessage(string userId, ApplicationDbContext context)
        {
            DateTime dtNow = DateTime.Now.Date;
            List<Notification> notifications;

            if (!string.IsNullOrEmpty(userId))
                notifications = context.Notifications.Where(n => n.ToUserId == userId &&
                                                                                n.NotificationExpiry >= dtNow &&
                                                                                n.IsActive).ToList();
            else
            {
                 notifications = context.Notifications.Where(n => n.NotificationExpiry >= dtNow &&
                                                                 n.IsActive &&
                                                                 n.IsPublic).Include(m => m.Attachments).OrderByDescending(n => n.NotificationDate).ToList();
            }

            return notifications;
        }
    }
}