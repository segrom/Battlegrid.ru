using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Battlegrid.ru.Startup))]
namespace Battlegrid.ru
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
