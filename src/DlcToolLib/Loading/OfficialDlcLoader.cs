using System;
using System.Collections.Generic;
using System.Linq;
using DlcToolLib.Entities;
using DlcToolLib.Model;
using LiteDB;

namespace DlcToolLib.Loading
{
	public class OfficialDlcLoader : IDlcLoader<OfficialDlcItem>
	{
		public List<string> LoadDlcList(LiteDatabase db, IFindDlcResult<OfficialDlcItem> dlcList, bool clearExistingItemsFirst)
		{
			var officialDlc = db.GetCollection<OfficialDlcItem>("officialdlc");

			//this might get trickier once we have mapping tables in place
			if (clearExistingItemsFirst)
			{
				officialDlc.Delete(x => true);
				officialDlc.InsertBulk(dlcList.DlcList);
			}
			else
			{
				var inDb = officialDlc.FindAll();
				Console.WriteLine($"There are {inDb.Count()} official dlc items already in the db");
			}
			return new List<string>();
		}
	}
}