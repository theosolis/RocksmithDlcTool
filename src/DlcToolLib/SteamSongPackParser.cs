using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DlcToolLib.Entities;
using DlcToolLib.Model;
using Microsoft.Win32;

namespace DlcToolLib
{
	public class SteamSongPackParser
	{
		private const string SongPackSongFinderRegex = @"Play\s(.*)\son any electric guitar or bass";

		public SteamSongPack ParseSteamString(string input)
		{
			var rv = new SteamSongPack {Parsing = string.Empty};

			var cleaned = input.Replace("“", "\"").Replace("”", "\"");
			var match = Regex.Match(cleaned, SongPackSongFinderRegex);
			if (!match.Success)
				return rv;

			var remaining = match.Groups[1];
			rv.Parsing = remaining.Value;

			var songBlocks = BreakIntoSongBlock(remaining.Value);
			foreach (var songBlock in songBlocks)
			{
				var parsedBlock = ParseSongBlock(songBlock);
				if (parsedBlock != null)
					rv.SongsFound.Add(parsedBlock);
			}

			if (rv.SongsFound.Any() && !string.IsNullOrWhiteSpace(rv.SongsFound.Last().Artist))
			{
				var lastArtist = rv.SongsFound.Last().Artist;
				foreach (var missingArtist in rv.SongsFound.Where(x => string.IsNullOrWhiteSpace(x.Artist)))
				{
					missingArtist.Artist = lastArtist;
				}
			}

			return rv;
		}

		private List<string> BreakIntoSongBlock(string input)
		{
			var rv = new List<string>();

			var currentStartingIdx = 0;
			while (currentStartingIdx >= 0 && currentStartingIdx <= input.Length)
			{
				var idxOpeningQuote = input.IndexOf("\"", currentStartingIdx, StringComparison.CurrentCulture);
				if (idxOpeningQuote < 0)
					return rv;

				var idxClosingQuote = input.IndexOf("\"", idxOpeningQuote + 1, StringComparison.CurrentCulture);
				if (idxClosingQuote < 0)
					return rv;

				var nextOpeningQuote = input.IndexOf("\"", idxClosingQuote + 1, StringComparison.CurrentCulture);
				var thisSubstring = nextOpeningQuote > 0
					? input.Substring(currentStartingIdx, nextOpeningQuote - currentStartingIdx)
					: input.Substring(currentStartingIdx);

				rv.Add(thisSubstring);

				currentStartingIdx = nextOpeningQuote;
			}
			return rv;
		}

		private SteamSongPack.SimpleDlc ParseSongBlock(string input)
		{
			var idxOpeningQuote = input.IndexOf("\"", StringComparison.CurrentCulture);
			if (idxOpeningQuote < 0)
				return null;

			var idxClosingQuote = input.LastIndexOf("\"", StringComparison.CurrentCulture);
			if (idxClosingQuote < 0)
				return null;

			var lengthLessLastQuote = idxClosingQuote - idxOpeningQuote - 1;
			var songName = input.Substring(idxOpeningQuote + 1, lengthLessLastQuote);
			var rv = new SteamSongPack.SimpleDlc {Song = songName, Artist = string.Empty};
			if (idxClosingQuote + 1 < input.Length)
			{
				var remaining = input.Substring(idxClosingQuote + 1).Trim();

				if (remaining.StartsWith("by "))
					rv.Artist = CleanUpArtist(remaining.Substring(3));
			}

			return rv;
		}

		private string CleanUpArtist(string remaining)
		{
			if (remaining.EndsWith(" and", StringComparison.CurrentCultureIgnoreCase))
				remaining = remaining.Substring(0, remaining.Length - 4);

			if (remaining.EndsWith(","))
				remaining = remaining.Substring(0, remaining.Length - 1);

			return remaining;
		}
	}
}