using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MMOasisAlajuela.Startup))]
namespace MMOasisAlajuela
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
