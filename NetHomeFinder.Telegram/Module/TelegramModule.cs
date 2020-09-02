using Autofac;
using NetHomeFinder.Common.Utils.Interface;
using NetHomeFinder.Telegram.Service;
using NetHomeFinder.Telegram.Service.Interface;
using NetHomeFinder.Telegram.Util;

namespace NetHomeFinder.Telegram.Module
{
	public class TelegramModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder.RegisterType<TelegramClientFactory>()
				.AsSelf()
				.SingleInstance();

			builder.RegisterType<TelegramMessageService>()
				.As<ITelegramMessageService, IIntegrationInitializer>()
				.SingleInstance();

			builder.RegisterType<TelegramFlatService>()
				.As<ITelegramFlatService>()
				.AsSelf();
		}
	}
}