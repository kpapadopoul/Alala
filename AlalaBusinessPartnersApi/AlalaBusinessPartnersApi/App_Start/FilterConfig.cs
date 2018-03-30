using System.Web;
using System.Web.Mvc;

namespace AlalaBusinessPartnersApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
