using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(mmsh.Startup))]
namespace mmsh
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
