using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Web.Routing;
using SmartStore.Core.Configuration;
using SmartStore.Core.Domain.Directory;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Domain.Payments;
using SmartStore.Core.Logging;
using SmartStore.Core.Plugins;
using SmartStore.PayTabs.Settings;
using SmartStore.Services;
using SmartStore.Services.Orders;
using SmartStore.Services.Payments;

namespace SmartStore.PayTabs
{
	public abstract class PayTabsProviderBase<TSetting> : PaymentMethodBase, IConfigurable where TSetting : PayTabsApiSettingsBase, ISettings, new()
    {
        protected PayTabsProviderBase()
		{
			Logger = NullLogger.Instance;
		}

		public static string ApiVersion
		{
			get { return "109"; }
		}

		public ILogger Logger { get; set; }
		public ICommonServices Services { get; set; }
		public IOrderService OrderService { get; set; }
        public IOrderTotalCalculationService OrderTotalCalculationService { get; set; }

		public override bool SupportCapture
		{
			get { return true; }
		}

		public override bool SupportPartiallyRefund
		{
			get { return true; }
		}

		public override bool SupportRefund
		{
			get { return true; }
		}

		public override bool SupportVoid
		{
			get { return true; }
		}

		protected abstract string GetResourceRootKey();
        
		//protected CustomSecurityHeaderType GetApiCredentials(PayTabsApiSettingsBase settings)
		//{
		//	var customSecurityHeaderType = new CustomSecurityHeaderType();

		//	customSecurityHeaderType.Credentials = new UserIdPasswordType();
		//	customSecurityHeaderType.Credentials.Username = settings.ApiAccountName;
		//	customSecurityHeaderType.Credentials.Password = settings.ApiAccountPassword;
		//	customSecurityHeaderType.Credentials.Signature = settings.Signature;
		//	customSecurityHeaderType.Credentials.Subject = "";

		//	return customSecurityHeaderType;
		//}

		//protected CurrencyCodeType GetApiCurrency(Currency currency)
		//{
		//	var currencyCodeType = CurrencyCodeType.USD;
		//	try
		//	{
		//		currencyCodeType = (CurrencyCodeType)Enum.Parse(typeof(CurrencyCodeType), currency.CurrencyCode, true);
		//	}
		//	catch {	}

		//	return currencyCodeType;
		//}

		protected abstract string GetControllerName();

		/// <summary>
		/// Gets additional handling fee
		/// </summary>
		/// <param name="cart">Shoping cart</param>
		/// <returns>Additional handling fee</returns>
		public override decimal GetAdditionalHandlingFee(IList<OrganizedShoppingCartItem> cart)
        {
			var result = decimal.Zero;
			try
			{
				var settings = Services.Settings.LoadSetting<TSetting>();

				result = this.CalculateAdditionalFee(OrderTotalCalculationService, cart, settings.AdditionalFee, settings.AdditionalFeePercentage);
			}
			catch (Exception)
			{
			}
			return result;
        }
        
        /// <summary>
        /// Cancels a recurring payment
        /// </summary>
        /// <param name="cancelPaymentRequest">Request</param>
        /// <returns>Result</returns>
   //     public override CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest request)
   //     {
   //         var result = new CancelRecurringPaymentResult();
   //         var order = request.Order;
			//var settings = Services.Settings.LoadSetting<TSetting>(order.StoreId);

   //         var req = new ManageRecurringPaymentsProfileStatusReq();
   //         req.ManageRecurringPaymentsProfileStatusRequest = new ManageRecurringPaymentsProfileStatusRequestType();
   //         req.ManageRecurringPaymentsProfileStatusRequest.Version = ApiVersion;
   //         var details = new ManageRecurringPaymentsProfileStatusRequestDetailsType();
   //         req.ManageRecurringPaymentsProfileStatusRequest.ManageRecurringPaymentsProfileStatusRequestDetails = details;

   //         details.Action = StatusChangeActionType.Cancel;
   //         //Recurring payments profile ID returned in the CreateRecurringPaymentsProfile response
   //         details.ProfileID = order.SubscriptionTransactionId;

   //         using (var service = GetApiAaService(settings))
   //         {
   //             var response = service.ManageRecurringPaymentsProfileStatus(req);

   //             string error = "";
   //             if (!IsSuccess(response, out error))
   //             {
   //                 result.AddError(error);
   //             }
   //         }

   //         return result;
   //     }

        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public override void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = GetControllerName();
            routeValues = new RouteValueDictionary() { { "area", "SmartStore.PayTabs" } };
        }

        /// <summary>
        /// Gets a route for payment info
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public override void GetPaymentInfoRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PaymentInfo";
            controllerName = GetControllerName();
            routeValues = new RouteValueDictionary() { { "area", "SmartStore.PayTabs" } };
        }
    }
}

