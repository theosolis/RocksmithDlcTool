using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DlcToolLib.Model
{
	public class DlcSortDetails
	{
		public string UniqueKey { get; set; }
		public string ArtistSort { get; set; }
		public string SongSort { get; set; }
	}

	public class SortableItem<T>
	{
		public DlcSortDetails SortDetails { get; set; }
		public T Item { get; set; }
	}


	public class SortableItemComparer<T> : IEqualityComparer<SortableItem<T>>
	{
		public bool Equals(SortableItem<T> x, SortableItem<T> y)
		{
			return x.SortDetails.UniqueKey == y.SortDetails.UniqueKey;
		}

		public int GetHashCode(SortableItem<T> obj)
		{
			return obj.SortDetails.UniqueKey.GetHashCode();
		}
	}
}
