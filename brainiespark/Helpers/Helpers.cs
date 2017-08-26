using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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

    public static class Utils
    {
        public static List<Notification> GetNotificationMessage(string userId, ApplicationDbContext dataSource)
        {
            DateTime dtNow = DateTime.Now.Date;
            List<Notification> notifications;

            if (!string.IsNullOrEmpty(userId))
                notifications = dataSource.Notifications.Where(n => n.ToUserId == userId &&
                                                                                n.NotificationExpiry >= dtNow &&
                                                                                n.IsActive).ToList();
            else
            {
                 notifications = dataSource.Notifications.Where(n => n.NotificationExpiry >= dtNow &&
                                                                 n.IsActive &&
                                                                 n.IsPublic).Include(m => m.Attachments).OrderByDescending(n => n.NotificationDate).ToList();
            }

            return notifications;
        }

        public static byte[] SaveFile(HttpPostedFileBase file, string serverPath)
        {
            try
            {
                byte[] binData = new byte[] { };
                if (file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        string path = Path.Combine(serverPath, fileName);
                        file.SaveAs(path);

                        BinaryReader b = new BinaryReader(file.InputStream);
                        binData = b.ReadBytes((int)file.InputStream.Length);
                    }
                }
                return binData;
            }
            catch
            {
                return null;
            }
        }
    }
}