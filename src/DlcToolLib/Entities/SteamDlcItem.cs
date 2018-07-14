namespace DlcToolLib.Entities
{
	public enum SteamDlcItemType
	{
		Song,
		SongPack,
		Other //time savers etc
	}

	public class SteamDlcItem : BaseEntity, IDlc
	{
		public const string TableName = "steamdlcitem";

		public string UniqueKey { get; set; }
		public string Artist { get; set; }
		public string ArtistSort { get; set; }
		public string Song { get; set; }
		public string SongSort { get; set; }
		
		public string SongPack { get; set; }

		public SteamDlcItemType ItemType { get; set; }
		public DlcGameVersionType GameVersion { get; set; }

		public string DlcPageUrl { get; set; }
		public bool HaveVisitedDlcPage { get; set; }
	}
}