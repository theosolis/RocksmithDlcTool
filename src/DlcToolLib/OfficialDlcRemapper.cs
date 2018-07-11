using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DlcToolLib.Entities;
using DlcToolLib.Model;

namespace DlcToolLib
{
	public class OfficialDlcRemapper
	{
		private readonly RemapOfficialEntries _remapOfficialEntries;
		private readonly ILookup<string, RemapOfficialEntries.Entry> _entriesByArtist;

		public OfficialDlcRemapper(RemapOfficialEntries remapOfficialEntries)
		{
			if (remapOfficialEntries.Entries.Any(x => string.IsNullOrWhiteSpace(x.Artist)))
				throw new ApplicationException("Each remapping entry required at least the Artist field specified");

			_remapOfficialEntries = remapOfficialEntries;
			_entriesByArtist = _remapOfficialEntries.Entries.ToLookup(x=> x.Artist);
		}

		public IEnumerable<OfficialDlcItem> GetMissingEntries()
		{
			foreach(var missingEntry in _remapOfficialEntries.AddMissing)
			{
				yield return new OfficialDlcItem 
				{
					Artist = missingEntry.Artist,
					Song = missingEntry.Song,
					SongPack = missingEntry.SongPack
				};
			}
		}

		public OfficialDlcItem Remap(OfficialDlcItem input)
		{
			var remapsForArtist = _entriesByArtist[input.Artist];
			if (!remapsForArtist.Any())
				return input;

			var rv = input.Clone();

			foreach(var remap in remapsForArtist)
			{
				//if the remap has a song requirement, and the song matches, then remap specific details
				if (!string.IsNullOrWhiteSpace(remap.Song))
				{
					if (string.Compare(remap.Song, input.Song, StringComparison.CurrentCultureIgnoreCase) == 0)
					{
						if (!string.IsNullOrWhiteSpace(remap.NewSong))
							rv.Song = remap.NewSong;

						if (!string.IsNullOrWhiteSpace(remap.NewSongPack))
							rv.SongPack = remap.NewSongPack;

						rv.Artist = GetArtist(remap, input.Artist);
					}
				}
				else
				{
					rv.Artist = GetArtist(remap, input.Artist);
				}
			}

			return rv;
		}

		private string GetArtist(RemapOfficialEntries.Entry remap, string originalArtist)
		{
			if (string.IsNullOrWhiteSpace(remap.NewArtist))
				return originalArtist;

			return remap.NewArtist;
		}
	}
}
