using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestHabanero.Startup))]
namespace TestHabanero
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
