using System.Collections.Generic;
using DlcToolLib.Entities;
using DlcToolLib.Model;
using LiteDB;

namespace DlcToolLib.Loading
{
	public interface IDlcLoader<T>
		where T : IDlc
	{
		List<string> LoadDlcList(LiteDatabase database, IFindDlcResult<T> dlcList, DlcLoadPolicy loadPolicy);
	}
}