using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShaulisCrazyFans.Startup))]
namespace ShaulisCrazyFans
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
