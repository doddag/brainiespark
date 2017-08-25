using brainiespark.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace brainiespark.Controllers
{
    public class GuestWallController : Controller
    {
        // GET: GuestWall
        public ActionResult Random()
        {
            var guestWall = new GuestWall() { Name = "Guest", Message = ""  };
            return View(guestWall);
        }
    }
}