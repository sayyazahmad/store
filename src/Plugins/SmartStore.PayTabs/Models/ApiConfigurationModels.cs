using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SmartStore.ComponentModel;
using SmartStore.PayTabs.Settings;
using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Modelling;
using SmartStore.Web.Framework.Validators;
using SmartStore.Core.Localization;
using System;
using FluentValidation;

namespace SmartStore.PayTabs.Models
{
	public abstract class ApiConfigurationModel : ModelBase
	{

        public string[] ConfigGroups { get; set; }
        public string PrimaryStoreCurrencyCode { get; set; }

        [SmartResourceDisplayName("Plugins.Payments.PayTabs.StoreUrl")]
        public string StoreUrl { get; set; }

        [SmartResourceDisplayName("Plugins.Payments.PayTabs.Currencey")]
        public string Currencey { get; set; }
        
		[SmartResourceDisplayName("Plugins.Payments.PayTabs.MarchantEmail")]
		public string MerchantEmail { get; set; }

		[SmartResourceDisplayName("Plugins.Payments.PayTabs.APIKey")]
		public string APIKey { get; set; }
	}
}