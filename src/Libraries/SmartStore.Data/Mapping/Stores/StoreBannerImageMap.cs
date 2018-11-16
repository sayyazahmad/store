using SmartStore.Core.Domain.Stores;
using System.Data.Entity.ModelConfiguration;

namespace SmartStore.Data.Mapping.Stores
{
    public class StoreBannerImageMap : EntityTypeConfiguration<StoreBannerImage>
    {
        public StoreBannerImageMap()
        {
            this.ToTable("StoreBannerImage");
            this.HasKey(x => x.Id);
            
        }
    }
}
