using SmartStore.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Data.Mapping.Customers
{
    public class CustomerPointsMap : EntityTypeConfiguration<CustomerPoints>
    {
        public CustomerPointsMap()
        {
            this.ToTable("CustomerPoints");
            this.HasKey(o => o.Id);
            
            this.HasRequired(cc => cc.Customer)
                .WithMany(c => c.CustomerPoints)
                .HasForeignKey(cc => cc.CustomerId);
        }
    }
}
