using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FitFeastExplore.Startup))]
namespace FitFeastExplore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
