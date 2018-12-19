namespace SmartStore.Core.Domain.Customers
{
	/// <summary>
	/// Represents the reason for creating a wallet history entry.
	/// </summary>
	public enum WalletPostingReason
	{
		/// <summary>
		/// Any administration reason.
		/// </summary>
		Admin = 0,

		/// <summary>
		/// The Customer has purchased goods which have been paid in part or in full by wallet.
		/// </summary>
		Purchase,

		/// <summary>
		/// The customer has bought wallet credits.
		/// </summary>
		Refill,

		/// <summary>
		/// The admin has refunded the used credit balance.
		/// </summary>
		Refund,

		/// <summary>
		/// The admin has refunded a part of the used credit balance.
		/// </summary>
		PartialRefund,

		/// <summary>
		/// The admin has debited the wallet, e.g. because the purchase of credit was cancelled.
		/// </summary>
		Debit,

        /// <summary>
        /// Commission amount on order placement
        /// </summary>
        Commission,
            
        /// <summary>
        /// Profit amount on order placement
        /// </summary>
        Profit,
            
        /// <summary>
        /// Withdraw Amount
        /// </summary>
        CommissionWithdrawl,
        
        /// <summary>
        /// Withdraw Amount
        /// </summary>
        ProfitWithdrawl,

        CommissionProfitReversal
    }
}
