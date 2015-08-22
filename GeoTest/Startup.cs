using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(GeoTest.Startup))]

namespace GeoTest
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
