using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCPapperLess.Startup))]
namespace MVCPapperLess
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
