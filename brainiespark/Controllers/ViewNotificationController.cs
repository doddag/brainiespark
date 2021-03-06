﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Mvc;
using brainiespark.Models;
using Microsoft.AspNet.Identity;

namespace brainiespark.Controllers
{
    public class ViewNotificationController : Controller
    {
        public ApplicationDbContext Context { get; }

        public ViewNotificationController(ApplicationDbContext context)
        {
            Context = context;
        }

        // GET: ViewNotification
        public ActionResult ViewNotification(int? id, HttpRequestMessage requestMessage)
        {
            // TO DO : Antiforgery Validations
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
             } */

            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            if (!Request.IsAuthenticated || !id.HasValue)
                return RedirectToAction("Login", "Account", new { returnUrl = "" });

            var users = Context.Users.ToList();
    

            string userId = User.Identity.GetUserId();
            var notificationInDb = Context.Set<Notification>().Include(m => m.Attachments)
                .SingleOrDefault(n => n.Id == id && n.ToUserId == userId);

            var viewModel = new NewNotificationViewModel()
            {
                Users = users,
                Notification = notificationInDb     
            };

            return View(viewModel);
        }
    }
}