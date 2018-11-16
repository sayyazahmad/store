using SmartStore.Core.Domain.Customers;
using System;
using System.Runtime.Serialization;

namespace SmartStore.Core.Domain.Common
{
    public class BankUpdateRequest : BaseEntity, IAuditable
    {
        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        [DataMember]
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the customer
        /// </summary>
        [DataMember]
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets the IP address
        /// </summary>
        [DataMember]
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        [DataMember]
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance update
        /// </summary>
        [DataMember]
        public DateTime UpdatedOnUtc { get; set; }
        /// <summary>
        /// Gets or sets the payment status identifier
        /// </summary>
        [DataMember]

        public string BankName { get; set; }

        [DataMember]
        public string IBAN { get; set; }
        
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
