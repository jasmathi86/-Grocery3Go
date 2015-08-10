using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Grocery3Go.Startup))]
namespace Grocery3Go
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
