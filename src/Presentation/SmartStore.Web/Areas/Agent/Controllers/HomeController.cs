using SmartStore.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartStore.Web.Areas.Agent.Controllers
{
    public class HomeController : AgentControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}