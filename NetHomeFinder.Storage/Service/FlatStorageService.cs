using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetHomeFinder.Common.Definitions.Models;
using NetHomeFinder.Common.Extensions;
using NetHomeFinder.Storage.Entities;
using NetHomeFinder.Storage.Repository.Interface;
using NetHomeFinder.Storage.Service.Interface;
using NetHomeFinder.Telegram.Service.Interface;

namespace NetHomeFinder.Storage.Service
{
	public class FlatStorageService : IFlatStorageService
	{
		private readonly IScrapedFlatRepository _scrapedFlatRepository;

		private readonly ITelegramFlatService _telegramFlatService;

		public FlatStorageService(
			IScrapedFlatRepository scrapedFlatRepository,
			ITelegramFlatService telegramFlatService)
		{
			_scrapedFlatRepository = scrapedFlatRepository;
			_telegramFlatService = telegramFlatService;
		}

		public async Task HandleScrapedFlats(List<Estate> estates)
		{
			var scrapedFlatEntities = estates
				.ConvertAll(x => new ScrapedFlatEntity()
				{
					InternalEstateId = x.InternalEstateId,
				});

			var unknownFlats = await _scrapedFlatRepository.InsertUnknownFlats(scrapedFlatEntities);

			var unknownEstates = estates
				.Where(x => unknownFlats.Any(y => y.InternalEstateId.Equals(x.InternalEstateId)))
				.ToList();

			$"Found {unknownEstates.Count} unknown flats".LogWithUtcPrefix();

			await _telegramFlatService.SendNewFlatsUpdate(unknownEstates);
		}
	}
}