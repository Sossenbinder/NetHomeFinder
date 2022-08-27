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
			Environment.SetEnvironmentVariable("TelegramApiToken", "1027531170:AAEk3yzzjVaghnkL_G7MF4eHsckO_KhJJ-s");
			Environment.SetEnvironmentVariable("TelegramChatId", "-1001157116554");
			Environment.SetEnvironmentVariable("SecondsBetweenScrapes", "10800");
			Environment.SetEnvironmentVariable("ImmoscoutUrl", "https://www.immobilienscout24.de/Suche/shape/wohnung-mieten?shape=X3lnaEh5dHZ3QHJ9Q2V1Q3p4QnliSHJxRGFpR3BYYXlMY2VCb3BBcW1CZ3NEX21Gc2xDc2hBcW5CY2JCfmpGc1VqcFd3X0FidUdsdEBsckd8YkF6dEE7aX5uaEh1dGF4QGRCdUxlQmtP&price=-700.0&sorting=2&enteredFrom=result_list");

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