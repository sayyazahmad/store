using SmartStore.Core.Domain.Common;
using System.Collections.Generic;

namespace SmartStore.Services.Agent
{
    public partial interface IBankUpdateRequestService
    {
        void InsertBankUpdateRequest(BankUpdateRequest request);

        void DeleteBankUpdateRequest(BankUpdateRequest request);

        IEnumerable<BankUpdateRequest> GetAllBankUpdateRequestsByCustomerId(int customerId);

        IEnumerable<BankUpdateRequest> GetAllBankUpdateRequests();

        BankUpdateRequest GetBankUpdateRequestById(int id);

        void UpdateBankUpdateRequestStatus(int id, int statusId);
    }
}
