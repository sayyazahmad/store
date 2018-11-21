using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Domain.Payments;
using SmartStore.Core.Domain.Stores;
using SmartStore.PayTabs.Settings;
using SmartStore.Services.Payments;

namespace SmartStore.PayTabs.Services
{
	public interface IPayTabsService
	{
		void AddOrderNote(PayTabsSettingsBase settings, Order order, string anyString, bool isIpn = false);

		PayTabsPaymentInstruction ParsePaymentInstruction(dynamic json);

		string CreatePaymentInstruction(PayTabsPaymentInstruction instruct);

		PaymentStatus GetPaymentStatus(string state, string reasonCode, PaymentStatus defaultStatus);

		PayTabsResponse CallApi(string method, string path, string accessToken, PayTabsApiSettingsBase settings, string data);

        PayTabsResponse EnsureAccessToken(PayTabsSessionData session, PayTabsApiSettingsBase settings);

        PayTabsResponse GetPayment(PayTabsApiSettingsBase settings, PayTabsSessionData session);

		PayTabsResponse CreatePayment(
			PayTabsApiSettingsBase settings,
			PayTabsSessionData session,
			List<OrganizedShoppingCartItem> cart,
			string providerSystemName,
			string returnUrl,
			string cancelUrl);

		PayTabsResponse PatchShipping(
			PayTabsApiSettingsBase settings,
			PayTabsSessionData session,
			List<OrganizedShoppingCartItem> cart,
			string providerSystemName);

		PayTabsResponse ExecutePayment(PayTabsApiSettingsBase settings, PayTabsSessionData session);

		PayTabsResponse Refund(PayTabsApiSettingsBase settings, PayTabsSessionData session, RefundPaymentRequest request);

		PayTabsResponse Capture(PayTabsApiSettingsBase settings, PayTabsSessionData session, CapturePaymentRequest request);

		PayTabsResponse Void(PayTabsApiSettingsBase settings, PayTabsSessionData session, VoidPaymentRequest request);

		PayTabsResponse CreateWebhook(PayTabsApiSettingsBase settings, PayTabsSessionData session, string url);

		PayTabsResponse DeleteWebhook(PayTabsApiSettingsBase settings, PayTabsSessionData session);

		HttpStatusCode ProcessWebhook(
			PayTabsApiSettingsBase settings,
			NameValueCollection headers,
			string rawJson,
			string providerSystemName);
	}
}