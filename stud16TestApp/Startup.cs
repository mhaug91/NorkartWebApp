using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(stud16TestApp.Startup))]
namespace stud16TestApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
