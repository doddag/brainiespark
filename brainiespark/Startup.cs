using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(brainiespark.Startup))]
namespace brainiespark
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
