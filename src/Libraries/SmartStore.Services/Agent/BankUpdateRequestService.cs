using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartStore.Core.Data;
using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Customers;

namespace SmartStore.Services.Agent
{
    public partial class BankUpdateRequestService : IBankUpdateRequestService
    {
        private readonly IRepository<BankUpdateRequest> _bankUpdateRequestRepository;
        private readonly IRepository<Customer> _customerRepository;

        public BankUpdateRequestService(IRepository<BankUpdateRequest> bankUpdateRequestRepository, IRepository<Customer> customerRepository)
        {
            _bankUpdateRequestRepository = bankUpdateRequestRepository;
            _customerRepository = customerRepository;
        }

        public virtual void InsertBankUpdateRequest(BankUpdateRequest request)
        {
            Guard.NotNull(request, nameof(request));
            request.CreatedOnUtc = DateTime.UtcNow;
            _bankUpdateRequestRepository.Insert(request);
        }

        public virtual void DeleteBankUpdateRequest(BankUpdateRequest request)
        {
            Guard.NotNull(request, nameof(request));
            _bankUpdateRequestRepository.Delete(request);
        }
        
        public IEnumerable<BankUpdateRequest> GetAllBankUpdateRequestsByCustomerId(int customerId)
        {
            return _bankUpdateRequestRepository.Get(x => x.CustomerId == customerId);
        }

        public IEnumerable<BankUpdateRequest> GetAllBankUpdateRequests()
        {
            return _bankUpdateRequestRepository.Table.Select(x => x);
        }

        public BankUpdateRequest GetBankUpdateRequestById(int id)
        {
            return _bankUpdateRequestRepository.GetById(id);
        }

        public void UpdateBankUpdateRequestStatus(int id, int statusId)
        {
            var request = _bankUpdateRequestRepository.GetById(id);
            if(statusId == 20)
            {
                var customer = _customerRepository.GetById(request.CustomerId);
                customer.BankName = request.BankName;
                customer.IBAN = request.IBAN;
                _customerRepository.Update(customer);
            }
            request.RequestStatusId = statusId;
            _bankUpdateRequestRepository.Update(request);
        }
    }
}
