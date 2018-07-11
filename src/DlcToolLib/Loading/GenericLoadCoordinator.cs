using System.Collections.Generic;
using System.Linq;
using DlcToolLib.Entities;
using DlcToolLib.Finders;
using DlcToolLib.Model;
using LiteDB;

namespace DlcToolLib.Loading
{
	public class GenericLoadCoordinator<T> : ILoadCoordinator
		where T: IDlc
	{
		private readonly IDlcFinder<T> _myDlcFinder;
		private readonly IDlcLoader<T> _myLoader;

		public GenericLoadCoordinator(IDlcFinder<T> myDlcFinder, IDlcLoader<T> myLoader)
		{
			_myDlcFinder = myDlcFinder;
			_myLoader = myLoader;
		}

		public List<string> LoadSourceToDatabase(string sourcePath, LiteDatabase database, DlcLoadPolicy loadPolicy)
		{
			var myResults = _myDlcFinder.FindDlc(sourcePath);
			if (myResults.Errors.Any())
				return myResults.Errors;

			return _myLoader.LoadDlcList(database, myResults, loadPolicy);
		}
	}
}