using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NetHomeFinder.Storage.Entities;

namespace NetHomeFinder.Storage.Repository.Interface
{
	public interface IScrapedFlatRepository
	{
		Task<IEnumerable<ScrapedFlatEntity>> GetWhere(Expression<Func<ScrapedFlatEntity, bool>> whereFunc);

		Task<List<ScrapedFlatEntity>> InsertUnknownFlats(List<ScrapedFlatEntity> newValues);
	}
}