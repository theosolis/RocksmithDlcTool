using System.Text.RegularExpressions;
using DlcToolLib.Entities;
using DlcToolLib.Model;

namespace DlcToolLib
{
	public class SteamDlcDescriptionParser
	{
		private const string RsRemasteredRegex = @"Rocksmith.\s2014\sEdition.* Remastered\s[–-]\s(.*)";
		private const string Rs2014Regex = @"Rocksmith.\s2014\s[–-]\s(.*)";
		private const string Rs1Regex = @"Rocksmith\s[–-]\s(.*)";

		private const string SongRegex = @"((.*)\s[-]\s[""“]?(.*?)[""”]?$)";
		public const string SongPackRegex = @"(.*Song\sPack.*)";
		private const string SaversAndGearPackRegex = @"(.*)((Time\sSaver\s(Pack|Bundle))|(Gear\sPack))$";

		public SteamDlcDescription ParseDlcLinkText(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
				return new SteamDlcDescription { ParsedSuccessfully = false };

			text = text.Trim();
			var name = StripRocksmithPart(text);
			if (string.IsNullOrWhiteSpace(name.name))
				return new SteamDlcDescription { ParsedSuccessfully = false };

			return Parse(name.name, name.version);
		}

		private (string name, DlcGameVersionType version) StripRocksmithPart(string s)
		{
			var match = Regex.Match(s, RsRemasteredRegex);
			if (match.Success)
				return (match.Groups[1].Value, DlcGameVersionType.Rs2014);

			match = Regex.Match(s, Rs2014Regex);
			if (match.Success)
				return (match.Groups[1].Value, DlcGameVersionType.Rs2014);

			match = Regex.Match(s, Rs1Regex);
			return match.Success ? (match.Groups[1].Value, DlcGameVersionType.Rs1) : (string.Empty, DlcGameVersionType.Rs1);
		}

		private SteamDlcDescription Parse(string s, DlcGameVersionType gameVersion)
		{
			var rv = new SteamDlcDescription
			{
				ParsedSuccessfully = true,
				GameVersion = gameVersion,
				DlcName = s
			};

			var match = Regex.Match(s, SaversAndGearPackRegex);
			if (match.Success)
			{
				rv.ItemType = SteamDlcItemType.Other;
				return rv;
			}

			match = Regex.Match(s, SongPackRegex);
			if (match.Success)
			{
				rv.ItemType = SteamDlcItemType.SongPack;
				rv.SongPack = match.Groups[1].Value;
				return rv;
			}

			/*
				--group 1 = song pack
				--group 3 = artist
				--group 4 = song
			*/
			match = Regex.Match(s, SongRegex);
			if (match.Success)
			{
				rv.ItemType = SteamDlcItemType.Song;
				rv.Artist = match.Groups[2].Value;
				rv.SongName = match.Groups[3].Value;
				return rv;
			}

			rv.Artist = s;
			rv.SongName = s;

			return rv;
		}
	}
}