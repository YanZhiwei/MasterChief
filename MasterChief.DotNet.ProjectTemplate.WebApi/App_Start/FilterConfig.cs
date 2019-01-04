using System.Web;
using System.Web.Mvc;

namespace MasterChief.DotNet.ProjectTemplate.WebApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
