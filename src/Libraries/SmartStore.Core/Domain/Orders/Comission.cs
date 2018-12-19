using SmartStore.Core.Domain.Catalog;
using System;
using System.Runtime.Serialization;

namespace SmartStore.Core.Domain.Orders
{
    [DataContract]
    public partial class Comission : BaseEntity
    {
        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public int OrderId { get; set; }

        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public decimal ComissionAmt { get; set; }

        [DataMember]
        public decimal ProfitAmt { get; set; }

        [DataMember]
        public decimal Point { get; set; }

        [DataMember]
        public bool ComissionPaid { get; set; }

        [DataMember]
        public string Remarks { get; set; }

        [DataMember]
        public DateTime CreatedOnUtc { get; set; }

        [DataMember]
        public DateTime UpdatedOnUtc { get; set; }

        [DataMember]
        public virtual Order Order { get; set; }
        
		[DataMember]
        public virtual Product Product { get; set; }
    }
}
