using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using SmartStore.Core.Domain.Payments;
using SmartStore.Core.Logging;
using SmartStore.PayTabs.Models;
using SmartStore.PayTabs.Settings;
using SmartStore.Services.Orders;
using SmartStore.Services.Payments;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Security;
using SmartStore.Web.Framework.Settings;

namespace SmartStore.PayTabs.Controllers
{
    public class PayTabsPayPageController : PayTabsControllerBase<PayTabsPayPagePaymentSettings>
    {
        private readonly HttpContextBase _httpContext;

        public PayTabsPayPageController(
            IPaymentService paymentService,
            IOrderService orderService,
            IOrderProcessingService orderProcessingService,
            PaymentSettings paymentSettings,
            HttpContextBase httpContext) : base(
                PayTabsPayPageProvider.SystemName,
                paymentService,
                orderService,
                orderProcessingService)
        {
            _httpContext = httpContext;
        }

        [AdminAuthorize, ChildActionOnly, LoadSetting]
        public ActionResult Configure(PayTabsPayPagePaymentSettings settings, int storeScope)
        {
            var model = new PayTabsPayPageConfigurationModel();
            model.Copy(settings, true);
            PrepareConfigurationModel(model, storeScope);
            return View(model);
        }

        [HttpPost, AdminAuthorize, ChildActionOnly]
        public ActionResult Configure(PayTabsPayPageConfigurationModel model, FormCollection form)
        {
            var storeDependingSettingHelper = new StoreDependingSettingHelper(ViewData);
            var storeScope = this.GetActiveStoreScopeConfiguration(Services.StoreService, Services.WorkContext);
            var settings = Services.Settings.LoadSetting<PayTabsPayPagePaymentSettings>(storeScope);

            if (!ModelState.IsValid)
            {
                return Configure(settings, storeScope);
            }

            ModelState.Clear();
            model.Copy(settings, false);

            using (Services.Settings.BeginScope())
            {
                storeDependingSettingHelper.UpdateSettings(settings, form, storeScope, Services.Settings);
            }
            NotifySuccess(T("Admin.Common.DataSuccessfullySaved"));

            return RedirectToConfiguration(PayTabsPayPageProvider.SystemName, false);
        }

        public ActionResult PaymentInfo()
        {
            return PartialView();
        }

        [NonAction]
        public override ProcessPaymentRequest GetPaymentInfo(FormCollection form)
        {
            return new ProcessPaymentRequest();
        }

        [NonAction]
        public override IList<string> ValidatePaymentForm(FormCollection form)
        {
            var warnings = new List<string>();
            return warnings;
        }

        [ValidateInput(false)]
        public ActionResult VerifyPayment(FormCollection form)
        {
            var res = Verify(Request["payment_reference"]);
            dynamic data = JObject.Parse(res);
            if (data.response_code == "100")
            {
                Guid orderNo = Guid.Empty;
                try
                {
                    orderNo = new Guid(data.reference_no);
                }
                catch { }
                var order = OrderService.GetOrderByGuid(orderNo);
                if(order != null)
                {
                    if (order.AuthorizationTransactionId.IsEmpty())
                    {
                        order.AuthorizationTransactionId = order.CaptureTransactionId = data.transaction_id;
                        order.AuthorizationTransactionResult = order.CaptureTransactionResult = "Success";
                        OrderService.UpdateOrder(order);
                    }
                    if (OrderProcessingService.CanMarkOrderAsPaid(order))
                    {
                        OrderProcessingService.MarkOrderAsPaid(order);
                    }
                }
                return RedirectToAction("Completed", "Checkout", new { area = "" });
            }
            else
            {
                try
                {
                    Guid orderNo = Guid.Empty;
                    try
                    {
                        orderNo = new Guid((string) data.reference_no);
                    }
                    catch { }
                    var order = OrderService.GetOrderByGuid(orderNo);
                    NotifyError((string) data.result);
                    return RedirectToAction("Details", "Order", new { id = order.Id, area = "" });
                }
                catch {}
            }
            return RedirectToAction("Completed", "Checkout", new { area = "" });
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
        
        private string Verify(string referenceId)
        {
            var settings = Services.Settings.LoadSetting<PayTabsPayPagePaymentSettings>();
            var request = $"merchant_email={settings.MerchantEmail}"
                        + $"&secret_key={settings.APIKey}"
                        + $"&payment_reference={referenceId}";
            return CreateWebRequest(settings.PaytabsVerifyPaymentAPIUrl, request);
        }

        [ValidateInput(false)]
        public ActionResult VerifyPaymentt(FormCollection form)
        {
            Dictionary<string, string> values;
            var tx = Services.WebHelper.QueryString<string>("tx");
            var utcNow = DateTime.UtcNow;
            var orderNumberGuid = Guid.Empty;
            var orderNumber = string.Empty;
            var total = decimal.Zero;
            string response;

            var provider = PaymentService.LoadPaymentMethodBySystemName(SystemName, true);
            var processor = (provider != null ? provider.Value as PayTabsPayPageProvider : null);
            if (processor == null)
            {
                Logger.Warn(null, T("Plugins.Payments.PayPal.NoModuleLoading", "PDTHandler"));
                return RedirectToAction("Completed", "Checkout", new { area = "" });
            }

            var settings = Services.Settings.LoadSetting<PayTabsPayPagePaymentSettings>();

            return RedirectToAction("Completed", "Checkout", new { area = "" });

            //if (processor.GetPDTDetails(tx, settings, out values, out response))
            //{
            //    values.TryGetValue("custom", out orderNumber);

            //    try
            //    {
            //        orderNumberGuid = new Guid(orderNumber);
            //    }
            //    catch { }

            //    var order = OrderService.GetOrderByGuid(orderNumberGuid);

            //    if (order != null)
            //    {
            //        try
            //        {
            //            total = decimal.Parse(values["mc_gross"], new CultureInfo("en-US"));
            //        }
            //        catch (Exception ex)
            //        {
            //            Logger.Error(ex, T("Plugins.Payments.PayPalStandard.FailedGetGross"));
            //        }

            //        string payer_status = string.Empty;
            //        values.TryGetValue("payer_status", out payer_status);
            //        string payment_status = string.Empty;
            //        values.TryGetValue("payment_status", out payment_status);
            //        string pending_reason = string.Empty;
            //        values.TryGetValue("pending_reason", out pending_reason);
            //        string mc_currency = string.Empty;
            //        values.TryGetValue("mc_currency", out mc_currency);
            //        string txn_id = string.Empty;
            //        values.TryGetValue("txn_id", out txn_id);
            //        string payment_type = string.Empty;
            //        values.TryGetValue("payment_type", out payment_type);
            //        string payer_id = string.Empty;
            //        values.TryGetValue("payer_id", out payer_id);
            //        string receiver_id = string.Empty;
            //        values.TryGetValue("receiver_id", out receiver_id);
            //        string invoice = string.Empty;
            //        values.TryGetValue("invoice", out invoice);
            //        string payment_fee = string.Empty;
            //        values.TryGetValue("payment_fee", out payment_fee);

            //        var paymentNote = T("Plugins.Payments.PayPalStandard.PaymentNote",
            //            total, mc_currency, payer_status, payment_status, pending_reason, txn_id, payment_type, payer_id, receiver_id, invoice, payment_fee);

            //        OrderService.AddOrderNote(order, paymentNote);

            //        // validate order total... you may get differences if settings.PassProductNamesAndTotals is true
            //        //if (settings.PdtValidateOrderTotal)
            //        //{
            //        //    var roundedTotal = Math.Round(total, 2);
            //        //    var roundedOrderTotal = Math.Round(order.OrderTotal, 2);
            //        //    var roundedDifference = Math.Abs(roundedTotal - roundedOrderTotal);

            //        //    if (!roundedTotal.Equals(roundedOrderTotal))
            //        //    {
            //        //        var message = T("Plugins.Payments.PayPalStandard.UnequalTotalOrder",
            //        //            total, roundedOrderTotal.FormatInvariant(), order.OrderTotal, roundedDifference.FormatInvariant());

            //        //        if (settings.PdtValidateOnlyWarn)
            //        //        {
            //        //            OrderService.AddOrderNote(order, message);
            //        //        }
            //        //        else
            //        //        {
            //        //            Logger.Error(message);

            //        //            return RedirectToAction("Index", "Home", new { area = "" });
            //        //        }
            //        //    }
            //        //}

            //        // mark order as paid
            //        var newPaymentStatus = GetPaymentStatus(payment_status, pending_reason, total, order.OrderTotal);

            //        if (newPaymentStatus == PaymentStatus.Paid)
            //        {
            //            // note, order can be marked as paid through IPN
            //            if (order.AuthorizationTransactionId.IsEmpty())
            //            {
            //                order.AuthorizationTransactionId = order.CaptureTransactionId = txn_id;
            //                order.AuthorizationTransactionResult = order.CaptureTransactionResult = "Success";

            //                OrderService.UpdateOrder(order);
            //            }

            //            if (OrderProcessingService.CanMarkOrderAsPaid(order))
            //            {
            //                OrderProcessingService.MarkOrderAsPaid(order);
            //            }
            //        }
            //    }

            //    return RedirectToAction("Completed", "Checkout", new { area = "" });
            //}
            //else
            //{
            //    try
            //    {
            //        values.TryGetValue("custom", out orderNumber);
            //        orderNumberGuid = new Guid(orderNumber);

            //        var order = OrderService.GetOrderByGuid(orderNumberGuid);
            //        OrderService.AddOrderNote(order, "{0} {1}".FormatInvariant(T("Plugins.Payments.PayPalStandard.PdtFailed"), response));
            //    }
            //    catch { }

            //    return RedirectToAction("Index", "Home", new { area = "" });
            //}
        }

    }
}