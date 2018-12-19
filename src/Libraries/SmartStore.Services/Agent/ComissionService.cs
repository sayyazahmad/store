using SmartStore.Core.Data;
using SmartStore.Core.Domain.Agent;
using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartStore.Services.Agent
{
    public partial class ComissionService : IComissionService
    {
        private readonly IRepository<Comission> _comissionRepository;
        private readonly IRepository<CommissionRequest> _comissionRequestRepository;
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly IDbContext _dbContext;

        public ComissionService(IRepository<Comission> comissionRepository, IRepository<CommissionRequest> comissionRequestRepository, IDbContext dbContext, IRepository<OrderItem> orderItemRepository)
        {
            _comissionRepository = comissionRepository;
            _comissionRequestRepository = comissionRequestRepository;
            _dbContext = dbContext;
            _orderItemRepository = orderItemRepository;
        }

        public IEnumerable<Comission> GetAllComissionByCustomerId(int customerId)
        {
            return _comissionRepository.Get(x => x.CustomerId == customerId);
                
        }

        public IEnumerable<Comission> GetAllComissions()
        {
            throw new NotImplementedException();
        }

        public Comission GetComissionById(int id)
        {
            return _comissionRepository.GetById(id);
        }

        public virtual void InsertComission(Comission comission)
        {
            Guard.NotNull(comission, nameof(comission));
            comission.CreatedOnUtc = DateTime.UtcNow;
            _comissionRepository.Insert(comission);
        }

        public void UpdateComission(Comission comission)
        {

        }

        public IEnumerable<CommissionRequest> GetAllComissionRequests()
        {
            return _comissionRequestRepository.Table.Select(x => x).OrderBy(x => x.CreatedOnUtc).ThenBy(x => x.RequestStatusId);
        }

        public void InsertCommissionRequest(CommissionRequest request)
        {
            Guard.NotNull(request, nameof(request));
            _comissionRequestRepository.Insert(request);
        }

        public IEnumerable<Comission> GetCustomerCommission(int customerId)
        {
            return _comissionRepository.Get(x => x.CustomerId == customerId);
        }

        public IEnumerable<CommissionRequest> GetAllCustomerComissionRequests(int customerId)
        {
            return _comissionRequestRepository.Get(x => x.CustomerId == customerId);
        }

        public void UpdateCommissionRequestStatus(int id, int statusId)
        {
            var request = _comissionRequestRepository.GetById(id);
            //if (statusId == 20)
            //{
            //    var unpaidcomm = _comissionRepository.TableUntracked.Where(x => !x.ComissionPaid).OrderByDescending(x => x.Id);
            //    var amt = request.WithdrawAmount;
            //    unpaidcomm?.ToList().ForEach(x => {
            //        if (amt > 0)
            //        {
            //            if (amt >= x.ComissionAmt)
            //            {
            //                amt = amt - x.ComissionAmt;
            //                x.ComissionPaid = true;
            //                _comissionRepository.Update(x);
            //            }
            //            else
            //            {
            //                x.ComissionAmt = amt;
            //                _comissionRepository.Update(x);
            //                amt = 0;
            //            }
            //        }
            //    });
            //}
            request.RequestStatusId = statusId;
            _comissionRequestRepository.Update(request);
        }

        public CommissionRequest GetCommissionRequestById(int id)
        {
            return _comissionRequestRepository.GetById(id);
        }
    }
}
