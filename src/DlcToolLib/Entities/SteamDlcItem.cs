namespace DlcToolLib.Entities
{
	public class SteamDlcItem : BaseEntity, IDlc
	{
		public string UniqueKey { get; set; }
		public string Artist { get; set; }
		public string ArtistSort { get; set; }
		public string Song { get; set; }
		public string SongSort { get; set; }
	}
}