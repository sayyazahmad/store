using Autofac;
using Autofac.Integration.Mvc;
using SmartStore.Core.Infrastructure;
using SmartStore.Core.Infrastructure.DependencyManagement;
using SmartStore.PayTabs.Services;
using SmartStore.Web.Controllers;

namespace SmartStore.PayTabs
{
	public class DependencyRegistrar : IDependencyRegistrar
	{
		public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, bool isActiveModule)
		{
			builder.RegisterType<PayTabsService>().As<IPayTabsService>().InstancePerRequest();

			if (isActiveModule)
			{
				//builder.RegisterType<PayTabsExpressCheckoutFilter>().AsActionFilterFor<CheckoutController>(x => x.PaymentMethod()).InstancePerRequest();

				//builder.RegisterType<PayTabsExpressWidgetZoneFilter>().AsActionFilterFor<ShoppingCartController>(x => x.OffCanvasShoppingCart()).InstancePerRequest();

				//builder.RegisterType<PayTabsPlusCheckoutFilter>()
				//	.AsActionFilterFor<CheckoutController>(x => x.PaymentMethod())
				//	.InstancePerRequest();
			}
		}

		public int Order
		{
			get { return 1; }
		}
	}
}
