using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace brainiespark.Controllers.Api
{
    public class UploadController : ApiController
    {
        // PUT /api/notifications/1/flase
        [HttpPost]
        public void UploadFile(HttpPostedFile uploadFile)
        {
            //var httpPostedFile = HttpContext.Request.Files;
        }

    }
}
