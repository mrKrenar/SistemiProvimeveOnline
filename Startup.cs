using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SistemiProvimeveOnline.Startup))]
namespace SistemiProvimeveOnline
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
