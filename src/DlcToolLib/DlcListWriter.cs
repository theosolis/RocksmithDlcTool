using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DlcToolLib.Entities;
using DlcToolLib.Model;

namespace DlcToolLib
{
	public class DlcListWriter
	{
		private static readonly char[] CharactersToEscape = { '"', ',', '\r', '\n' };
		private readonly StringCleaner _stringCleaner;

		public DlcListWriter(StringCleaner stringCleaner)
		{
			_stringCleaner = stringCleaner;
		}

		public void WriteOfficialDlcToFile(List<OfficialDlcItem> officialDlcItems, string filePath)
		{
			using (var file = new StreamWriter(filePath))
			{
				file.WriteLine("Song,Artist,Pack");
				foreach (var missing in officialDlcItems.OrderBy(x=>x.SongPack).ThenBy(x=>x.Artist).ThenBy(x=>x.Song))
				{
					file.WriteLine($"{PrepareField(missing.Song)},{PrepareField(missing.Artist)},{PrepareField(missing.SongPack)}");
				}
			}
		}

		public void WriteSongPackInfoToFile(List<SongPack> songPacks, string filePath)
		{
			using (var file = new StreamWriter(filePath))
			{
				file.WriteLine("Song Pack,Total,Purchased,Status,Missing Artist,Missing Song");
				foreach (var songPack in songPacks.OrderBy(x => x.Name))
				{
					file.WriteLine($"{PrepareField(songPack.Name)},{songPack.Total},{songPack.Purchased},{GetStatus(songPack.Total,songPack.Purchased)}");
					if (songPack.Purchased > 0 && songPack.Total != songPack.Purchased)
					{
						foreach(var song in songPack.ItemsInPack.Where(x=>!x.IsPurchased))
						{
							file.WriteLine($",,,,{PrepareField(song.OfficialDlcItem.Artist)},{PrepareField(song.OfficialDlcItem.Song)}");
						}
					}
				}
			}
		}
		private string GetStatus(int total, int purchased)
		{
			if (purchased == 0) return "Not Purchased";
			if (purchased == total) return "Purchased";
			return "Partial";
		}

		public void WriteDlcListToFile<T>(List<T> dlcItems, string filePath) where T:IDlc
		{
			using (var file = new StreamWriter(filePath))
			{
				file.WriteLine("Song,Artist");
				foreach (var song in dlcItems)
				{
					file.WriteLine($"{PrepareField(song.Song)},{PrepareField(song.Artist)}");
				}
			}
		}

		private string PrepareField(string s)
		{
			if (s == null)
				return String.Empty;

			s = _stringCleaner.Clean(s);

			if (s.IndexOfAny(CharactersToEscape) < 0)
				return s;

			var safe = s.Replace("\"", "\"\"").Replace("\r", "").Replace("\n", "");

			return $"\"{safe}\"";
		}
	}
}