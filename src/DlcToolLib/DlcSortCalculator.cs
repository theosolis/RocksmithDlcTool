using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DlcToolLib.Entities;
using DlcToolLib.Model;
using System.Text.RegularExpressions;

namespace DlcToolLib
{
	public interface IDlcSortCalculator
	{
		DlcSortDetails CreateSortDetails(IDlc dlc);
	}

	public class DlcSortCalculator : IDlcSortCalculator
	{
		private readonly StringCleaner _stringCleaner;
		private const string DlcUniqueSeparator = "---";
		private const string reBrackets = @"(\(.*\))|(\[.*\])";

		public DlcSortCalculator(StringCleaner stringCleaner)
		{
			_stringCleaner = stringCleaner;
		}

		public DlcSortDetails CreateSortDetails(IDlc dlc)
		{
			var artistSort = _stringCleaner.MakeArtistSortName(dlc.Artist);
			var songSort = _stringCleaner.MakeSongSortName(dlc.Song);
			var uniqueKey = GetUniqueKey(artistSort, songSort);

			return new DlcSortDetails
			{
				UniqueKey = uniqueKey,
				ArtistSort = artistSort,
				SongSort = songSort
			};
		}

		public List<SortableItem<T>> ConstructSortableItemList<T>(IEnumerable<T> itemsToSort) where T:IDlc
		{
			var rv = new List<SortableItem<T>>();

			foreach (var item in itemsToSort)
			{
				rv.Add(new SortableItem<T> 
				{
					Item = item,
					SortDetails = CreateSortDetails(item)
				});
			}

			return rv;
		}

		private string GetUniqueKey(string artistSort, string songSort)
		{
			return string.Concat(PrepareForUnique(artistSort), DlcUniqueSeparator, PrepareForUnique(songSort));
		}

		private static string PrepareForUnique(string s)
		{
			var sPrime = Regex.Replace(s, reBrackets, string.Empty).Trim();

			return sPrime;
		}
	}
}
