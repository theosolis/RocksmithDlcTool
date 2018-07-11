namespace DlcToolLib.Entities
{
	public enum DlcGameVersionType
	{
		Rs1,
		Rs2014
	}

	public class ExistingDlcItem : IDlc
	{
		public const string TableName = "existingdlcitem";

		public int Id { get; set; }
		public string UniqueKey { get; set; }
		public string Artist { get; set; }
		public string ArtistSort { get; set; }
		public string Song { get; set; }
		public string SongSort { get; set; }

		public string PathToFile { get; set; }
		public string Identifier { get; set; }
		public DlcGameVersionType DlcSource { get; set; }
	}
}