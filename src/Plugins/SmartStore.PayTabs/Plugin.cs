using System;
using SmartStore.Core.Logging;
using SmartStore.Core.Plugins;
using SmartStore.PayTabs.Services;
using SmartStore.PayTabs.Settings;
using SmartStore.Services.Configuration;
using SmartStore.Services.Localization;

namespace SmartStore.PayTabs
{
	public class Plugin : BasePlugin
	{
		private readonly ISettingService _settingService;
		private readonly ILocalizationService _localizationService;
		private readonly Lazy<IPayTabsService> _PayTabsService;

		public Plugin(
			ISettingService settingService,
			ILocalizationService localizationService,
			Lazy<IPayTabsService> PayTabsService)
		{
			_settingService = settingService;
			_localizationService = localizationService;
			_PayTabsService = PayTabsService;

			Logger = NullLogger.Instance;
		}

		public ILogger Logger { get; set; }

		public static string SystemName
		{
			get { return "SmartStore.PayTabs"; }
		}

		public override void Install()
		{
			_settingService.SaveSetting<PayTabsPayPagePaymentSettings>(new PayTabsPayPagePaymentSettings());

			_localizationService.ImportPluginResourcesFromXml(this.PluginDescriptor);

			base.Install();
		}

		public override void Uninstall()
		{
			try
			{
				var settings = _settingService.LoadSetting<PayTabsPayPagePaymentSettings>();
				if (settings.WebhookId.HasValue())
				{
					var session = new PayTabsSessionData();
					var result = _PayTabsService.Value.EnsureAccessToken(session, settings);

					if (result.Success)
						result = _PayTabsService.Value.DeleteWebhook(settings, session);

					if (!result.Success)
						Logger.Log(LogLevel.Error, null, result.ErrorMessage, null);
				}
			}
			catch (Exception exception)
			{
				Logger.Log(LogLevel.Error, exception, null, null);
			}

            _settingService.DeleteSetting<PayTabsPayPagePaymentSettings>();

			_localizationService.DeleteLocaleStringResources(PluginDescriptor.ResourceRootKey);

			base.Uninstall();
		}
	}
}
