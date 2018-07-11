namespace DlcToolLib.Entities
{
	public class OfficialDlcItem : IDlc
	{
		public const string TableName = "officialdlc";

		public int Id { get; set; }
		public string UniqueKey { get; set; }
		public string Artist { get; set; }
		public string ArtistSort { get; set; }
		public string Song { get; set; }
		public string SongSort { get; set; }
		public string SongPack { get; set; }
		public string Year { get; set; }
		public string Genre { get; set; }

		public OfficialDlcItem Clone()
		{
			return new OfficialDlcItem
			{
				Artist = Artist,
				Song = Song,
				SongPack = SongPack,
				Year = Year,
				Genre = Genre
			};
		}
	}
}