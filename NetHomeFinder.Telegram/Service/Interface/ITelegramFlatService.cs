using System.Collections.Generic;
using System.Threading.Tasks;
using NetHomeFinder.Common.Definitions.Models;

namespace NetHomeFinder.Telegram.Service.Interface
{
	public interface ITelegramFlatService
	{
		Task SendNewFlatsUpdate(List<Estate> newFlats);
	}
}