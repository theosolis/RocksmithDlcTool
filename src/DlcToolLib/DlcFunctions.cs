using System;
using System.IO;
using DlcToolLib.Entities;
using DlcToolLib.Finders;
using DlcToolLib.Model;
using RocksmithToolkitLib.PsarcLoader;
using System.Collections.Generic;

namespace DlcToolLib
{
	public class DlcFunctions
	{
		public DlcOwnership CalculateOwnership(DlcOwnershipInput input)
		{
			var officialDlcList = GetOfficialDlcList(input.OfficialDlcSource, input.OfficialDlcSongNodeSelector, input.RemapOfficialEntries);
			var existingDlcList = GetExistingDlcList(input.DlcFolder2014, input.DlcFolder2012);

			var stringCleaner = new StringCleaner(true,true,false,false);
			var dlcSortCalculator = new DlcSortCalculator(stringCleaner);
			var dlcMatchCalculator = new DlcMatchCalculator(stringCleaner);
			var dlcOwnershipCalculator = new DlcOwnershipCalculator(dlcSortCalculator, dlcMatchCalculator);

			return dlcOwnershipCalculator.Calculate(officialDlcList, existingDlcList);
		}

		public List<DlcMatch<OfficialDlcItem, DlcTuningItem>> CheckDlcLists(DlcOwnershipInput input)
		{
			var officialDlcList = GetOfficialDlcList(input.OfficialDlcSource, input.OfficialDlcSongNodeSelector, input.RemapOfficialEntries);
			var tuningDlcList = GetTuningDlcList(input.TuningSource);

			var stringCleaner = new StringCleaner(true, true, true, true);
			var dlcMatchCalculator = new DlcMatchCalculator(stringCleaner);
			var dlcMatches = dlcMatchCalculator.GetDlcMatches(officialDlcList.DlcItems, tuningDlcList.DlcTunings);

			return dlcMatches;
		}

		public void WriteAllEntriesFromPsArcFiles(string outputDir, string directoryToSearch)
		{
			var psArcFiles = Directory.GetFiles(directoryToSearch, "*.psarc", SearchOption.AllDirectories);
			foreach (var file in psArcFiles)
			{
				WriteEntriesForFile(outputDir, file);
			}
		}

		private DlcTuningList GetTuningDlcList(string inputSource)
		{
			var dlcTuningsFinder = new DlcTuningsFinder();
			var dlcTuningList = dlcTuningsFinder.GetDlcTuningList(inputSource);
			return dlcTuningList;
		}

		private ExistingDlcList GetExistingDlcList(string rs2014DlcFolder, string rs1DlcFolder)
		{
			var existingFinder = new ExistingDlcFinder();
			var existingList = existingFinder.FindAllDlc(rs2014DlcFolder,rs1DlcFolder);
			return existingList;
		}

		private OfficialDlcList GetOfficialDlcList(string officialDlcSource, string xpathSelector, RemapOfficialEntries remapOfficialEntries)
		{
			var officialDlcRemapper = new OfficialDlcRemapper(remapOfficialEntries);
			var officialDlcFinder = new OfficialDlcFinder(officialDlcRemapper);
			
			return officialDlcFinder.GetOfficialDlcList(officialDlcSource, xpathSelector);
		}

		private void WriteEntriesForFile(string outputDir, string fileToRead)
		{
			var output_root = Path.GetFileNameWithoutExtension(fileToRead);

			using (var arcLoader = new PsarcLoader(fileToRead))
			{
				File.WriteAllLines(Path.Combine(outputDir, $"{output_root}_entries.txt"), arcLoader.ExtractEntryNames());
			}
		}
	}
}