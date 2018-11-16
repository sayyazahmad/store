using SmartStore.Core.Domain.Media;
using System.Runtime.Serialization;

namespace SmartStore.Core.Domain.Stores
{
    public partial class StoreBannerImage : BaseEntity
    {
        [DataMember]
        public int StoreId { get; set; }
        
        [DataMember]
        public int DisplayOrder { get; set; }

        [DataMember]
        public string URL { get; set; }
    }
}
