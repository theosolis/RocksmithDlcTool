namespace DlcToolLib.Model
{
	public enum DlcSourceType
	{
		Rs1,
		Rs2014
	}

	public class ExistingDlcItem : IDlc
	{
		public string Artist { get; set; }
		public string Song { get; set; }

		public string PathToFile { get; set; }
		public string Identifier { get; set; }
		public DlcSourceType DlcSource { get; set; }
	}
}