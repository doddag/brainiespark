using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using brainiespark.Helpers;
using brainiespark.Models;
using Microsoft.AspNet.Identity;

namespace brainiespark.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationDbContext Context { get; }

        public HomeController()
        {
            Context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var viewModel = new NewNotificationViewModel()
            {
                Notifications = Utils.GetNotificationMessage(null, Context)
            };

            return View(viewModel);

            /* HttpRequestHeaders h = requestMessage.Headers;
             try
             {
                 var token = h.GetValues("RequestVerificationToken").FirstOrDefault();
                 string message = "";
                 if (!Helpers.Security.ValidateAntiForgeryTokens(token, out message))
                 {
                     return new EmptyResult();
                 }
             }
             catch (Exception)
             {
                 return new EmptyResult();
             }*/


            // notificationInDb.

        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}