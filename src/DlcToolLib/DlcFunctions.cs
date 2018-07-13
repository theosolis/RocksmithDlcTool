using System;
using System.IO;
using DlcToolLib.Entities;
using DlcToolLib.Finders;
using DlcToolLib.Model;
using RocksmithToolkitLib.PsarcLoader;
using System.Collections.Generic;
using System.Linq;
using DlcToolLib.Loading;
using LiteDB;

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
			var dlcMatches = dlcMatchCalculator.GetDlcMatches(officialDlcList.DlcList, tuningDlcList.DlcList);

			return dlcMatches;
		}

		public List<string> LoadSourceToStore(string dbFile, string sourcePath, DlcSourceType sourceType, DlcLoadPolicy loadPolicy)
		{
			using (var db = new LiteDatabase(dbFile))
			{
				var factory = LoadingOracle.GetDefaultLoadCoordinatorFactory();
				var loadCoordinator = factory.CreateLoadCoordinator(sourceType);
				return loadCoordinator.LoadSourceToDatabase(sourcePath, db, loadPolicy);
			}
		}

		public void AttemptLinking()
		{
			using (var db = new LiteDatabase(@"h:\temp\test.db"))
			{
				var officialDlcCol = db.GetCollection<OfficialDlcItem>(OfficialDlcItem.TableName);
				var existingDlcCol = db.GetCollection<ExistingDlcItem>(ExistingDlcItem.TableName);
				var tuningDlcCol = db.GetCollection<DlcTuningItem>(DlcTuningItem.TableName);
				var linksCol = db.GetCollection<LinksBetweenDlcSources>();

				var existingByKey = existingDlcCol.FindAll().ToDictionary(x => x.UniqueKey);
				var tuningDlcByKey = tuningDlcCol.FindAll().ToDictionary(x => x.UniqueKey);

				foreach (var item in officialDlcCol.FindAll())
				{
					var newLink = new LinksBetweenDlcSources {OfficialDlcItem = item};

					if (existingByKey.ContainsKey(item.UniqueKey))
					{
						newLink.ExistingDlcItem = existingByKey[item.UniqueKey];
						existingByKey.Remove(item.UniqueKey);
					}

					if (tuningDlcByKey.ContainsKey(item.UniqueKey))
					{
						newLink.DlcTuningItem = tuningDlcByKey[item.UniqueKey];
						tuningDlcByKey.Remove(item.UniqueKey);
					}

					linksCol.Insert(newLink);
				}

				foreach (var unmatchedExisting in existingByKey)
				{
					var newLink = new LinksBetweenDlcSources {ExistingDlcItem = unmatchedExisting.Value};

					if (tuningDlcByKey.ContainsKey(unmatchedExisting.Key))
					{
						newLink.DlcTuningItem = tuningDlcByKey[unmatchedExisting.Key];
						tuningDlcByKey.Remove(unmatchedExisting.Key);
					}

					linksCol.Insert(newLink);
				}

				foreach (var unmatchedTuningItem in tuningDlcByKey)
				{
					var newLink = new LinksBetweenDlcSources { DlcTuningItem = unmatchedTuningItem.Value };

					linksCol.Insert(newLink);
				}
			}
		}

		private DlcTuningList GetTuningDlcList(string inputSource)
		{
			var dlcTuningsFinder = new DlcTuningsFinder(DlcSortCalculatorOracle.GetDefaultDlcSortCalculator());
			var dlcTuningList = dlcTuningsFinder.GetDlcTuningList(inputSource);
			return dlcTuningList;
		}

		private ExistingDlcList GetExistingDlcList(string rs2014DlcFolder, string rs1DlcFolder)
		{
			var existingFinder = new ExistingDlcFinder(DlcSortCalculatorOracle.GetDefaultDlcSortCalculator());
			var existingList = existingFinder.FindAllDlc(rs2014DlcFolder,rs1DlcFolder);
			return existingList;
		}

		private OfficialDlcList GetOfficialDlcList(string officialDlcSource, string xpathSelector, RemapOfficialEntries remapOfficialEntries)
		{
			var officialDlcRemapper = new OfficialDlcRemapper(remapOfficialEntries);
			var officialDlcFinder = new OfficialDlcFinder(officialDlcRemapper, xpathSelector, DlcSortCalculatorOracle.GetDefaultDlcSortCalculator());
			
			return officialDlcFinder.GetOfficialDlcList(officialDlcSource);
		}
	}
}