using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace NetHomeFinder.WebScraper.Extensions
{
	public static class HtmlNodeExtensions
	{
		public static IEnumerable<HtmlNode> GetNodesByClass(this HtmlNode node, string className)
		{
			return node
				.Descendants()
				.Where(x => x.Attributes["class"]?.Value.Contains(className) ?? false);
		}

		public static HtmlNode GetNodeByClass(this HtmlNode node, string className)
		{
			return node
				.GetNodesByClass(className)
				.FirstOrDefault();
		}
	}
}