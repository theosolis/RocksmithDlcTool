using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DlcToolLib.Model;

namespace DlcToolLib
{
	public interface IDlcMatchCalculator
	{
		List<DlcMatch<TLeft, TRight>> GetDlcMatches<TLeft, TRight>(List<TLeft> leftList, List<TRight> rightList)
			where TLeft : IDlc
			where TRight : IDlc;
	}

	public class DlcMatchCalculator : IDlcMatchCalculator
	{
		private readonly StringCleaner _stringCleaner;
		private readonly DlcSortCalculator _sortCalculator;

		public DlcMatchCalculator(StringCleaner stringCleaner)
		{
			_stringCleaner = stringCleaner;
			_sortCalculator = new DlcSortCalculator(_stringCleaner);
		}

		public List<DlcMatch<TLeft, TRight>> GetDlcMatches<TLeft, TRight>(List<TLeft> leftList, List<TRight> rightList)
			where TLeft: IDlc
			where TRight: IDlc
		{
			var rv = new List<DlcMatch<TLeft, TRight>>();

			var rightListByKey = _sortCalculator.ConstructSortableItemList(rightList).Distinct(new SortableItemComparer<TRight>()).ToDictionary(x => x.SortDetails.UniqueKey);
			var preparedLeftList = _sortCalculator.ConstructSortableItemList(leftList).Distinct(new SortableItemComparer<TLeft>()).ToList();

			foreach (var leftDlc in preparedLeftList)
			{
				var uniqueKey = leftDlc.SortDetails.UniqueKey;
				var matchResult = new DlcMatch<TLeft, TRight> { UniqueKey = uniqueKey, LeftDlc = leftDlc.Item };


				if (rightListByKey.ContainsKey(uniqueKey))
				{
					matchResult.RightDlc = rightListByKey[uniqueKey].Item;
					matchResult.MatchResult = MatchResultType.Matched;
					rightListByKey.Remove(uniqueKey);
				}
				else
				{
					matchResult.MatchResult = MatchResultType.UnmatchedLeftDlc;
				}
				rv.Add(matchResult);
			}

			foreach (var existing in rightListByKey.Values)
			{
				rv.Add(new DlcMatch<TLeft, TRight>
				{
					UniqueKey = existing.SortDetails.UniqueKey,
					MatchResult = MatchResultType.UnmatchedRightDlc,
					RightDlc = existing.Item
				});
			}

			return rv;
		}
	}
}
