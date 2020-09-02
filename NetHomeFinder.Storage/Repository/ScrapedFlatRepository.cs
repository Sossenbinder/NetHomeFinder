using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetHomeFinder.Storage.Context;
using NetHomeFinder.Storage.Entities;
using NetHomeFinder.Storage.Repository.Interface;

namespace NetHomeFinder.Storage.Repository
{
	public class ScrapedFlatRepository : IScrapedFlatRepository
	{
		private readonly SqliteDbContextFactory _dbContextFactory;

		public ScrapedFlatRepository(SqliteDbContextFactory dbContextFactory)
		{
			_dbContextFactory = dbContextFactory;
		}

		public async Task<IEnumerable<ScrapedFlatEntity>> GetWhere(Expression<Func<ScrapedFlatEntity, bool>> whereFunc)
		{
			await using (var ctx = await _dbContextFactory.CreateContext())
			{
				return await ctx
					.ScrapedFlats
					.Where(whereFunc)
					.ToListAsync();
			}
		}

		public async Task<List<ScrapedFlatEntity>> InsertUnknownFlats(List<ScrapedFlatEntity> newValues)
		{
			await using (var ctx = await _dbContextFactory.CreateContext())
			{
				var unknownFlats = newValues
					.Where(x => !ctx.ScrapedFlats.Any(y => y.InternalEstateId == x.InternalEstateId))
					.ToList();

				ctx.ScrapedFlats.AddRange(unknownFlats);

				await ctx.SaveChangesAsync();

				return unknownFlats;
			}
		}
	}
}