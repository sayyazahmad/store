using SmartStore.Core.Domain.Orders;
using System.Data.Entity.ModelConfiguration;

namespace SmartStore.Data.Mapping.Orders
{
    public partial class ComissionMap : EntityTypeConfiguration<Comission>
    {
        public ComissionMap()
        {
            ToTable("Comission");
            HasKey(x => x.Id);
            Property(x => x.Remarks).IsMaxLength();
            Property(x => x.ComissionAmt).HasPrecision(18, 4);
            Property(x => x.Point).HasPrecision(18, 4);

            HasRequired(x => x.Order)
               .WithMany(o => o.Comissions)
               .HasForeignKey(x => x.OrderId);
        }
    }
}
