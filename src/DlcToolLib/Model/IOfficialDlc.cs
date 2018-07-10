namespace DlcToolLib.Model
{
	public interface IOfficialDlc : IDlc
	{
		string SongPack { get; set; }
		string Year { get; set; }
		string Genre { get; set; }
	}
}