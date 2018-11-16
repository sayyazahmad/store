using SmartStore.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Data.Mapping.Customers
{
    public class CustomerMembershipMap : EntityTypeConfiguration<CustomerMembership>
    {
        public CustomerMembershipMap()
        {
            this.ToTable("CustomerMembership");
            this.HasKey(o => o.Id);
            
            this.HasRequired(cc => cc.Customer)
                .WithMany(c => c.CustomerMemberships)
                .HasForeignKey(cc => cc.CustomerId);
        }
    }
}
