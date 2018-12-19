using SmartStore.Core.Domain.Common;
using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Modelling;
using System;

namespace SmartStore.Admin.Models.Agent
{
    public class CommissionRequestModel : EntityModelBase
    {
        public int CustomerId { get; set; }

        [SmartResourceDisplayName("Admin.Agent.CommissionRequest.TotalCommission")]
        public decimal TotalCommission { get; set; }

        [SmartResourceDisplayName("Admin.Agent.CommissionRequest.AvailableCommission")]
        public decimal AvailableCommission { get; set; }

        [SmartResourceDisplayName("Admin.Agent.CommissionRequest.TotalProfit")]
        public decimal TotalProfit { get; set; }

        [SmartResourceDisplayName("Admin.Agent.CommissionRequest.AvailableProfit")]
        public decimal AvailableProfit { get; set; }

        public decimal AvailableCommissionOrg { get; set; }

        public decimal AvailableProfitOrg { get; set; }

        [SmartResourceDisplayName("Admin.Agent.CommissionRequest.CommissionWithdrawAmount")]
        public decimal? CommissionWithdrawAmount { get; set; }

        [SmartResourceDisplayName("Admin.Agent.CommissionRequest.ProfitWithdrawAmount")]
        public decimal? ProfitWithdrawAmount { get; set; }

        [SmartResourceDisplayName("Admin.Agent.CommissionRequest.TotalWithdrawAmount")]
        public decimal? TotalWithdrawAmount { get { return CommissionWithdrawAmount + ProfitWithdrawAmount; } }

        [SmartResourceDisplayName("Admin.Agent.CommissionRequest.Note")]
        public string Note { get; set; }

        [SmartResourceDisplayName("Admin.Agent.CommissionRequest.CreatedOn")]
        public DateTime CreatedOnUtc { get; set; }

        [SmartResourceDisplayName("Admin.Agent.CommissionRequest.UpdatedOn")]
        public DateTime UpdatedOnUtc { get; set; }

        [SmartResourceDisplayName("Admin.Agent.CommissionRequest.Status")]
        public int RequestStatusId { get; set; }

        [SmartResourceDisplayName("Admin.Agent.CommissionRequest.Status")]
        public string Status { get; set; }

        [SmartResourceDisplayName("Admin.Agent.CommissionRequest.Customer")]
        public string CustomerName { get; set; }

        public bool CanRequestPayment { get; set; }

        public RequestStatus RequestStatus
        {
            get
            {
                return (RequestStatus)this.RequestStatusId;
            }
            set
            {
                this.RequestStatusId = (int)value;
            }
        }
    }
}