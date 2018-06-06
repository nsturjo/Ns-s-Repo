using System.Web;
using System.Web.Mvc;

namespace Sample_Voucher_Entry
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
