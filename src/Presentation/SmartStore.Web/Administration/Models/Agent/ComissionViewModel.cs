using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Orders;
using SmartStore.Web.Framework;
using System;
using System.ComponentModel;

namespace SmartStore.Admin.Models.Agent
{
    public class ComissionViewModel
    {
        public int CustomerId { get; set; }
        
        public int ProductId { get; set; }
        
        [SmartResourceDisplayName("Agent.Commission.Order")]
        public int OrderId { get; set; }

        [SmartResourceDisplayName("Agent.Commission.CreatedOn")]
        public DateTime CreatedOnUtc { get; set; }

        [SmartResourceDisplayName("Agent.Commission.PaymentType")]
        public string PaymentType { get; set; }

        [SmartResourceDisplayName("Agent.Commission.OrderTotal")]
        public decimal OrderTotal { get; set; }

        [SmartResourceDisplayName("Agent.Commission.Commission")]
        public decimal? Commission { get; set; }

        [SmartResourceDisplayName("Agent.Commission.Profit")]
        public decimal? Profit { get; set; }
        
        [SmartResourceDisplayName("Agent.Commission.MembershipPlan")]
        public string MembershipPlan { get; set; }

        [SmartResourceDisplayName("Agent.Commission.PaymentMethod")]
        public string PaymentMethod { get; set; }

        [SmartResourceDisplayName("Agent.Commission.CommissionPlusProfit")]
        public decimal? CommissionPlusProfit { get { return Commission + Profit; } }

        [SmartResourceDisplayName("Agent.Commission.AdminComment")]
        public string Remarks{ get; set; }

    }
}
