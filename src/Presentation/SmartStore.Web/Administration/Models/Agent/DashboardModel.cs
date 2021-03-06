﻿using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Modelling;
using System;

namespace SmartStore.Admin.Models.Agent
{
    public class DashboardModel : EntityModelBase
    {
        [SmartResourceDisplayName("Admin.Agent.Dashboard.LastLogin")]
        public DateTime? LastLogin { get; set; }

        [SmartResourceDisplayName("Admin.Agent.Dashboard.TotalSales")]
        public int? TotalSales { get; set; }

        [SmartResourceDisplayName("Admin.Agent.Dashboard.TotalPoints")]
        public int? TotalPoints { get; set; }

        [SmartResourceDisplayName("Admin.Agent.Dashboard.WalletBalance")]
        public decimal? WalletBalance { get; set; }

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
    }
}