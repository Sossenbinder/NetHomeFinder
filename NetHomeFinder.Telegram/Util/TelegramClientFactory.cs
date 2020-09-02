using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using NetHomeFinder.Common.Utils;
using Telegram.Bot;

namespace NetHomeFinder.Telegram.Util
{
	public class TelegramClientFactory : BaseLazyFactory<TelegramBotClient>
	{
		public TelegramClientFactory([NotNull] IConfiguration configuration)
			: base(() => new TelegramBotClient(configuration["TelegramApiToken"]))
		{
		}
	}
}