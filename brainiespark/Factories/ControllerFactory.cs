using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using brainiespark.Controllers;
using brainiespark.Models;
using Microsoft.JScript;

namespace brainiespark.Factories
{
    public class ControllerFactory : DefaultControllerFactory
    {
        public ControllerFactory()
        {
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext,
            Type controllerType)
        {
            if (controllerType == typeof(MyWallController))
                return new MyWallController(ApplicationDbContext.Create());
            else if (controllerType == typeof(HomeController))
                return new HomeController(ApplicationDbContext.Create());
            else if (controllerType == typeof(ViewNotificationController))
                return new ViewNotificationController(ApplicationDbContext.Create());

            return base.GetControllerInstance(requestContext, controllerType);
        }


        public override void ReleaseController(IController controller)
        {
            IDisposable dispose = controller as IDisposable;
            dispose?.Dispose();
        }
    }
}
