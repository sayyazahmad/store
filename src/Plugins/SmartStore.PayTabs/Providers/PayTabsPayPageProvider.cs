using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Routing;
using Newtonsoft.Json.Linq;
using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Domain.Payments;
using SmartStore.Core.Domain.Shipping;
using SmartStore.Core.Logging;
using SmartStore.Core.Plugins;
using SmartStore.PayTabs.Controllers;
using SmartStore.PayTabs.Settings;
using SmartStore.Services;
using SmartStore.Services.Localization;
using SmartStore.Services.Orders;
using SmartStore.Services.Payments;

namespace SmartStore.PayTabs
{
    [SystemName("Payments.PayTabsPayPage")]
    [FriendlyName("PayTabs PayPage")]
    [DisplayOrder(1)]
    public partial class PayTabsPayPageProvider : PaymentPluginBase, IConfigurable
    {
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly ICommonServices _services;
        private readonly ILogger _logger;

        public PayTabsPayPageProvider(
            IOrderTotalCalculationService orderTotalCalculationService,
            ICommonServices services,
            ILogger logger)
        {
            _orderTotalCalculationService = orderTotalCalculationService;
            _services = services;
            _logger = logger;
        }

        public static string SystemName { get { return "Payments.PayTabsPayPage"; } }

        public override PaymentMethodType PaymentMethodType
        {
            get
            {
                return PaymentMethodType.Redirection;
            }
        }

        /// <summary>
        /// Process a payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>Process payment result</returns>
        public override ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var result = new ProcessPaymentResult();
            result.NewPaymentStatus = PaymentStatus.Pending;

            var settings = _services.Settings.LoadSetting<PayTabsPayPagePaymentSettings>(processPaymentRequest.StoreId);

            if (settings.MerchantEmail.IsEmpty() || settings.APIKey.IsEmpty())
            {
                result.AddError(T("Plugins.Payments.PayTabsPayPage.InvalidCredentials"));
            }

            return result;
        }

        /// <summary>
        /// Post process payment (used by payment gateways that require redirecting to a third-party URL)
        /// </summary>
        /// <param name="postProcessPaymentRequest">Payment info required for an order processing</param>
        public override void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            if (postProcessPaymentRequest.Order.PaymentStatus == PaymentStatus.Paid)
                return;

            var store = _services.StoreService.GetStoreById(postProcessPaymentRequest.Order.StoreId);
            var settings = _services.Settings.LoadSetting<PayTabsPayPagePaymentSettings>(postProcessPaymentRequest.Order.StoreId);
            var returnUrl = _services.WebHelper.GetStoreLocation(store.SslEnabled) + "Plugins/SmartStore.PayTabs/PayTabsPayPage/VerifyPayment";

            var builder = new StringBuilder();
            builder.Append($"merchant_email={settings.MerchantEmail}");
            builder.Append($"&secret_key={settings.APIKey}");
            builder.Append($"&currency={store.PrimaryStoreCurrency.CurrencyCode}");
            builder.Append($"&amount={postProcessPaymentRequest.Order.OrderTotal}");
            builder.Append($"&title={store.Name} Payment");
            builder.Append($"&quantity={postProcessPaymentRequest.Order.OrderItems.Sum(x => x.Quantity)}");
            builder.Append($"&unit_price={postProcessPaymentRequest.Order.OrderTotal}");
            builder.Append($"&products_per_title={store.Name} Payment");
            builder.Append($"&return_url={returnUrl}");
            builder.Append($"&cc_first_name={postProcessPaymentRequest.Order.Customer.FirstName}");
            builder.Append($"&cc_last_name={postProcessPaymentRequest.Order.Customer.LastName}");
            builder.Append($"&cc_phone_number=00000");
            builder.Append($"&phone_number=00000");
            builder.Append($"&billing_address={postProcessPaymentRequest.Order.BillingAddress.Address1} {postProcessPaymentRequest.Order.BillingAddress.Address2}");
            builder.Append($"&city={postProcessPaymentRequest.Order.BillingAddress.City}");
            builder.Append($"&state={postProcessPaymentRequest.Order.BillingAddress.StateProvince}");
            builder.Append($"&postal_code={postProcessPaymentRequest.Order.BillingAddress.ZipPostalCode}");
            builder.Append($"&country=SAU");
            builder.Append($"&email={postProcessPaymentRequest.Order.Customer.Email}");
            builder.Append($"&ip_customer=000");
            builder.Append($"&ip_merchant={GetIPAddress()}");
            builder.Append($"&address_shipping={postProcessPaymentRequest.Order.ShippingAddress.Address1} {postProcessPaymentRequest.Order.ShippingAddress.Address2}");
            builder.Append($"&city_shipping={postProcessPaymentRequest.Order.ShippingAddress.City}");
            builder.Append($"&state_shipping={postProcessPaymentRequest.Order.ShippingAddress.StateProvince}");
            builder.Append($"&postal_code_shipping={postProcessPaymentRequest.Order.ShippingAddress.ZipPostalCode}");
            builder.Append($"&postal_code_shipping={postProcessPaymentRequest.Order.ShippingAddress.ZipPostalCode}");
            builder.Append($"&country_shipping=SAU");
            builder.Append($"&other_charges=0");
            builder.Append($"&discount=0");
            builder.Append($"&&reference_no={postProcessPaymentRequest.Order.OrderGuid}");
            builder.Append($"&msg_lang=English");
            builder.Append($"&cms_with_version=API");
            
            

            var res = CreateWebRequest(settings.PayPageAPIUrl, builder.ToString());
            if (res.Contains("WebException"))
            {
                _logger.Error("Could not create SSL/TLS secure channel");
            }
            else
            {
                dynamic data = JObject.Parse(res);
                if (data.response_code == "4012")
                {
                    postProcessPaymentRequest.RedirectUrl = data.payment_url;
                }
                else
                {
                    string msg = data.result;
                    _logger.Error(msg);
                }
            }
        }

        private string CreateWebRequest(string url, string request)
        {
            HttpWebRequest req;
            byte[] byteArray;
            WebResponse response;
            StreamReader reader;
            Stream dataStream;
            string res;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            try
            {
                req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                byteArray = Encoding.UTF8.GetBytes(request);
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = byteArray.Length;
                dataStream = req.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                response = req.GetResponse();
                dataStream = response.GetResponseStream();
                reader = new StreamReader(dataStream);
                res = HttpUtility.UrlDecode(reader.ReadToEnd());
                reader.Close();
                dataStream.Close();
                response.Close();
                return res;
            }
            catch (WebException ex)
            {
                return ex.ToString();
            }
        }

        private string GetIPAddress()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[1];
            return ipAddress.ToString();
        }
        /// <summary>
        /// Gets a value indicating whether customers can complete a payment after order is placed but not completed (for redirection payment methods)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Result</returns>
        public override bool CanRePostProcessPayment(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            if (order.PaymentStatus == PaymentStatus.Pending && (DateTime.UtcNow - order.CreatedOnUtc).TotalSeconds > 5)
            {
                return true;
            }
            return true;
        }

        public override Type GetControllerType()
        {
            return typeof(PayTabsPayPageController);
        }

        public override decimal GetAdditionalHandlingFee(IList<OrganizedShoppingCartItem> cart)
        {
            var result = decimal.Zero;
            try
            {
                var settings = _services.Settings.LoadSetting<PayTabsPayPagePaymentSettings>(_services.StoreContext.CurrentStore.Id);

                result = this.CalculateAdditionalFee(_orderTotalCalculationService, cart, settings.AdditionalFee, settings.AdditionalFeePercentage);
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// Gets PDT details
        /// </summary>
        /// <param name="tx">TX</param>
        /// <param name="values">Values</param>
        /// <param name="response">Response</param>
        /// <returns>Result</returns>
        //public bool GetPDTDetails(string tx, PayTabsPayPagePaymentSettings settings, out Dictionary<string, string> values, out string response)
        //{
        //    var request = (HttpWebRequest)WebRequest.Create(settings.GetPayTabsUrl());
        //    request.Method = "POST";
        //    request.ContentType = "application/x-www-form-urlencoded";

        //    var formContent = string.Format("cmd=_notify-synch&at={0}&tx={1}", settings.PdtToken, tx);
        //    request.ContentLength = formContent.Length;

        //    using (var sw = new StreamWriter(request.GetRequestStream(), Encoding.ASCII))
        //    {
        //        sw.Write(formContent);
        //    }

        //    response = null;
        //    using (var sr = new StreamReader(request.GetResponse().GetResponseStream()))
        //    {
        //        response = HttpUtility.UrlDecode(sr.ReadToEnd());
        //    }

        //    values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        //    var firstLine = true;
        //    var success = false;

        //    foreach (string l in response.Split('\n'))
        //    {
        //        string line = l.Trim();
        //        if (firstLine)
        //        {
        //            success = line.Equals("SUCCESS", StringComparison.OrdinalIgnoreCase);
        //            firstLine = false;
        //        }
        //        else
        //        {
        //            int equalPox = line.IndexOf('=');
        //            if (equalPox >= 0)
        //                values.Add(line.Substring(0, equalPox), line.Substring(equalPox + 1));
        //        }
        //    }

        //    return success;
        //}

        /// <summary>
        /// Splits the difference of two value into a portion value (for each item) and a rest value
        /// </summary>
        /// <param name="difference">The difference value</param>
        /// <param name="numberOfLines">Number of lines\items to split the difference</param>
        /// <param name="portion">Portion value</param>
        /// <param name="rest">Rest value</param>
        private void SplitDifference(decimal difference, int numberOfLines, out decimal portion, out decimal rest)
        {
            portion = rest = decimal.Zero;

            if (numberOfLines == 0)
                numberOfLines = 1;

            int intDifference = (int)(difference * 100);
            int intPortion = (int)Math.Truncate((double)intDifference / (double)numberOfLines);
            int intRest = intDifference % numberOfLines;

            portion = Math.Round(((decimal)intPortion) / 100, 2);
            rest = Math.Round(((decimal)intRest) / 100, 2);

            Debug.Assert(difference == ((numberOfLines * portion) + rest));
        }

        /// <summary>
        /// Get all PayTabs line items
        /// </summary>
        /// <param name="postProcessPaymentRequest">Post process paymenmt request object</param>
        /// <param name="checkoutAttributeValues">List with checkout attribute values</param>
        /// <param name="cartTotal">Receives the calculated cart total amount</param>
        /// <returns>All items for PayTabs Standard API</returns>
        public List<PayTabsLineItem> GetLineItems(PostProcessPaymentRequest postProcessPaymentRequest, out decimal cartTotal)
        {
            cartTotal = decimal.Zero;

            var order = postProcessPaymentRequest.Order;
            var lst = new List<PayTabsLineItem>();

            // Order items... checkout attributes are included in order total
            foreach (var orderItem in order.OrderItems)
            {
                var item = new PayTabsLineItem
                {
                    Type = PayTabsItemType.CartItem,
                    Name = orderItem.Product.GetLocalized(x => x.Name),
                    Quantity = orderItem.Quantity,
                    Amount = orderItem.UnitPriceExclTax
                };
                lst.Add(item);

                cartTotal += orderItem.PriceExclTax;
            }

            // Shipping
            if (order.OrderShippingExclTax > decimal.Zero)
            {
                var item = new PayTabsLineItem
                {
                    Type = PayTabsItemType.Shipping,
                    Name = T("Plugins.Payments.PayTabsPayPage.ShippingFee").Text,
                    Quantity = 1,
                    Amount = order.OrderShippingExclTax
                };
                lst.Add(item);

                cartTotal += order.OrderShippingExclTax;
            }

            // Payment fee
            if (order.PaymentMethodAdditionalFeeExclTax > decimal.Zero)
            {
                var item = new PayTabsLineItem
                {
                    Type = PayTabsItemType.PaymentFee,
                    Name = T("Plugins.Payments.PayTabs.PaymentMethodFee").Text,
                    Quantity = 1,
                    Amount = order.PaymentMethodAdditionalFeeExclTax
                };
                lst.Add(item);

                cartTotal += order.PaymentMethodAdditionalFeeExclTax;
            }

            // Tax
            if (order.OrderTax > decimal.Zero)
            {
                var item = new PayTabsLineItem
                {
                    Type = PayTabsItemType.Tax,
                    Name = T("Plugins.Payments.PayTabsPayPage.SalesTax").Text,
                    Quantity = 1,
                    Amount = order.OrderTax
                };
                lst.Add(item);

                cartTotal += order.OrderTax;
            }

            return lst;
        }

        /// <summary>
        /// Manually adjusts the net prices for cart items to avoid rounding differences with the PayTabs API.
        /// </summary>
        /// <param name="PayTabsItems">PayTabs line items</param>
        /// <param name="postProcessPaymentRequest">Post process paymenmt request object</param>
        /// <remarks>
        /// In detail: We add what we have thrown away in the checkout when we rounded prices to two decimal places.
        /// It's a workaround. Better solution would be to store the thrown away decimal places for each OrderItem in the database.
        /// More details: http://magento.xonu.de/magento-extensions/empfehlungen/magento-PayTabs-rounding-error-fix/
        /// </remarks>
        public void AdjustLineItemAmounts(List<PayTabsLineItem> PayTabsItems, PostProcessPaymentRequest postProcessPaymentRequest)
        {
            try
            {
                var cartItems = PayTabsItems.Where(x => x.Type == PayTabsItemType.CartItem);

                if (cartItems.Count() <= 0)
                    return;

                decimal totalSmartStore = Math.Round(postProcessPaymentRequest.Order.OrderSubtotalExclTax, 2);
                decimal totalPayTabs = decimal.Zero;
                decimal delta, portion, rest;

                // calculate what PayTabs calculates
                cartItems.Each(x => totalPayTabs += (x.AmountRounded * x.Quantity));
                totalPayTabs = Math.Round(totalPayTabs, 2, MidpointRounding.AwayFromZero);

                // calculate difference
                delta = Math.Round(totalSmartStore - totalPayTabs, 2);
                //"SM: {0}, PP: {1}, delta: {2}".FormatInvariant(totalSmartStore, totalPayTabs, delta).Dump();

                if (delta == decimal.Zero)
                    return;

                // prepare lines... only lines with quantity = 1 are adjustable. if there is no one, create one.
                if (!cartItems.Any(x => x.Quantity == 1))
                {
                    var item = cartItems.First(x => x.Quantity > 1);
                    item.Quantity -= 1;
                    var newItem = item.Clone();
                    newItem.Quantity = 1;
                    PayTabsItems.Insert(PayTabsItems.IndexOf(item) + 1, newItem);
                }

                var cartItemsOneQuantity = PayTabsItems.Where(x => x.Type == PayTabsItemType.CartItem && x.Quantity == 1);
                Debug.Assert(cartItemsOneQuantity.Count() > 0);

                SplitDifference(delta, cartItemsOneQuantity.Count(), out portion, out rest);

                if (portion != decimal.Zero)
                {
                    cartItems
                        .Where(x => x.Quantity == 1)
                        .Each(x => x.Amount = x.Amount + portion);
                }

                if (rest != decimal.Zero)
                {
                    var restItem = cartItems.First(x => x.Quantity == 1);
                    restItem.Amount = restItem.Amount + rest;
                }

                //"SM: {0}, PP: {1}, delta: {2} (portion: {3}, rest: {4})".FormatInvariant(totalSmartStore, totalPayTabs, delta, portion, rest).Dump();
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public override void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "PayTabsPayPage";
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
            controllerName = "PayTabsPayPage";
            routeValues = new RouteValueDictionary() { { "area", "SmartStore.PayTabs" } };
        }
    }


    public class PayTabsLineItem : ICloneable<PayTabsLineItem>
    {
        public PayTabsItemType Type { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }

        public decimal AmountRounded
        {
            get
            {
                return Math.Round(Amount, 2);
            }
        }

        public PayTabsLineItem Clone()
        {
            var item = new PayTabsLineItem
            {
                Type = this.Type,
                Name = this.Name,
                Quantity = this.Quantity,
                Amount = this.Amount
            };
            return item;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }

    public enum PayTabsItemType
    {
        CartItem = 0,
        Shipping,
        PaymentFee,
        Tax
    }
}
