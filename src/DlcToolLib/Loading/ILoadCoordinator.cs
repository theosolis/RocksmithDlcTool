using System.Collections.Generic;
using DlcToolLib.Model;
using LiteDB;

namespace DlcToolLib.Loading
{
	public interface ILoadCoordinator
	{
		List<string> LoadSourceToDatabase(string sourcePath, LiteDatabase database, DlcLoadPolicy loadPolicy);
	}
}