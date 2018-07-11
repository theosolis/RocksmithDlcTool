using System.Collections.Generic;
using LiteDB;

namespace DlcToolLib.Loading
{
	public interface ILoadCoordinator
	{
		List<string> LoadSourceToDatabase(string sourcePath, LiteDatabase database, bool clearExistingItemsFirst);
	}
}