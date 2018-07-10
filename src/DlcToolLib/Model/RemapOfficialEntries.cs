using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DlcToolLib.Model
{
	public class RemapOfficialEntries : ICarriesErrors
	{
		public List<Entry> Entries { get; set; } = new List<Entry>();
		public List<MissingEntry> AddMissing { get; set; } = new List<MissingEntry>();
		public List<string> Errors { get; set; } = new List<string>();

		public class Entry
		{
			public string Song { get; set; }
			public string Artist { get; set; }

			public string NewSong { get; set; }
			public string NewArtist { get; set; }
			public string NewSongPack { get; set; }
		}

		public class MissingEntry
		{
			public string Song { get; set; }
			public string Artist { get; set; }
			public string SongPack { get; set; }
		}

		public bool Validate()
		{
			if (Entries.Any(x => string.IsNullOrWhiteSpace(x.Artist)))
				Errors.Add("Each entry must specify an Artist");

			return !Errors.Any();
		}
	}
}
