using System.Threading.Tasks;

namespace NetHomeFinder.Storage.Context
{
	public class SqliteDbContextFactory
	{
		public async Task<SqliteDbContext> CreateContext()
		{
			var context = new SqliteDbContext();
			await context.Database.EnsureCreatedAsync();

			return context;
		}
	}
}