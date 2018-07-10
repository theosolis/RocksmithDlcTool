using CommandLine;
using CommandLine.Text;

namespace DlcTool
{
	public class CmdLineArgs
	{
		[Option(Required = true, HelpText = "Path to dlc folder for Rocksmith")]
		public string DlcFolder { get; set; }

		[Option("rs1dlcfolder", HelpText = "Path to RS1 dlc")]
		public string rs1DlcFolder { get; set; }

		[Option("source", Required = true, HelpText = "Where to find the list of Rocksmith dlc")]
		public string OfficialDlcSource { get; set; }

		[Option("outputdir", Required = true, HelpText = "Where to write the output files")]
		public string OutputDirPath { get; set; }

		[Option(Default = "//div[@class='tabPanel downloads']/songs", HelpText ="XPath to find the <songs> node in the official source")]
		public string OfficialSourceXPath { get; set; }

		[Option("convert", Default =false, HelpText = "Try to convert unicode characters to ASCII where possible (eg ’ to ')")]
		public bool ConvertEncoding { get; set; }

		[Option("dlcmap", Default= "OfficialEntriesToDlc.xml", HelpText="Path to the mapping file for helping map official dlc list to dlc items")]
		public string RemapFilePathOfficialToDlc { get; set; }

		[Option("tuningmap", Default = "OfficialEntriesToTunings.xml", HelpText = "Path to the mapping file for helping map official dlc list to tunings list")]
		public string RemapFilePathOfficialToTunings { get; set; }

		[Option("checkdlc")]
		public bool CheckDlc { get; set; }

		[Option("tunings")]
		public string TuningSource { get; set; }
	}
}