using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KeyWordsWebCrawler.Startup))]
namespace KeyWordsWebCrawler
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
