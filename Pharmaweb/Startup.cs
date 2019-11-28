using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Pharmaweb.Startup))]
namespace Pharmaweb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
