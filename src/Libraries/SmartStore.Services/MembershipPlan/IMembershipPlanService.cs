using System.Collections.Generic;
using SmartStore.Core.Domain.Membership;

namespace SmartStore.Services.MembershipPlan
{
    public interface IMembershipPlanService
    {
        void DeleteMembershipPlan(Core.Domain.Membership.MembershipPlan membershipPlan);
        IEnumerable<Core.Domain.Membership.MembershipPlan> GetAll();
        Core.Domain.Membership.MembershipPlan GetMembershipById(int membershipPlanId);
        void InsertMembershipPlan(Core.Domain.Membership.MembershipPlan membershipPlan);
        void UpdateMembershipPlan(Core.Domain.Membership.MembershipPlan membershipPlan);
        Core.Domain.Membership.MembershipPlan GetById(int id);
    }
}