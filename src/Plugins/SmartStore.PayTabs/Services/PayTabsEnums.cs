
namespace SmartStore.PayTabs.Services
{
	public enum PayTabsPaymentInstructionItem
	{
		Reference = 0,
		BankRoutingNumber,
		Bank,
		Bic,
		Iban,
		AccountHolder,
		AccountNumber,
		Amount,
		PaymentDueDate,
		Details
	}

	public enum PayTabsMessage
	{
		Message = 0,
		Event,
		EventId,
		State,
		Amount,
		PaymentId
	}
}