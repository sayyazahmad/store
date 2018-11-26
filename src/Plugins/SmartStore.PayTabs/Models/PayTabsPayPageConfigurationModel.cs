using SmartStore.ComponentModel;
using SmartStore.PayTabs.Settings;
using SmartStore.Web.Framework;

namespace SmartStore.PayTabs.Models
{
	public class PayTabsPayPageConfigurationModel : ApiConfigurationModel
	{
		[SmartResourceDisplayName("Plugins.Payments.PayTabsPayPage.Fields.PayPageAPIUrl")]
		public string PayPageAPIUrl { get; set; }

		[SmartResourceDisplayName("Plugins.Payments.PayTabsPayPage.Fields.PaytabsVerifyPaymentAPIUrl")]
		public string PaytabsVerifyPaymentAPIUrl { get; set; }
        
		public void Copy(PayTabsPayPagePaymentSettings settings, bool fromSettings)
        {
            if (fromSettings)
			{
				MiniMapper.Map(settings, this);
			}
            else
			{
				MiniMapper.Map(this, settings);
				settings.MerchantEmail = MerchantEmail.TrimSafe();
                settings.APIKey = APIKey.TrimSafe();
                settings.StoreUrl = StoreUrl.TrimSafe();
                settings.PayPageAPIUrl = PayPageAPIUrl.TrimSafe();
                settings.PaytabsVerifyPaymentAPIUrl = PaytabsVerifyPaymentAPIUrl.TrimSafe();
			}
        }
	}
}