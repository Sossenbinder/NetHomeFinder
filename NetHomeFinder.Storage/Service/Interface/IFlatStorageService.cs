using System.Collections.Generic;
using System.Threading.Tasks;
using NetHomeFinder.Common.Definitions.Models;

namespace NetHomeFinder.Storage.Service.Interface
{
	public interface IFlatStorageService
	{
		Task HandleScrapedFlats(List<Estate> estates);
	}
}