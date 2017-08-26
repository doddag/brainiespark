using System.Web.Mvc;
using brainiespark.Factories;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(brainiespark.Startup))]
namespace brainiespark
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            RegisterDataSource();

            ConfigureAuth(app);
        }

        private void RegisterDataSource()
        {
            ControllerBuilder.Current.SetControllerFactory(new ControllerFactory());
        }
    }
}
