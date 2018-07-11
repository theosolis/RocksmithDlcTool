using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DlcToolLib.Entities;
using DlcToolLib.Model;
using HtmlAgilityPack;

namespace DlcToolLib.Finders
{
	public class DlcTuningsFinder : IDlcFinder<DlcTuningItem>
	{
		private const int DlcTableCellSong = 0;
		private const int DlcTableCellArtist = 1;
		private const int DlcTableCellLeadTuning = 2;
		private const int DlcTableCellRhythmTuning = 3;
		private const int DlcTableCellBassTuning = 4;

		private IDlcSortCalculator _dlcSortCalculator;

		public DlcTuningsFinder(IDlcSortCalculator dlcSortCalculator)
		{
			_dlcSortCalculator = dlcSortCalculator;
		}

		public IFindDlcResult<DlcTuningItem> FindDlc(string sourcePath)
		{
			return GetDlcTuningList(sourcePath);
		}

		public DlcTuningList GetDlcTuningList(string sourcePath)
		{
			var doc = DlcHelper.GetHtmlDocument(sourcePath);
			return ParsePage(doc);
		}

		private DlcTuningList ParsePage(HtmlDocument doc)
		{
			var rv = new DlcTuningList();

			var value = doc.DocumentNode
				.SelectNodes("//article/div[@class='entry-content']/table")
				.FirstOrDefault();

			if (value == null)
			{
				rv.Errors.Add("Could not find the DLC table inside the page");
				return rv;
			}

			var rawList =
				from dlcRow in value.SelectNodes("tbody/tr")
				select MapToOfficialDlcItem(dlcRow);

			//want unique per artist - currently the DlcTuning page has duplicates in it!
			foreach (var artist in rawList.GroupBy(x => new {x.Artist, x.Song}))
			{
				rv.DlcList.Add(artist.First());
			}
			return rv;
		}

		private DlcTuningItem MapToOfficialDlcItem(HtmlNode dlcRow)
		{
			var tableCells = dlcRow.ChildNodes.Where(x => x.Name == "td").ToList();
			var rv = new DlcTuningItem
			{
				Song = GetChildCellText(tableCells, DlcTableCellSong),
				Artist = GetChildCellText(tableCells, DlcTableCellArtist),
				LeadTuning = GetChildCellText(tableCells, DlcTableCellLeadTuning),
				RhythmTuning = GetChildCellText(tableCells, DlcTableCellRhythmTuning),
				BassTuning = GetChildCellText(tableCells, DlcTableCellBassTuning)
			};

			var sortDetails = _dlcSortCalculator.CreateSortDetails(rv.Artist, rv.Song);
			rv.ArtistSort = sortDetails.ArtistSort;
			rv.SongSort = sortDetails.SongSort;
			rv.UniqueKey = sortDetails.UniqueKey;
			return rv;
		}

		private string GetChildCellText(List<HtmlNode> tableCells, int dlcTableCellNumber)
		{
			if (tableCells.Count < dlcTableCellNumber) return null;
			return WebUtility.HtmlDecode(tableCells[dlcTableCellNumber].InnerText).Trim();
		}
	}
}
//entry-content