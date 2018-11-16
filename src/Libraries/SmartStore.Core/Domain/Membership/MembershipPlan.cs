using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Core.Domain.Membership
{
    public class MembershipPlan : BaseEntity, IAuditable
    {
        [DataMember]
        public int Order { get; set; }

        [DataMember]
        public int PaymentRequestDaysGap { get; set; }

        /// <summary>
        /// Gets or sets the date and time of order creation
        /// </summary>
		[DataMember]
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time when order was updated
        /// </summary>
        [DataMember]
        public DateTime UpdatedOnUtc { get; set; }

        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public decimal EarnPoint { get; set; }
        [DataMember]
        public decimal ComissionPct { get; set; }
        [DataMember]
        public decimal Fee { get; set; }
        [DataMember]
        public bool IsDefault { get; set; }
        [DataMember]
        public bool IsTrail { get; set; }
        [DataMember]
        public int PointToUpgrade { get; set; }
        [DataMember]
        public int ComissionRequestResentDays { get; set; }
        [DataMember]
        public bool AvailableOnRegistration { get; set; }
    }
}
