using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RocksmithToolkitLib.PsarcLoader;

namespace DlcToolLib
{
	class MyPsarcBrowser
	{
		private readonly PsarcLoader _psarcLoader;

		public MyPsarcBrowser(string filePath)
		{
			_psarcLoader = new PsarcLoader(filePath);
		}

		public List<string> GetEntryNames()
		{
			return _psarcLoader.ExtractEntryNames();
		}
	}
}
