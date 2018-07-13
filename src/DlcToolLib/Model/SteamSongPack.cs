using System.Collections.Generic;

namespace DlcToolLib.Model
{
	public class SteamSongPack
	{
		public class SimpleDlc
		{
			public string Artist { get; set; }
			public string Song { get; set; }
		}

		public List<SimpleDlc> SongsFound { get; set; } = new List<SimpleDlc>();
		public string Parsing { get; set; }
	}
}