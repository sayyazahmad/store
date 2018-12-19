using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Core.Domain.Agent
{
    public class CommissionRequest : BaseEntity, IAuditable
    {
        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public virtual Customer Customer { get; set; }

        public decimal TotalCommission { get; set; }

        [DataMember]
        public decimal AvailableCommission { get; set; }

        [DataMember]
        public decimal TotalProfit { get; set; }

        [DataMember]
        public decimal AvailableProfit { get; set; }

        [DataMember]
        public decimal? CommissionWithdrawAmount { get; set; }

        [DataMember]
        public decimal? ProfitWithdrawAmount { get; set; }

        [DataMember]
        public string Note { get; set; }

        [DataMember]
        public DateTime CreatedOnUtc { get; set; }
        
        [DataMember]
        public DateTime UpdatedOnUtc { get; set; }

        public int RequestStatusId { get; set; }

        [DataMember]
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
