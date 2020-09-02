using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using NetHomeFinder.Common.Definitions.Models;
using NetHomeFinder.Common.Extensions;
using NetHomeFinder.WebScraper.Extensions;
using NetHomeFinder.WebScraper.Service.Scrapers.Interface;
using NetHomeFinder.WebScraper.Utils;

namespace NetHomeFinder.WebScraper.Service.Scrapers
{
	public class ImmobilienScoutScraperService : IScrapingService
	{
		private readonly HtmlWeb _htmlWeb;

		private readonly string _immoScoutUrl;

		public ImmobilienScoutScraperService(IConfiguration configuration)
		{
			_htmlWeb = new HtmlWeb();

			_immoScoutUrl = configuration["ImmoscoutUrl"];
		}

		public async Task<IEnumerable<Estate>> GetData()
		{
			try
			{
				var rawData = await GetDataRaw();

				var estates = ParseAndConvert(rawData);

				return estates;
			}
			catch (Exception e)
			{
				"Problem scraping ImmobilienScout".LogWithUtcPrefix();
				Console.WriteLine(e);

				return new List<Estate>();
			}
		}

		private Task<HtmlDocument> GetDataRaw() => _htmlWeb.LoadFromWebAsync(_immoScoutUrl);

		private static IEnumerable<Estate> ParseAndConvert(HtmlDocument document)
		{
			// Drill down the the displayed items, then get the data entries by class
			var resultItems = document
				.GetElementbyId("resultListItems")
				.GetNodesByClass("result-list-entry__data")
				.ToList();

			var estates = resultItems.Select(x =>
			{
				var estate = new Estate();

				AddMetaData(estate, x);
				AddFlatDetails(estate, x);
				AddAddress(estate, x);

				return estate;
			})
			.Where(x => x.Address != null && x.Url != null)
			.ToList();

			return estates;
		}

		private static void AddAddress(Estate estate, HtmlNode node)
		{
			var addressContainer = node.GetNodeByClass("result-list-entry__address");

			estate.Address = addressContainer == null ? "Parsing address failed" : addressContainer.FirstChild.InnerText;
		}

		private static void AddMetaData(Estate estate, HtmlNode node)
		{
			var linkNode = node.GetNodeByClass("result-list-entry__brand-title");

			var flatName = linkNode
				.ChildNodes
				.Elements()
				.FirstOrDefault(x => x.Name.Equals("#text"));

			estate.Name = flatName?.InnerText;
			estate.Url = $"https://www.immobilienscout24.de{linkNode.GetAttributeValue("href", "")}";
			estate.Id = linkNode.GetAttributeValue("data-go-to-expose-id", "");
		}

		private static void AddFlatDetails(Estate estate, HtmlNode node)
		{
			var detailDataNodes = node
				.GetNodeByClass("result-list-entry__criteria")
				.Descendants()
				.Where(x => x.Name.Equals("dd"))
				.ToList();

			var priceNode = detailDataNodes.FirstOrDefault(x => x.InnerText.Contains("€"));

			if (priceNode != null)
			{
				estate.Price = ParseUtils.GetPrice(priceNode.InnerText);
			}

			var squareMetersNode = detailDataNodes.FirstOrDefault(x => x.InnerText.Contains("m²"));

			if (squareMetersNode != null)
			{
				estate.SquareMeters = ParseUtils.GetSquareMeters(squareMetersNode.InnerText);
			}

			var roomsNode = detailDataNodes
				.Except(new List<HtmlNode>() { priceNode, squareMetersNode })
				.FirstOrDefault();

			if (roomsNode != null)
			{
				estate.Rooms = float.Parse(roomsNode
					.GetNodeByClass("onlyLarge")
					.InnerText);
			}
		}
	}
}