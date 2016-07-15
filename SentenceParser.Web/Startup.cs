using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SentenceParser.Web.Startup))]
namespace SentenceParser.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
