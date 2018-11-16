using SmartStore.Core.Domain.Common;
using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Modelling;
using System;

namespace SmartStore.Admin.Models.Common
{
    public class BankUpdateRequestModel : EntityModelBase
    {
        public int CustomerId { get; set; }

        public Customers.CustomerModel Customer { get; set; }

        [SmartResourceDisplayName("Admin.Agent.BankUpdateRequest.BankName")]
        public string BankName { get; set; }

        [SmartResourceDisplayName("Admin.Agent.BankUpdateRequest.IBAN")]
        public string IBAN { get; set; }
        
        [SmartResourceDisplayName("Admin.Agent.BankUpdateRequest.Status")]
        public int RequestStatusId { get; set; }

        [SmartResourceDisplayName("Admin.Agent.BankUpdateRequest.CreatedOn")]
        public DateTime CreatedOnUtc { get; set; }

        [SmartResourceDisplayName("Admin.Agent.BankUpdateRequest.Status")]
        public string Status { get; set; }

        [SmartResourceDisplayName("Admin.Agent.BankUpdateRequest.Customer")]
        public string CustomerName { get; set; }

        [SmartResourceDisplayName("Admin.Agent.BankUpdateRequest.CurrentBankName")]
        public string CurrentBankName { get; set; }

        [SmartResourceDisplayName("Admin.Agent.BankUpdateRequest.CurrentIBAN")]
        public string CurrentIBAN { get; set; }

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