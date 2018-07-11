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
	public class OfficialDlcFinder
	{
		private readonly OfficialDlcRemapper _dlcRemapper;

		public OfficialDlcFinder(OfficialDlcRemapper officialDlcRemapper)
		{
			_dlcRemapper = officialDlcRemapper;
		}

		public OfficialDlcList GetOfficialDlcList(string sourcePath, string songsXPath)
		{
			var doc = DlcHelper.GetHtmlDocument(sourcePath);
			return ParsePage(doc, songsXPath);
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
