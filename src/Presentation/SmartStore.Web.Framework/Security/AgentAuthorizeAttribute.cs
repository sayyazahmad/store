using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SmartStore.Services.Security;

namespace SmartStore.Web.Framework.Security
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AgentAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public IPermissionService PermissionService { get; set; }

        private void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }

        private IEnumerable<AgentAuthorizeAttribute> GetAgentAuthorizeAttributes(ActionDescriptor descriptor)
        {
            return descriptor.GetCustomAttributes(typeof(AgentAuthorizeAttribute), true)
                .Concat(descriptor.ControllerDescriptor.GetCustomAttributes(typeof(AgentAuthorizeAttribute), true))
                .OfType<AgentAuthorizeAttribute>();
        }

        private bool IsAgentPageRequested(AuthorizationContext filterContext)
        {
            var adminAttributes = GetAgentAuthorizeAttributes(filterContext.ActionDescriptor);
            if (adminAttributes != null && adminAttributes.Any())
                return true;
            return false;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
                throw new InvalidOperationException("You cannot use [AgentAuthorize] attribute when a child action cache is active");

            if (IsAgentPageRequested(filterContext))
            {
                if (!this.HasAgentAccess(filterContext))
                    this.HandleUnauthorizedRequest(filterContext);
            }
        }

        public virtual bool HasAgentAccess(AuthorizationContext filterContext)
        {
            var result = PermissionService.Authorize(StandardPermissionProvider.AccessAgentPanel);
            return result;
        }
    }
}
