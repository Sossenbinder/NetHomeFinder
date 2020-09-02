using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetHomeFinder.Common.Module;
using NetHomeFinder.Common.Utils;
using NetHomeFinder.Storage.Module;
using NetHomeFinder.Telegram.Module;
using NetHomeFinder.WebScraper.Module;
using NetHomeFinder.WebScraper.Service.Interface;

namespace NetHomeFinder
{
	internal class Program
	{
		private static async Task Main(string[] args)
		{
			var containerBuilder = new ContainerBuilder();
			RegisterModules(containerBuilder);
			RegisterServices(containerBuilder);

			var serviceProvider = new AutofacServiceProvider(containerBuilder.Build());

			var integrationInitializer = serviceProvider.GetRequiredService<CompositeIntegrationInitializer>();

			await integrationInitializer.Initialize();

			await serviceProvider.GetRequiredService<IScrapingManager>().StartContinuousScraping();

			await Task.Delay(Timeout.Infinite);
		}

		private static void RegisterModules(ContainerBuilder containerBuilder)
		{
			containerBuilder.RegisterModule<WebScrapperModule>();
			containerBuilder.RegisterModule<TelegramModule>();
			containerBuilder.RegisterModule<StorageModule>();
			containerBuilder.RegisterModule<CommonModule>();
		}

		private static void RegisterServices(ContainerBuilder containerBuilder)
		{
			var configuration = new ConfigurationBuilder()
				.AddEnvironmentVariables()
				.Build();

			containerBuilder
				.Register(_ => configuration)
				.As<IConfiguration>();

			containerBuilder.RegisterType<CompositeIntegrationInitializer>()
				.AsSelf()
				.SingleInstance();
		}
	}
}