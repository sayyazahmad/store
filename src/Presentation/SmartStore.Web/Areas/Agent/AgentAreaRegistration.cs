using SmartStore.Web.Framework.Routing;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmartStore.Web.Areas.Agent
{
    public class AgentAreaRegistration : AreaRegistration, IRouteProvider
    {
        public override string AreaName
        {
            get
            {
                return "Agent";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
        }

        public int Priority => 999;

        public void RegisterRoutes(RouteCollection routes)
        {
            var route = routes.MapRoute(
                "Agent_default",
                "Agent/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", area = "Agent", id = UrlParameter.Optional }
                , new[] { "SmartStore.Web.Areas.Agent.Controllers" }
            );
            route.DataTokens["area"] = "Agent";
        }
    }
}