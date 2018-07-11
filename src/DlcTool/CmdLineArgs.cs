namespace DlcTool
{
	public class CmdLineArgs
	{
		public string DlcFolder { get; set; }

		public string rs1DlcFolder { get; set; }

		public string OfficialDlcSource { get; set; }

		public string OutputDirPath { get; set; }

		public string OfficialSourceXPath { get; set; }

		public bool ConvertEncoding { get; set; }

		public string RemapFilePathOfficialToDlc { get; set; }

		public string RemapFilePathOfficialToTunings { get; set; }

		public bool CheckDlc { get; set; }

		public string TuningSource { get; set; }
	}
}