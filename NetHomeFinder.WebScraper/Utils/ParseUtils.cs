namespace NetHomeFinder.WebScraper.Utils
{
	public static class ParseUtils
	{
		// Turns something like "770 €" into the number only
		public static float GetPrice(string priceStr)
		{
			var trimmedStr = priceStr
				.Replace(" ", "")
				.Replace("€", "");

			return float.Parse(trimmedStr);
		}

		// Turns something like "53 m²" into the number only
		public static float GetSquareMeters(string squareMetersString)
		{
			var trimmedStr = squareMetersString
				.Replace(" ", "")
				.Replace("m²", "");

			return float.Parse(trimmedStr);
		}
	}
}