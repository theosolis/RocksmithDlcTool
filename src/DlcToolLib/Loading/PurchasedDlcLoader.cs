using System.Collections.Generic;
using DlcToolLib.Entities;
using DlcToolLib.Model;
using LiteDB;

namespace DlcToolLib.Loading
{
	public class PurchasedDlcLoader : IDlcLoader<ExistingDlcItem>
	{
		public List<string> LoadDlcList(LiteDatabase database, IFindDlcResult<ExistingDlcItem> dlcList, bool clearExistingItemsFirst)
		{
			throw new System.NotImplementedException();
		}
	}
}