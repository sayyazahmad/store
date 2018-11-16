using SmartStore.Core.Domain.Orders;
using System.Collections.Generic;

namespace SmartStore.Services.Agent
{
    public partial interface IComissionService
    {
        void InsertComission(Comission request);
        
        IEnumerable<Comission> GetAllComissionByCustomerId(int customerId);

        IEnumerable<Comission> GetAllComissions();

        Comission GetComissionById(int id);

        void UpdateComission(Comission comission);
    }
}
