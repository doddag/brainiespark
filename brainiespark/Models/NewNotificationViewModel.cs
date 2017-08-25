using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace brainiespark.Models
{
    public class NewNotificationViewModel
    {
        public List<ApplicationUser> Users { get; set; }
        public Notification Notification { get; set; }
        public List<Notification> Notifications { get; set; }
    }
}