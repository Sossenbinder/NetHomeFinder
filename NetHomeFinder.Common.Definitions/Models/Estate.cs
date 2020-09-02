using NetHomeFinder.Common.Definitions.Enums;

namespace NetHomeFinder.Common.Definitions.Models
{
	public class Estate
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string Address { get; set; }

		public float SquareMeters { get; set; }

		public float Rooms { get; set; }

		public float Price { get; set; }

		public string Url { get; set; }

		public EstateSource Source { get; set; }

		public string InternalEstateId => $"{Source}_{Id}";
	}
}