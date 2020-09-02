using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NetHomeFinder.Common.Definitions.Models;
using NetHomeFinder.Common.Extensions;
using NetHomeFinder.Telegram.Service.Interface;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NetHomeFinder.Telegram.Service
{
	internal class TelegramFlatService : ITelegramFlatService
	{
		private readonly ChatId _chatId;

		private readonly ITelegramMessageService _telegramMessageService;

		public TelegramFlatService(
			IConfiguration configuration,
			ITelegramMessageService telegramMessageService)
		{
			_telegramMessageService = telegramMessageService;
			_chatId = long.Parse(configuration["TelegramChatId"]);
		}

		public async Task SendNewFlatsUpdate(List<Estate> newFlats)
		{
			// Yes, this could be sped up with Task.WhenAll, but
			// TG only allows a certain amount of concurrent requests
			// before running into a 429, so an easy way to throttle is
			// doing it like this
			foreach (var flat in newFlats)
			{
				await SendMessage(flat);

				await Task.Delay(2000);
			}
		}

		private async Task SendMessage(Estate flat)
		{
			for (var i = 0; i < 5; ++i)
			{
				try
				{
					await _telegramMessageService.SendMessage(
						_chatId,
						GenerateFlatMessage(flat),
						parseMode: ParseMode.Html);
					return;
				}
				catch (HttpRequestException)
				{
					await Task.Delay(5000);
				}
			}

			$"Failed to send notification for {flat.InternalEstateId}".LogWithUtcPrefix();
		}

		private string GenerateFlatMessage(Estate estate)
		{
			return "<b><u>🏠 New flat found 🏠</u></b>\n\n" +
				   $"{estate.Name}\n\n" +
				   $"Address: {estate.Address}\n\n" +
				   $"Rooms: {estate.Rooms}\n\n" +
				   $"Size: {estate.SquareMeters} m²\n\n" +
				   $"Price: {estate.Price} €\n\n\n" +
				   $"<a>{estate.Url}</a>";
		}
	}
}