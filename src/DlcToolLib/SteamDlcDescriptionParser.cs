using System.Text.RegularExpressions;
using DlcToolLib.Model;

namespace DlcToolLib
{
	public class SteamDlcDescriptionParser
	{
		private const string RsRemasteredRegex = @"Rocksmith.\s2014\sEdition.* Remastered\s[–-]\s(.*)";
		private const string Rs2014Regex = @"Rocksmith.\s2014\s[–-]\s(.*)";
		private const string Rs1Regex = @"Rocksmith\s[–-]\s(.*)";
		private const string SongRegex = @"(.*Song\sPack.*)|((.*)\s[-]\s[""“]?(.*?)[""”]?$)";

		public SteamDlcDescription ParseDlcLinkText(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
				return new SteamDlcDescription { ParsedSuccessfully = false };

			text = text.Trim();
			var name = StripRocksmithPart(text);
			if (string.IsNullOrWhiteSpace(name))
				return new SteamDlcDescription { ParsedSuccessfully = false };

			return Parse(name);
		}

		private string StripRocksmithPart(string s)
		{
			var match = Regex.Match(s, RsRemasteredRegex);
			if (match.Success)
				return match.Groups[1].Value;

			match = Regex.Match(s, Rs2014Regex);
			if (match.Success)
				return match.Groups[1].Value;

			match = Regex.Match(s, Rs1Regex);
			return match.Success ? match.Groups[1].Value : string.Empty;
		}

		private SteamDlcDescription Parse(string s)
		{
			/*
				--group 1 = song pack
				--group 3 = artist
				--group 4 = song
			*/
			var match = Regex.Match(s, SongRegex);
			if (!match.Success)
				return new SteamDlcDescription {ParsedSuccessfully = true};

			return new SteamDlcDescription
			{
				ParsedSuccessfully = true,
				Artist = match.Groups[3].Value,
				SongPack = match.Groups[1].Value,
				SongName = match.Groups[4].Value
			};
		}
	}
}