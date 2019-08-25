using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BDApaint.Startup))]
namespace BDApaint
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
