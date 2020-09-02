using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NetHomeFinder.Storage.Entities;

namespace NetHomeFinder.Storage.Context
{
	public class SqliteDbContext : DbContext
	{
		public DbSet<ScrapedFlatEntity> ScrapedFlats { get; set; }

		private const string SQLITE_DB_NAME = "/storage/nethomefinder.db";

		public SqliteDbContext()
		{
			if (!File.Exists(SQLITE_DB_NAME))
			{
				File.Create(SQLITE_DB_NAME);
			}
		}

		protected override void OnConfiguring([NotNull] DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite($"FileName={SQLITE_DB_NAME}", options =>
			{
				options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
				options.CommandTimeout(20);
			});

			base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating([NotNull] ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ScrapedFlatEntity>().ToTable("ScrapedFlats");
			modelBuilder.Entity<ScrapedFlatEntity>(entity => { entity.HasKey(e => e.Id); });

			base.OnModelCreating(modelBuilder);
		}
	}
}