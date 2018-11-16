using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SmartStore.Web.Framework.Theming
{
    public class AgentThemedAttribute : FilterAttribute, IResultFilter
    {
        public virtual void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext == null || filterContext.Result == null)
                return;

            // add extra view location formats to all view results (even the partial ones)
            // {0} is appended by view engine
            filterContext.RouteData.DataTokens["ExtraAreaViewLocations"] = new string[]
            {
                "~/Areas/Agent/Views/{1}/{0}",
                "~/Areas/Agent/Views/Shared/{0}"
            };
        }

        public virtual void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }
    }
}
