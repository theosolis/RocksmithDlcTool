namespace DlcToolLib.Entities
{
	public class SteamDlcItem : BaseEntity, IDlc
	{
		public const string TableName = "steamdlcitem";

		public string UniqueKey { get; set; }
		public string Artist { get; set; }
		public string ArtistSort { get; set; }
		public string Song { get; set; }
		public string SongSort { get; set; }
		
		public string SongPack { get; set; }
		public string SongPackUrl { get; set; }

		public string DlcPageUrl { get; set; }

		public bool ThisIsASongPackEntry { get; set; }
	}
}