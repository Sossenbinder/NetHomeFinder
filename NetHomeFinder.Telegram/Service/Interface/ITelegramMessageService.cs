using System.IO;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace NetHomeFinder.Telegram.Service.Interface
{
	public interface ITelegramMessageService
	{
		Task DeleteMessage(ChatId chatId, int messageId);

		Task<Message> SendPhoto(ChatId chatId, Stream stream, string fileName, string message);

		Task<Message> SendMessage(ChatId chatId, string message, InlineKeyboardMarkup replyMarkup = null, ParseMode parseMode = ParseMode.Default);

		Task<Message> EditMessage(ChatId chatId, int accountId, string message, InlineKeyboardMarkup replyMarkup = null);

		Task PinMessage(ChatId chatId, Message message);
	}
}