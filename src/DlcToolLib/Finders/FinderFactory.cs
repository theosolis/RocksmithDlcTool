using System.Runtime.InteropServices;
using DlcToolLib.Model;

namespace DlcToolLib.Finders
{
	public class FinderFactory
	{
		private const string OfficialDlcDefaultXpath = "//div[@class='tabPanel downloads']/songs";
		private readonly IDlcSortCalculator _dlcSortCalculator;

		public FinderFactory(IDlcSortCalculator dlcSortCalculator)
		{
			_dlcSortCalculator = dlcSortCalculator;
		}

		public OfficialDlcFinder GetDefaultOfficialDlcFinder()
		{
			var remapper = new OfficialDlcRemapper(new RemapOfficialEntries());
			return new OfficialDlcFinder(remapper, OfficialDlcDefaultXpath, _dlcSortCalculator);
		}

		public DlcTuningsFinder GeDlcTuningsDlcFinder()
		{
			return new DlcTuningsFinder(_dlcSortCalculator);
		}

		public ExistingDlcFinder GetExistingDlcFinder()
		{
			return new ExistingDlcFinder(_dlcSortCalculator);
		}
	}
}