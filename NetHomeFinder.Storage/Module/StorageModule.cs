using Autofac;
using NetHomeFinder.Storage.Context;
using NetHomeFinder.Storage.Repository;
using NetHomeFinder.Storage.Repository.Interface;
using NetHomeFinder.Storage.Service;
using NetHomeFinder.Storage.Service.Interface;

namespace NetHomeFinder.Storage.Module
{
	public class StorageModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<SqliteDbContextFactory>()
				.AsSelf()
				.SingleInstance();

			builder.RegisterType<ScrapedFlatRepository>()
				.As<IScrapedFlatRepository>()
				.SingleInstance();

			builder.RegisterType<FlatStorageService>()
				.As<IFlatStorageService>()
				.SingleInstance()
				.AutoActivate();
		}
	}
}