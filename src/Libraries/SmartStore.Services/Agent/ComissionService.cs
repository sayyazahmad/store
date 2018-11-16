using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartStore.Core.Data;
using SmartStore.Core.Domain.Orders;

namespace SmartStore.Services.Agent
{
    public partial class ComissionService : IComissionService
    {
        private readonly IRepository<Comission> _comissionRepository;

        public ComissionService(IRepository<Comission> comissionRepository)
        {
            _comissionRepository = comissionRepository;
        }

        public IEnumerable<Comission> GetAllComissionByCustomerId(int customerId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comission> GetAllComissions()
        {
            throw new NotImplementedException();
        }

        public Comission GetComissionById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual void InsertComission(Comission comission)
        {
            Guard.NotNull(comission, nameof(comission));
            comission.CreatedOnUtc = DateTime.UtcNow;
            _comissionRepository.Insert(comission);
        }

        public void UpdateComission(Comission comission)
        {
            throw new NotImplementedException();
        }
    }
}
