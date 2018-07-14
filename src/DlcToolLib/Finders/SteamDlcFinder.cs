using System;
using System.Linq;
using System.Text;
using DlcToolLib.Entities;
using DlcToolLib.Model;
using HtmlAgilityPack;

namespace DlcToolLib.Finders
{
	public class SteamDlcFinder : IDlcFinder<SteamDlcItem>
	{
		private readonly IDlcSortCalculator _dlcSortCalculator;
		private readonly SteamDlcDescriptionParser _steamDlcDescriptionParser = new SteamDlcDescriptionParser();

		public SteamDlcFinder(IDlcSortCalculator dlcSortCalculator)
		{
			_dlcSortCalculator = dlcSortCalculator;
		}

		public IFindDlcResult<SteamDlcItem> FindDlc(string sourcePath)
		{
			var doc = DlcHelper.GetHtmlDocument(sourcePath);
			return ParsePage(doc);
		}

		private IFindDlcResult<SteamDlcItem> ParsePage(HtmlDocument doc)
		{
			var rv = new SteamDlcList();

			var value = doc.DocumentNode
				.SelectSingleNode("//div[@class='gameDlcBlocks']");

			if (value == null)
			{
				rv.Errors.Add("Could not find the DLC block inside the page");
				return rv;
			}

			var aNodes = value.SelectNodes("a").ToList();
			aNodes.AddRange(doc.DocumentNode.SelectSingleNode("//div[@id='game_area_dlc_expanded']").SelectNodes("a"));

			var theList = aNodes.Select(MapToSteamDlcItem).Where(x => x != null).ToList();
			Console.WriteLine($"Found {theList.Count} steam items");
			Console.WriteLine($"...of which {theList.Count(x => x.ItemType == SteamDlcItemType.SongPack)} are song packs");
			rv.DlcList.AddRange(theList);
			return rv;
		}

		private SteamDlcItem MapToSteamDlcItem(HtmlNode dlcRow)
		{
			var dlcName = dlcRow.SelectSingleNode("div[@class='game_area_dlc_name']");
			if (dlcName == null)
				return null;

			string dlcUrl = string.Empty;
			if (dlcRow.HasAttributes)
			{
				var href = dlcRow.Attributes.SingleOrDefault(x => x.Name == "href");
				if (href != null)
					dlcUrl = href.DeEntitizeValue;
			}

			var song = _steamDlcDescriptionParser.ParseDlcLinkText(dlcName.InnerText.Trim());
			if (!song.ParsedSuccessfully)
				return null;

			var rv = new SteamDlcItem
			{
				Artist = song.Artist,
				Song = song.SongName,
				SongPack = song.SongPack,
				DlcPageUrl = dlcUrl
			};
			if (!string.IsNullOrWhiteSpace(rv.SongPack))
			{
				rv.ItemType = SteamDlcItemType.SongPack;
			}

			var sortDetails = rv.ItemType == SteamDlcItemType.SongPack
				? _dlcSortCalculator.CreateSortDetailsForSongPack(rv.SongPack)
				: _dlcSortCalculator.CreateSortDetails(rv.Artist, rv.Song);

			rv.ArtistSort = sortDetails.ArtistSort;
			rv.SongSort = sortDetails.SongSort;
			rv.UniqueKey = sortDetails.UniqueKey;

			return rv;
		}
	}
}