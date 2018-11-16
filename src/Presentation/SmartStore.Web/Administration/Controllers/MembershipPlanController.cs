using SmartStore.Admin.Models.Membership;
using SmartStore.Core.Logging;
using SmartStore.Services.Localization;
using SmartStore.Services.MembershipPlan;
using SmartStore.Services.Security;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Filters;
using SmartStore.Web.Framework.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    [AdminAuthorize]
    public class MembershipPlanController : AdminControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly IMembershipPlanService _membershipPlanService;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerActivityService _customerActivityService;

        public MembershipPlanController(IPermissionService permissionService, IMembershipPlanService membershipPlanService, 
            ILocalizationService localizationService, ICustomerActivityService customerActivityService)
        {
            _permissionService = permissionService;
            _membershipPlanService = membershipPlanService;
            _localizationService = localizationService;
            _customerActivityService = customerActivityService;
        }
        
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                return AccessDeniedView();
            }
            
            var all = _membershipPlanService.GetAll();
            var listModel = new List<MembershipPlanModel>();
            foreach (var item in all)
            {
                listModel.Add(item.ToModel());
            }

            var gridModel = new GridModel<MembershipPlanModel>
            {
                Data = listModel,
                Total = listModel.Count()
            };

            return View(gridModel);
        }


        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            var model = new MembershipPlanModel();
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(MembershipPlanModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var membershipPlan = model.ToEntity();
                _membershipPlanService.InsertMembershipPlan(membershipPlan);

                //activity log
                _customerActivityService.InsertActivity("AddNewMembershipPlan", _localizationService.GetResource("ActivityLog.AddNewMembershipPlan"), membershipPlan.Title);

                NotifySuccess(_localizationService.GetResource("Admin.MembershipPlan.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = membershipPlan.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            var membershipPlan = _membershipPlanService.GetMembershipById(id);
            if (membershipPlan == null)
                //No customer role found with the specified id
                return RedirectToAction("List");

            var model = membershipPlan.ToModel();

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(MembershipPlanModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomerRoles))
                return AccessDeniedView();

            var membershipPlan = _membershipPlanService.GetMembershipById(model.Id);
            if (membershipPlan == null)
                // No customer role found with the specified id
                return RedirectToAction("List");

            try
            {
                if (ModelState.IsValid)
                {
                    //if (customerRole.IsSystemRole && !model.Active)
                    //    throw new SmartException(_localizationService.GetResource("Admin.Customers.CustomerRoles.Fields.Active.CantEditSystem"));

                    //if (customerRole.IsSystemRole && !customerRole.SystemName.Equals(model.SystemName, StringComparison.InvariantCultureIgnoreCase))
                    //    throw new SmartException(_localizationService.GetResource("Admin.Customers.CustomerRoles.Fields.SystemName.CantEditSystem"));

                    membershipPlan = model.ToEntity(membershipPlan);
                    _membershipPlanService.UpdateMembershipPlan(membershipPlan);

                    _customerActivityService.InsertActivity("EditMembershipPlan", _localizationService.GetResource("ActivityLog.EditMembershipPlan"), membershipPlan.Title);

                    NotifySuccess(_localizationService.GetResource("Admin.MembershipPlan.Updated"));
                    return continueEditing ? RedirectToAction("Edit", membershipPlan.Id) : RedirectToAction("List");
                }

                return View(model);
            }
            catch (Exception exc)
            {
                NotifyError(exc);
                return RedirectToAction("Edit", new { id = membershipPlan.Id });
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            var membershipPlan = _membershipPlanService.GetMembershipById(id);
            if (membershipPlan == null)
                return RedirectToAction("List");

            try
            {
                _membershipPlanService.DeleteMembershipPlan(membershipPlan);

                //activity log
                _customerActivityService.InsertActivity("DeleteMembershipPlan", _localizationService.GetResource("ActivityLog.DeleteMembershipPlan"), membershipPlan.Title);

                NotifySuccess(_localizationService.GetResource("Admin.MembershipPlan.Deleted"));
                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                NotifyError(exc.Message);
                return RedirectToAction("Edit", new { id = membershipPlan.Id });
            }

        }
    }
}