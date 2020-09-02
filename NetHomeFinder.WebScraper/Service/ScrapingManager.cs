using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using NetHomeFinder.Common.Extensions;
using NetHomeFinder.Storage.Service.Interface;
using NetHomeFinder.WebScraper.Service.Interface;
using NetHomeFinder.WebScraper.Service.Scrapers.Interface;

namespace NetHomeFinder.WebScraper.Service
{
	public class ScrapingManager : IScrapingManager
	{
		private readonly IEnumerable<IScrapingService> _scrapingServices;

		private readonly IFlatStorageService _flatStorageService;

		private readonly TimeSpan _timeSpanBetweenScrapes;

		public ScrapingManager(
			IEnumerable<IScrapingService> scrapingServices,
			IFlatStorageService flatStorageService,
			IConfiguration configuration)
		{
			_scrapingServices = scrapingServices;
			_flatStorageService = flatStorageService;

			_timeSpanBetweenScrapes = TimeSpan.FromSeconds(double.Parse(configuration["SecondsBetweenScrapes"]));
		}

		public async Task StartContinuousScraping(CancellationToken? cancellationToken = null)
		{
			var internalCancellationToken = cancellationToken ?? CancellationToken.None;

			while (!internalCancellationToken.IsCancellationRequested)
			{
				await ScrapeData(internalCancellationToken);

				await Task.Delay(_timeSpanBetweenScrapes, internalCancellationToken).IgnoreTaskCancelledException();
			}
		}

		private async Task ScrapeData(CancellationToken cancellationToken)
		{
			"Starting scrape...".LogWithUtcPrefix();

			var scrapeTasks = _scrapingServices
				.Select(x => x.GetData())
				.ToList();

			await Task.WhenAny(Task.WhenAll(scrapeTasks), cancellationToken.AsTask());

			// If the cancellationToken is cancelled, WhenAny() failed because of it
			// and we should return
			if (cancellationToken.IsCancellationRequested)
			{
				return;
			}

			var estates = scrapeTasks
				.SelectMany(x => x.Result)
				.GroupBy(x => x.InternalEstateId)
				.Select(x => x.First())
				.ToList();

			await _flatStorageService.HandleScrapedFlats(estates);
		}
	}
}