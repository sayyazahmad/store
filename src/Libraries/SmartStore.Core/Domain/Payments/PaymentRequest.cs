using SmartStore.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Core.Domain.Payments
{
    public class PaymentRequest : BaseEntity, IAuditable
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
        /// Gets or sets the date and time of order creation
        /// </summary>
		[DataMember]
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time when order was updated
        /// </summary>
        [DataMember]
        public DateTime UpdatedOnUtc { get; set; }
        public string UserEmail { get; set; }
        public string UserFullName { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public DateTime Date { get; set; }


        /// <summary>
        /// Gets or sets the payment status identifier
        /// </summary>
        [DataMember]
        public int PaymentStatusId { get; set; }

        [DataMember]
        public PaymentStatus PaymentStatus
        {
            get
            {
                return (PaymentStatus)this.PaymentStatusId;
            }
            set
            {
                this.PaymentStatusId = (int)value;
            }
        }
    }
}
