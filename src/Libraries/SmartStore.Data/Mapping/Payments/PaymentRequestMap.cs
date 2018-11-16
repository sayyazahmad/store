using SmartStore.Core.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Data.Mapping.Payments
{
    public class PaymentRequestMap : EntityTypeConfiguration<PaymentRequest>
    {
        public PaymentRequestMap()
        {
            this.ToTable("PaymentRequest");
            this.HasKey(o => o.Id);

            this.Ignore(o => o.PaymentStatus);

            this.HasRequired(cc => cc.Customer)
                .WithMany(c => c.PaymentRequests)
                .HasForeignKey(cc => cc.CustomerId);
        }
    }
}
