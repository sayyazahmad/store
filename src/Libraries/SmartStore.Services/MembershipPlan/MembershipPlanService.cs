using SmartStore.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartStore.Core.Domain.Membership;

namespace SmartStore.Services.MembershipPlan
{
    public class MembershipPlanService : IMembershipPlanService
    {
        private readonly IRepository<Core.Domain.Membership.MembershipPlan> _membershipPlanRepository;

        public MembershipPlanService(IRepository<Core.Domain.Membership.MembershipPlan> membershipPlanRepository)
        {
            _membershipPlanRepository = membershipPlanRepository;
        }

        public virtual void InsertMembershipPlan(Core.Domain.Membership.MembershipPlan membershipPlan)
        {
            Guard.NotNull(membershipPlan, nameof(membershipPlan));

            _membershipPlanRepository.Insert(membershipPlan);
        }

        public virtual void UpdateMembershipPlan(Core.Domain.Membership.MembershipPlan membershipPlan)
        {
            Guard.NotNull(membershipPlan, nameof(membershipPlan));

            _membershipPlanRepository.Update(membershipPlan);
        }

        public virtual void DeleteMembershipPlan(Core.Domain.Membership.MembershipPlan membershipPlan)
        {
            Guard.NotNull(membershipPlan, nameof(membershipPlan));
            _membershipPlanRepository.Delete(membershipPlan);
        }

        public virtual Core.Domain.Membership.MembershipPlan GetMembershipById(int membershipPlanId)
        {
            if (membershipPlanId == 0)
                return null;

            var customer = _membershipPlanRepository.GetById(membershipPlanId);

            return customer;
        }

        public virtual IEnumerable<Core.Domain.Membership.MembershipPlan> GetAll()
        {
            var customers = _membershipPlanRepository.Get(p => p.Id > 0).OrderBy(p => p.Order);

            return customers;
        }
    }
}
