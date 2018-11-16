using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Modelling;
using System;

namespace SmartStore.Admin.Models.Agent
{
    public class DashboardModel : EntityModelBase
    {
        [SmartResourceDisplayName("Admin.Agent.Dashboard.LastLogin")]
        public DateTime? LastLogin { get; set; }

        [SmartResourceDisplayName("Admin.Agent.Dashboard.TotalSales")]
        public int? TotalSales { get; set; }

        [SmartResourceDisplayName("Admin.Agent.Dashboard.TotalPoints")]
        public int? TotalPoints { get; set; }
    }
}