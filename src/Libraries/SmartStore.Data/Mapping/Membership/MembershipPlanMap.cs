using SmartStore.Core.Domain.Membership;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Data.Mapping.Membership
{
    public class MembershipPlanMap : EntityTypeConfiguration<MembershipPlan>
    {
        public MembershipPlanMap()
        {
            this.ToTable("MembershipPlan");
            this.HasKey(o => o.Id);
            this.Property(o => o.EarnPoint).HasPrecision(18, 4);
            this.Property(o => o.Fee).HasPrecision(18, 4);
        }
    }
}
