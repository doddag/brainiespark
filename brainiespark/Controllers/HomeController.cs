using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using brainiespark.Helpers;
using brainiespark.Models;

namespace brainiespark.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext Context;

        public HomeController(ApplicationDbContext context)
        {
            Context = context;
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
            ViewBag.Message = "Brainiespark";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "goutham.reddy.d@gmail.com";

            return View();
        }
    }
}