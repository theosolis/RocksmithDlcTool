namespace DlcToolLib.Entities
{
	public class OfficialDlcItem : IDlc
	{
		public int Id { get; set; }
		public string Artist { get; set; }
		public string Song { get; set; }
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