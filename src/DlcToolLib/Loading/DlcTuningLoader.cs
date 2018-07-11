using System.Collections.Generic;
using DlcToolLib.Entities;
using DlcToolLib.Model;
using LiteDB;

namespace DlcToolLib.Loading
{
	public class DlcTuningLoader : IDlcLoader<DlcTuningItem>
	{
		public List<string> LoadDlcList(LiteDatabase database, IFindDlcResult<DlcTuningItem> dlcList, bool clearExistingItemsFirst)
		{
			throw new System.NotImplementedException();
		}
	}
}