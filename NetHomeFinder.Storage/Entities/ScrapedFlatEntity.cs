using System.ComponentModel.DataAnnotations.Schema;

namespace NetHomeFinder.Storage.Entities
{
	public class ScrapedFlatEntity
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public string InternalEstateId { get; set; }
	}
}