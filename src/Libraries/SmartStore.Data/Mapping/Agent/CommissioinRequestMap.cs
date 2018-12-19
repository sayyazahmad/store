using SmartStore.Core.Domain.Agent;
using System.Data.Entity.ModelConfiguration;

namespace SmartStore.Data.Mapping.Agent
{
    public class CommissioinRequestMap : EntityTypeConfiguration<CommissionRequest>
    {
        public CommissioinRequestMap()
        {
            ToTable("CommissionRequest");
            HasKey(o => o.Id);
            Ignore(o => o.RequestStatus);
            HasRequired(cc => cc.Customer).WithMany(c => c.CommissionRequests).HasForeignKey(cc => cc.CustomerId);
        }
    }
}
