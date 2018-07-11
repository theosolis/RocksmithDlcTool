using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using DlcToolLib.Entities;
using DlcToolLib.Model;

namespace DlcToolLib.Finders
{
	public class OfficialDlcFinder : IDlcFinder<OfficialDlcItem>
	{
		private readonly OfficialDlcRemapper _dlcRemapper;
		private readonly string _songsXPath;
		private readonly IDlcSortCalculator _dlcSortCalculator;

		public OfficialDlcFinder(OfficialDlcRemapper officialDlcRemapper, string songsXPath, IDlcSortCalculator dlcSortCalculator)
		{
			_dlcRemapper = officialDlcRemapper;
			_songsXPath = songsXPath;
			_dlcSortCalculator = dlcSortCalculator;
		}

		public IFindDlcResult<OfficialDlcItem> FindDlc(string sourcePath)
		{
			return GetOfficialDlcList(sourcePath);
		}

		public OfficialDlcList GetOfficialDlcList(string sourcePath)
		{
			var doc = DlcHelper.GetHtmlDocument(sourcePath);
			return ParsePage(doc, _songsXPath);
		}


		private List<OfficialDlcItem> RemapOfficialDlc(IEnumerable<OfficialDlcItem> dlcList)
		{
			var remapped = dlcList.Select(x => _dlcRemapper.Remap(x)).ToList();
			remapped.AddRange(_dlcRemapper.GetMissingEntries());
			return remapped;
		}

		private OfficialDlcList ParsePage(HtmlDocument doc, string songsXPath)
		{
			var rv = new OfficialDlcList();

			var value = doc.DocumentNode
				.SelectNodes(songsXPath)
				.FirstOrDefault();

			if (value == null)
			{
				rv.Errors.Add("Could not find the list of official DLC inside the page");
				return rv;
			}

			var rawList =
				from dlcRow in value.SelectNodes("song")
				select MapToOfficialDlcItem(dlcRow);

			rv.DlcList.AddRange(RemapOfficialDlc(rawList));

			return rv;
		}

		private OfficialDlcItem MapToOfficialDlcItem(HtmlNode dlcRow)
		{
			var rv = new OfficialDlcItem
			{
				Song = GetField(dlcRow, "track-name"),
				Artist = GetField(dlcRow, "artist"),
				Genre = GetField(dlcRow, "genre"),
				Year = GetField(dlcRow, "year"),
				SongPack = GetField(dlcRow, "song-pack")
			};
			var sortDetails = _dlcSortCalculator.CreateSortDetails(rv.Artist, rv.Song);
			rv.ArtistSort = sortDetails.ArtistSort;
			rv.SongSort = sortDetails.SongSort;
			rv.UniqueKey = sortDetails.UniqueKey;

			return rv;
		}

		private string GetField(HtmlNode dlcRow, string tagName)
		{
			var childNode = dlcRow.ChildNodes.SingleOrDefault(x => x.Name == tagName);
			if (childNode == null)
				return string.Empty;

			return childNode.InnerText;
		}
	}
}
