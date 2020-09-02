using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NetHomeFinder.Common.Extensions;
using NetHomeFinder.Common.Utils.Interface;
using NetHomeFinder.Telegram.Service.Interface;
using NetHomeFinder.Telegram.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace NetHomeFinder.Telegram.Service
{
	public class TelegramMessageService : ITelegramMessageService, IIntegrationInitializer
	{
		private readonly TelegramBotClient _telegramBotClient;

		public TelegramMessageService(TelegramClientFactory telegramClientFactory)
		{
			_telegramBotClient = telegramClientFactory.Get();
		}

		public async Task Initialize()
		{
			var updates = await _telegramBotClient.GetUpdatesAsync();

			if (updates.Length > 0)
			{
				await _telegramBotClient.GetUpdatesAsync(updates.Last().Id + 1);
			}

			"Starting to receive input...".LogWithUtcPrefix();
			_telegramBotClient.StartReceiving();
		}

		public Task DeleteMessage(ChatId chatId, int messageId)
		{
			return _telegramBotClient.DeleteMessageAsync(chatId, messageId);
		}

		public Task<Message> SendPhoto(ChatId chatId, Stream stream, string fileName, string message)
		{
			return _telegramBotClient.SendPhotoAsync(chatId, new InputOnlineFile(stream, fileName), message);
		}

		public Task<Message> SendMessage(ChatId chatId, string message, InlineKeyboardMarkup replyMarkup = null, ParseMode parseMode = ParseMode.Default)
		{
			return _telegramBotClient.SendTextMessageAsync(chatId, message, replyMarkup: replyMarkup, parseMode: parseMode);
		}

		public Task<Message> EditMessage(ChatId chatId, int messageId, string message, InlineKeyboardMarkup replyMarkup = null)
		{
			return _telegramBotClient.EditMessageTextAsync(chatId, messageId, message, replyMarkup: replyMarkup);
		}

		public async Task PinMessage(ChatId chatId, Message message)
		{
			await _telegramBotClient.PinChatMessageAsync(chatId, message.MessageId);
		}
	}
}