namespace DlcToolLib.Entities
{
	public interface IDlc
	{
		string UniqueKey { get; set; }
		string Artist { get; set; }
		string ArtistSort { get; set; }
		string Song { get; set; }
		string SongSort { get; set; }
	}
}