using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Modelling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.Admin.Models.Membership
{
    public class MembershipPlanModel : EntityModelBase
    {
        [SmartResourceDisplayName("Admin.MembershipPlan.PaymentRequestDaysGap")]
        public int PaymentRequestDaysGap { get; set; }
        [SmartResourceDisplayName("Admin.MembershipPlan.Order")]
        public int Order { get; set; }

        [SmartResourceDisplayName("Admin.MembershipPlan.Title")]
        public string Title { get; set; }

        [SmartResourceDisplayName("Admin.MembershipPlan.Description")]
        public string Description { get; set; }

        [SmartResourceDisplayName("Admin.MembershipPlan.EarnPoint")]
        public decimal EarnPoint { get; set; }

        [SmartResourceDisplayName("Admin.MembershipPlan.ComissionPct")]
        public decimal ComissionPct { get; set; }

        [SmartResourceDisplayName("Admin.MembershipPlan.Fee")]
        public decimal Fee { get; set; }

        [SmartResourceDisplayName("Admin.MembershipPlan.IsDefault")]
        public bool IsDefault { get; set; }

        [SmartResourceDisplayName("Admin.MembershipPlan.IsTrail")]
        public bool IsTrail { get; set; }

        [SmartResourceDisplayName("Admin.MembershipPlan.PointToUpgrade")]
        public int PointToUpgrade { get; set; }

        [SmartResourceDisplayName("Admin.MembershipPlan.ComissionRequestResentDays")]
        public int ComissionRequestResentDays { get; set; }

        [SmartResourceDisplayName("Admin.MembershipPlan.AvailableOnRegistration")]
        public bool AvailableOnRegistration { get; set; }
    }
}