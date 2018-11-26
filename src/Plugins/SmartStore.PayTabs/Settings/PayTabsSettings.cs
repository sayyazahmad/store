using System;
using System.Collections.Generic;
using SmartStore.Core.Configuration;

namespace SmartStore.PayTabs.Settings
{
	public abstract class PayTabsSettingsBase
    {
		public PayTabsSettingsBase()
		{
			AddOrderNotes = true;
		}

		public bool AddOrderNotes { get; set; }

		public bool AdditionalFeePercentage { get; set; }
        
        public decimal AdditionalFee { get; set; }

        public string WebhookId { get; set; }
    }

    public abstract class PayTabsApiSettingsBase : PayTabsSettingsBase
	{
		public string ApiAccountName { get; set; }
	}
    
	public class PayTabsPayPagePaymentSettings : PayTabsApiSettingsBase, ISettings
    {
        public string MerchantEmail { get; set; }
        public string APIKey { get; set; }
        public string StoreUrl { get; set; }
        public string PayPageAPIUrl { get; set; }
        public string PaytabsVerifyPaymentAPIUrl { get; set; }
    }
}
