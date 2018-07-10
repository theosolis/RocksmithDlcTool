namespace DlcToolLib.Model
{
	public class DlcOwnershipInput
	{
		public string DlcFolder2014 { get; set; }
		public string DlcFolder2012 { get; set; }
		public string TuningSource { get; set; }
		public string OfficialDlcSource { get; set; }
		public string OfficialDlcSongNodeSelector { get; set; }
		public RemapOfficialEntries RemapOfficialEntries { get; set; }
	}
}