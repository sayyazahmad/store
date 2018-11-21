using SmartStore.Core.Configuration;
using SmartStore.Core.Domain.Payments;
using SmartStore.PayTabs.Models;
using SmartStore.PayTabs.Settings;
using SmartStore.Services.Orders;
using SmartStore.Services.Payments;
using SmartStore.Web.Framework.Controllers;
using System;

namespace SmartStore.PayTabs.Controllers
{
    public abstract class PayTabsPaymentControllerBase : PaymentControllerBase
	{
		protected void PrepareConfigurationModel(ApiConfigurationModel model, int storeScope)
		{
			var store = storeScope == 0
				? Services.StoreContext.CurrentStore
				: Services.StoreService.GetStoreById(storeScope);
            
			model.PrimaryStoreCurrencyCode = store.PrimaryStoreCurrency.CurrencyCode;
		}
	}

	public abstract class PayTabsControllerBase<TSetting> : PayTabsPaymentControllerBase where TSetting : PayTabsSettingsBase, ISettings, new()
	{
		public PayTabsControllerBase(
			string systemName,
			IPaymentService paymentService,
			IOrderService orderService,
			IOrderProcessingService orderProcessingService)
		{
			SystemName = systemName;
			PaymentService = paymentService;
			OrderService = orderService;
			OrderProcessingService = orderProcessingService;
		}

		protected string SystemName { get; private set; }
		protected IPaymentService PaymentService { get; private set; }
		protected IOrderService OrderService { get; private set; }
		protected IOrderProcessingService OrderProcessingService { get; private set; }

		protected PaymentStatus GetPaymentStatus(string paymentStatus, string pendingReason, decimal PayTabsTotal, decimal orderTotal)
		{
			var result = PaymentStatus.Pending;

			if (paymentStatus == null)
				paymentStatus = string.Empty;

			if (pendingReason == null)
				pendingReason = string.Empty;

			switch (paymentStatus.ToLowerInvariant())
			{
				case "pending":
					switch (pendingReason.ToLowerInvariant())
					{
						case "authorization":
							result = PaymentStatus.Authorized;
							break;
						default:
							result = PaymentStatus.Pending;
							break;
					}
					break;
				case "processed":
				case "completed":
				case "canceled_reversal":
					result = PaymentStatus.Paid;
					break;
				case "denied":
				case "expired":
				case "failed":
				case "voided":
					result = PaymentStatus.Voided;
					break;
				case "reversed":
					result = PaymentStatus.Refunded;
					break;
				case "refunded":
					if ((Math.Abs(orderTotal) - Math.Abs(PayTabsTotal)) > decimal.Zero)
						result = PaymentStatus.PartiallyRefunded;
					else
						result = PaymentStatus.Refunded;
					break;
				default:
					break;
			}
			return result;
		}
	}
}