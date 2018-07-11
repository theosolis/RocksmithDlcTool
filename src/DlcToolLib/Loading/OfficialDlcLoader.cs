using System.Collections.Generic;
using DlcToolLib.Entities;
using DlcToolLib.Model;
using LiteDB;

namespace DlcToolLib.Loading
{
	public class OfficialDlcLoader : IDlcLoader<OfficialDlcItem>
	{
		public List<string> LoadDlcList(LiteDatabase database, IFindDlcResult<OfficialDlcItem> dlcList, bool clearExistingItemsFirst)
		{
			throw new System.NotImplementedException();
		}
	}
}