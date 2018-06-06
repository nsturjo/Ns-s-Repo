using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sample_Voucher_Entry.Startup))]
namespace Sample_Voucher_Entry
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
