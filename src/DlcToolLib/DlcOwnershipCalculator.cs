using DlcToolLib.Entities;
using DlcToolLib.Model;
using System.Linq;
using System;
using System.Collections.Generic;

namespace DlcToolLib
{
	public class DlcOwnershipCalculator
	{
		private readonly IDlcSortCalculator _sortCalculator;
		private readonly IDlcMatchCalculator _dlcMatchCalculator;

		public DlcOwnershipCalculator(IDlcSortCalculator dlcSortCalculator, IDlcMatchCalculator dlcMatchCalculator)
		{
			_sortCalculator = dlcSortCalculator;
			_dlcMatchCalculator = dlcMatchCalculator;
		}

		public DlcOwnership Calculate(OfficialDlcList dlcList, ExistingDlcList existingList)
		{
			var rv = new DlcOwnership();
			var dlcMatches = _dlcMatchCalculator.GetDlcMatches(dlcList.DlcList, existingList.DlcList);

			rv.MissingOfficialDlc = dlcMatches.Where(x=>x.MatchResult == MatchResultType.UnmatchedLeftDlc).Select(x=>x.LeftDlc).ToList();
			rv.UnknownExisting = dlcMatches.Where(x => x.MatchResult == MatchResultType.UnmatchedRightDlc).Select(x => x.RightDlc).ToList();

			rv.SongPacks = CreateSongPackDetails(dlcList.DlcList, dlcMatches);

			return rv;
		}


		private List<SongPack> CreateSongPackDetails(List<OfficialDlcItem> officialDlcList, List<DlcMatch<OfficialDlcItem, ExistingDlcItem>> dlcMatches)
		{
			var rv = new List<SongPack>();
			foreach(var group in dlcMatches.GroupBy(x=>x.UniqueKey).Where(x=>x.Count() > 1))
			{
				Console.WriteLine($"Key {group.Key} found {group.Count()} times");
			}
			var matchesByUniqueKey = dlcMatches.ToDictionary(x => x.UniqueKey);

			foreach (var pack in officialDlcList.GroupBy(x=>x.SongPack))
			{
				var spi = new SongPack
				{
					Name = pack.Key
				};

				foreach(var song in pack)
				{
					var songSort = _sortCalculator.CreateSortDetails(song);
					spi.ItemsInPack.Add(new SongPack.PurchasedStatus { OfficialDlcItem = song, IsPurchased = matchesByUniqueKey[songSort.UniqueKey].MatchResult == MatchResultType.Matched } );
				}

				rv.Add(spi); 
			}

			return rv;
		}
	}

}
