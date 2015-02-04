using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DTS.HelpDesk.Startup))]
namespace DTS.HelpDesk
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
