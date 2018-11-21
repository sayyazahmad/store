using System.Web.Mvc;
using System.Web.Routing;
using SmartStore.Web.Framework.Routing;

namespace SmartStore.PayTabs
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("SmartStore.PayTabsPayPage",
                "Plugins/SmartStore.PayTabs/{controller}/{action}",
                new { controller = "PayTabsPayPage", action = "Index" },
                new[] { "SmartStore.PayTabs.Controllers" }
            )
            .DataTokens["area"] = "SmartStore.PayTabs";
        }

        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
