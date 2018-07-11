using System.Runtime.InteropServices;
using DlcToolLib.Model;

namespace DlcToolLib.Finders
{
	public class FinderFactory
	{
		private const string OfficialDlcDefaultXpath = "//div[@class='tabPanel downloads']/songs";

		public OfficialDlcFinder GetDefaultOfficialDlcFinder()
		{
			var remapper = new OfficialDlcRemapper(new RemapOfficialEntries());
			return new OfficialDlcFinder(remapper, OfficialDlcDefaultXpath);
		}

		public DlcTuningsDlcFinder GeDlcTuningsDlcFinder()
		{
			return new DlcTuningsDlcFinder();
		}

		public ExistingDlcFinder GetExistingDlcFinder()
		{
			return new ExistingDlcFinder();
		}
	}
}