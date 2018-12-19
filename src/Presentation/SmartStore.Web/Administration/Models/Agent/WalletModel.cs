using SmartStore.Admin.Models.Customers;
using SmartStore.Web.Framework;
using System.Collections.Generic;

namespace SmartStore.Admin.Models.Agent
{
    public class WalletModel
    {
        [SmartResourceDisplayName("Admin.Agent.Wallet.LifeTimeCommisison ")]
        public decimal LifeTimeCommisison { get; set; }

        [SmartResourceDisplayName("Admin.Agent.Wallet.LifeTimeProfit")]
        public decimal LifeTimeProfit { get; set; }

        [SmartResourceDisplayName("Admin.Agent.Wallet.CurrentCommission")]
        public decimal CurrentCommission { get; set; }

        [SmartResourceDisplayName("Admin.Agent.Wallet.CurrentProfit")]
        public decimal CurrentProfit { get; set; }

        [SmartResourceDisplayName("Admin.Agent.Wallet.Total")]
        public decimal TotalLifeTime { get { return LifeTimeCommisison + LifeTimeProfit; } }

        [SmartResourceDisplayName("Admin.Agent.Wallet.Total")]
        public decimal TotalCurrent { get { return CurrentCommission + CurrentProfit; } }
        public List<ComissionViewModel> Data { get; set; }
        public List<CustomerModel.WalletHistoryModel> CustomerWallet { get; set; }

        public string WalletBalance { get; set; }
    }
}