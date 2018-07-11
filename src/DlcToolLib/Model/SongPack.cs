using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DlcToolLib.Entities;

namespace DlcToolLib.Model
{
	public class SongPack
	{
		public string Name { get; set; }
		public int Total => ItemsInPack.Count;
		public int Purchased => ItemsInPack.Count(x => x.IsPurchased);

		public List<PurchasedStatus> ItemsInPack { get; set; } = new List<PurchasedStatus>();

		public class PurchasedStatus
		{
			public OfficialDlcItem OfficialDlcItem { get; set; }
			public bool IsPurchased { get; set; }
		}
	}
}
