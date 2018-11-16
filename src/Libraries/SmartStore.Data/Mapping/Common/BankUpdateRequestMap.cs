using SmartStore.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Data.Mapping.Common
{
    public class BankUpdateRequestMap : EntityTypeConfiguration<BankUpdateRequest>
    {
        public BankUpdateRequestMap()
        {
            this.ToTable("BankUpdateRequest");
            this.HasKey(o => o.Id);

            this.Ignore(o => o.RequestStatus);

            this.HasRequired(cc => cc.Customer)
                .WithMany(c => c.BankUpdateRequests)
                .HasForeignKey(cc => cc.CustomerId);
        }
    }
}
