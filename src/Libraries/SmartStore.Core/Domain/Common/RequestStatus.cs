using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Core.Domain.Common
{
    public enum RequestStatus
    {
        /// <summary>
        /// Pending
        /// </summary>
        Pending = 10,
        /// <summary>
        /// Authorized
        /// </summary>
        Approved = 20,
        /// <summary>
        /// Paid
        /// </summary>
        Rejected = 30,
        /// <summary>
        /// Partially Refunded
        /// </summary>
        OnHold = 35,
        /// <summary>
        /// Refunded
        /// </summary>
        Voided = 40
    }
}
